using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Battle UI Root")]
    [SerializeField] private GameObject battleUI; // parent that holds ALL battle panels

    public bool IsInBattle { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // Always hidden when the scene loads
        battleUI.SetActive(false);
    }

    public void StartBattle()
    {
        if (IsInBattle) return;

        IsInBattle = true;
        battleUI.SetActive(true);

        // Freeze player movement during battle
        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = false;

        Debug.Log("Battle started!");
    }

    public void EndBattle()
    {
        if (!IsInBattle) return;

        IsInBattle = false;
        battleUI.SetActive(false);

        // Restore player movement
        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = true;

        Debug.Log("Battle ended!");
    }
}
