using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static InventoryUI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Battle UI Root")]
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject inventoryUI;


    [Header("Enemy Display")]
    [SerializeField] private RawImage enemyRawImage;   // assign a RawImage in top-right of battle UI
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyHPText;

    [Header("Enemy Preview Camera")]
    [SerializeField] private Camera previewCamera;     // dedicated camera, Culling Mask = EnemyPreview layer only
    [SerializeField] private Vector3 previewCameraOffset = new Vector3(0f, 1f, -3f);

    [Header("Player Display")]
    [SerializeField] private TextMeshProUGUI playerHPText;

    [Header("Battle Log")]
    [SerializeField] private TextMeshProUGUI battleLogText;

    [Header("Action Buttons")]
    [SerializeField] private Button itemButton;
    [SerializeField] private Button fleeButton;

    public bool IsInBattle { get; private set; }

    private int _turn = -1;
    private bool _awaitingPlayerAction;
    private int _playerComboCount = 0;

    private EnemyData _currentEnemy;
    private int _enemyCurrentHP;
    private GameObject _enemyObject;

    // Stores (attacker, damage) pairs for the current battle.
    private System.Collections.Generic.List<(string attacker, int damage)> _turnHistory
        = new System.Collections.Generic.List<(string, int)>();

    private RenderTexture _previewRT;
    private GameObject _previewClone;
    private const string PreviewLayer = "EnemyPreview";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        battleUI.SetActive(false);
        if (previewCamera != null) previewCamera.gameObject.SetActive(false);
    }

    public void StartBattle(EnemyData enemy, GameObject enemyObject)
    {
        if (IsInBattle) return;

        _currentEnemy = enemy;
        _enemyObject = enemyObject;
        _enemyCurrentHP = enemy != null ? enemy.maxHP : 0;

        _playerComboCount = 0;
        _turnHistory.Clear();
        IsInBattle = true;
        battleUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = false;

        SetupEnemyPreview(enemyObject);
        RefreshEnemyUI();
        RefreshPlayerUI();
        SetLog($"A wild {(_currentEnemy != null ? _currentEnemy.enemyName : "enemy")} appeared!");
        SetActionsInteractable(true);
        _awaitingPlayerAction = true;
        _turn = 0;
    }

    public void EndBattle()
    {
        if (!IsInBattle) return;

        IsInBattle = false;
        _turn = -1;
        _awaitingPlayerAction = false;

        if (_turnHistory.Count > 0)
            Debug.Log($"[Battle] Summary:\n{BuildTurnSummary(0)}");

        TeardownEnemyPreview();
        battleUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = true;
    }

    // Called by WebSocketClient when an attack gesture is received
    public void PlayerAttack(int damage)
    {
        if (!IsInBattle || !_awaitingPlayerAction || _turn != 0) return;
        _awaitingPlayerAction = false;
        SetActionsInteractable(false);
        StartCoroutine(PlayerAttackRoutine(damage));
    }


    // Called by Flee button
    public void FleeBattle()
    {
        if (!IsInBattle) return;
        SetLog("You fled from battle!");
        StartCoroutine(EndAfterDelay(1f));
    }

    // Recursively multiplies damage by 1.1 for each combo hit beyond the first (max depth = comboCount).
    private float ComboMultiplier(int comboCount)
    {
        if (comboCount <= 1) return 1f;
        return 1.1f * ComboMultiplier(comboCount - 1);
    }

    private IEnumerator PlayerAttackRoutine(int damage)
    {
        _playerComboCount++;
        int finalDamage = Mathf.RoundToInt(damage * ComboMultiplier(_playerComboCount));

        _turnHistory.Add(("Player", finalDamage));
        _enemyCurrentHP = Mathf.Max(0, _enemyCurrentHP - finalDamage);
        SetLog($"You dealt {finalDamage} damage to {_currentEnemy.enemyName}!" +
               (_playerComboCount > 1 ? $" (x{_playerComboCount} combo!)" : ""));
        RefreshEnemyUI();
        yield return new WaitForSeconds(1.2f);

        if (_enemyCurrentHP <= 0)
        {
            int reward = _currentEnemy.coinReward;
            PlayerWallet.Instance?.AddCoins(reward);
            SetLog($"{_currentEnemy.enemyName} was defeated! You earned {reward} coins!");
            yield return new WaitForSeconds(1f);
            EndBattle();
            yield break;
        }

        _turn = 1;
        yield return StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        int dmg = _currentEnemy != null ? _currentEnemy.attackDamage : 5;
        _turnHistory.Add((_currentEnemy.enemyName, dmg));
        SetLog($"{_currentEnemy.enemyName} attacks!");
        yield return new WaitForSeconds(1f);

        PlayerStats.Instance?.TakeDamage(dmg);
        SetLog($"{_currentEnemy.enemyName} dealt {dmg} damage to you!");
        RefreshPlayerUI();
        yield return new WaitForSeconds(1f);

        if (PlayerStats.Instance != null && PlayerStats.Instance.CurrentHP <= 0)
        {
            SetLog("You fainted...");
            yield return new WaitForSeconds(1.2f);
            EndBattle();
            yield break;
        }

        _turn = 0;
        _awaitingPlayerAction = true;
        SetActionsInteractable(true);
        SetLog("Your turn! Use a gesture to attack.");
    }

    private IEnumerator EndAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndBattle();
    }

    // --- Enemy preview via RenderTexture ---

    private void SetupEnemyPreview(GameObject enemyObject)
    {
        if (previewCamera == null || enemyRawImage == null || enemyObject == null) return;

        int previewLayerIndex = LayerMask.NameToLayer(PreviewLayer);
        if (previewLayerIndex < 0)
        {
            Debug.LogWarning($"[BattleManager] Layer '{PreviewLayer}' not found. Add it in Project Settings > Tags and Layers.");
            return;
        }

        // Clone the enemy mesh into a hidden preview position
        _previewClone = Instantiate(enemyObject, new Vector3(0f, -1000f, 0f), enemyObject.transform.rotation);
        SetLayerRecursive(_previewClone, previewLayerIndex);

        // Position preview camera behind and slightly above the clone
        previewCamera.transform.position = _previewClone.transform.position + previewCameraOffset;
        previewCamera.transform.LookAt(_previewClone.transform.position + Vector3.up * 0.5f);
        previewCamera.cullingMask = 1 << previewLayerIndex;

        // Create RenderTexture sized to the RawImage rect
        int w = Mathf.Max(1, (int)enemyRawImage.rectTransform.rect.width);
        int h = Mathf.Max(1, (int)enemyRawImage.rectTransform.rect.height);
        _previewRT = new RenderTexture(w, h, 16);
        previewCamera.targetTexture = _previewRT;
        enemyRawImage.texture = _previewRT;

        previewCamera.gameObject.SetActive(true);
    }

    private void TeardownEnemyPreview()
    {
        if (previewCamera != null)
        {
            previewCamera.targetTexture = null;
            previewCamera.gameObject.SetActive(false);
        }

        if (enemyRawImage != null)
            enemyRawImage.texture = null;

        if (_previewRT != null)
        {
            _previewRT.Release();
            Destroy(_previewRT);
            _previewRT = null;
        }

        if (_previewClone != null)
        {
            Destroy(_previewClone);
            _previewClone = null;
        }
    }

    // Recursively builds a summary string from the turn history list.
    private string BuildTurnSummary(int index)
    {
        if (index >= _turnHistory.Count) return "";
        var (attacker, dmg) = _turnHistory[index];
        return $"  Turn {index + 1}: {attacker} dealt {dmg}\n" + BuildTurnSummary(index + 1);
    }

    private void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
            SetLayerRecursive(child.gameObject, layer);
    }

    // --- UI helpers ---

    private void RefreshEnemyUI()
    {
        if (_currentEnemy == null) return;
        if (enemyNameText != null) enemyNameText.text = _currentEnemy.enemyName;
        if (enemyHPText != null) enemyHPText.text = $"HP: {_enemyCurrentHP}/{_currentEnemy.maxHP}";
    }

    private void RefreshPlayerUI()
    {
        if (playerHPText == null || PlayerStats.Instance == null) return;
        playerHPText.text = $"Player HP: {PlayerStats.Instance.CurrentHP}/{PlayerStats.Instance.maxHP}";
    }

    private void SetLog(string message)
    {
        if (battleLogText != null) battleLogText.text = message;
        Debug.Log($"[Battle] {message}");
    }

    private void SetActionsInteractable(bool interactable)
    {
        if (itemButton != null) itemButton.interactable = interactable;
        if (fleeButton != null) fleeButton.interactable = interactable;
    }
}
