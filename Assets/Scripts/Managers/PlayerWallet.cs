using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance { get; private set; }

    public int Coins { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        if (SaveManager.Instance != null && SaveManager.Instance.HasSave())
            Coins = SaveManager.Instance.Load().currency;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log($"[Wallet] +{amount} coins. Total: {Coins}");
        Persist();
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        Persist();
        return true;
    }

    private void Persist()
    {
        if (SaveManager.Instance == null) return;
        SaveData data = SaveManager.Instance.HasSave()
            ? SaveManager.Instance.Load()
            : new SaveData();
        data.currency = Coins;
        SaveManager.Instance.Save(data);
    }
}
