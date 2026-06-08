using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Text priceText;
    [SerializeField] private Button buyButton;

    private ShopItem _shopItem;
    private ShopUI _shopUI;

    public void Setup(ShopItem shopItem, ShopUI shopUI)
    {
        _shopItem = shopItem;
        _shopUI = shopUI;
        itemImage.sprite = shopItem.item.Icon;
        priceText.text = shopItem.price + "g";
        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (PlayerWallet.Instance.SpendCoins(_shopItem.price))
        {
            Item newItem = Instantiate(_shopItem.item);
            newItem.Rarity = "Common";
            InventoryManager.Instance.AddItem(newItem);
            _shopUI.UpdateCoinsDisplay();
            Debug.Log($"Bought {_shopItem.item.Name} for {_shopItem.price} coins");
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}