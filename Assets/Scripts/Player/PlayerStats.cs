using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] public int maxHP = 100;
    public int CurrentHP { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        CurrentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - amount);
        Debug.Log($"Player HP: {CurrentHP}/{maxHP}");
        if (CurrentHP <= 0)
            Debug.Log("Player fainted!");
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(maxHP, CurrentHP + amount);
    }
}
