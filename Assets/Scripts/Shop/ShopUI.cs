using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject shopSlotPrefab;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private Text coinsText;

    public void OpenShop(ShopItem[] stock)
    {
        shopPanel.SetActive(true);
        PlayerMovement.IsInventoryOpen = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PopulateShop(stock);
        UpdateCoinsDisplay();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        PlayerMovement.IsInventoryOpen = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ClearSlots();
    }

    private void PopulateShop(ShopItem[] stock)
    {
        ClearSlots();
        foreach (ShopItem shopItem in stock)
        {
            GameObject slot = Instantiate(shopSlotPrefab, slotContainer);
            slot.GetComponent<ShopSlot>().Setup(shopItem, this);
        }
    }

    private void ClearSlots()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);
    }

    public void UpdateCoinsDisplay()
    {
        coinsText.text = "Coins: " + PlayerWallet.Instance.Coins;
    }
}