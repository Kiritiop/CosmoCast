using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _discardButton;
    [SerializeField] private Image itemImage;

    private Item _item;

    public void Setup(Item item, InventoryUI inventoryUI)
    {
        _item = item;
        _discardButton.GetComponent<Image>().color = GetRarityColor(_item.Rarity);
        itemImage.sprite = item.Icon;
        _equipButton.GetComponent<Button>().onClick.AddListener(Equip);
        _discardButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            InventoryManager.Instance.RemoveItem(_item);
            inventoryUI.RefreshCurrentTab();
            Destroy(_item);
        });
    }

    private void Equip()
    {
        if (_item is Hat h)       h.Equip();
        else if (_item is Rune r) r.Equip();
        else if (_item is Potion p) p.Drink();
    }

    private void Discard()
    {
        InventoryManager.Instance.RemoveItem(_item);
        Destroy(_item);
    }

    private Color GetRarityColor(string rarity)
    {
        return rarity switch
        {
            "Legendary" => new Color(1f, 0.5f, 0f),   // orange
            "Epic"      => new Color(0.5f, 0f, 1f),    // purple
            "Rare"      => new Color(0f, 0.5f, 1f),    // blue
            "Common"    => Color.white,
            _           => Color.white
        };
    }
}