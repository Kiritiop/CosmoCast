using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Battle UI Root")]
    [SerializeField] private GameObject battleUI;

    [Header("Enemy Display")]
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyHPText;

    [Header("Player Display")]
    [SerializeField] private TextMeshProUGUI playerHPText;

    [Header("Battle Log")]
    [SerializeField] private TextMeshProUGUI battleLogText;

    [Header("Action Buttons")]
    [SerializeField] private Button itemButton;
    [SerializeField] private Button fleeButton;

    public bool IsInBattle { get; private set; }

    // -1 = not started, 0 = player turn, 1 = enemy turn
    private int _turn = -1;
    private bool _awaitingPlayerAction;

    private EnemyData _currentEnemy;
    private int _enemyCurrentHP;
    private GameObject _enemyObject;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        battleUI.SetActive(false);
    }

    // Called by EnemyEncounterTrigger
    public void StartBattle(EnemyData enemy, GameObject enemyObject)
    {
        if (IsInBattle) return;

        _currentEnemy = enemy;
        _enemyObject = enemyObject;
        _enemyCurrentHP = enemy != null ? enemy.maxHP : 0;

        IsInBattle = true;
        battleUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = false;

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

    // Called by Item button
    public void UseItem()
    {
        if (!IsInBattle || !_awaitingPlayerAction || _turn != 0) return;
        // Placeholder — hook up to InventoryManager when item use in battle is ready
        SetLog("You rummaged through your bag... (no usable items yet)");
    }

    // Called by Flee button
    public void FleeBattle()
    {
        if (!IsInBattle) return;
        SetLog("You fled from battle!");
        StartCoroutine(EndAfterDelay(1f));
    }

    private IEnumerator PlayerAttackRoutine(int damage)
    {
        _enemyCurrentHP = Mathf.Max(0, _enemyCurrentHP - damage);
        SetLog($"You dealt {damage} damage to {_currentEnemy.enemyName}!");
        RefreshEnemyUI();
        yield return new WaitForSeconds(1.2f);

        if (_enemyCurrentHP <= 0)
        {
            SetLog($"{_currentEnemy.enemyName} was defeated!");
            yield return new WaitForSeconds(1f);
            EndBattle();
            yield break;
        }

        // Enemy turn
        _turn = 1;
        yield return StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        int dmg = _currentEnemy != null ? _currentEnemy.attackDamage : 5;
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

        // Back to player turn
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

    private void RefreshEnemyUI()
    {
        if (_currentEnemy == null) return;

        if (enemyImage != null)
        {
            enemyImage.sprite = _currentEnemy.sprite;
            enemyImage.enabled = _currentEnemy.sprite != null;
        }
        if (enemyNameText != null)
            enemyNameText.text = _currentEnemy.enemyName;
        if (enemyHPText != null)
            enemyHPText.text = $"HP: {_enemyCurrentHP}/{_currentEnemy.maxHP}";
    }

    private void RefreshPlayerUI()
    {
        if (playerHPText == null || PlayerStats.Instance == null) return;
        playerHPText.text = $"Player HP: {PlayerStats.Instance.CurrentHP}/{PlayerStats.Instance.maxHP}";
    }

    private void SetLog(string message)
    {
        if (battleLogText != null)
            battleLogText.text = message;
        Debug.Log($"[Battle] {message}");
    }

    private void SetActionsInteractable(bool interactable)
    {
        if (itemButton != null) itemButton.interactable = interactable;
        if (fleeButton != null) fleeButton.interactable = interactable;
    }
}
