using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Battle UI Root")]
    [SerializeField] private GameObject battleUI;

    public bool IsInBattle { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        battleUI.SetActive(false);
    }

    public void StartBattle()
    {
        if (IsInBattle) return;

        IsInBattle = true;
        battleUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = false;

        Debug.Log("Battle started!");
    }

    public void EndBattle()
    {
        if (!IsInBattle) return;

        IsInBattle = false;
        battleUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) player.enabled = true;

        Debug.Log("Battle ended!");
    }

    // Called by a Flee / Exit button in the battle UI
    public void FleeBattle() => EndBattle();
}
