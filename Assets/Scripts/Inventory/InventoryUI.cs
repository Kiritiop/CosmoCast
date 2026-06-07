using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject Inventory_Bg;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    private enum Tab { Potions, Hats, Runes }
    private Tab _currentTab;
    private bool _isInvOpen;

    public void Start()
    {
        _isInvOpen = false;
    }

    public void Update()
    {
        ReadInput();
    }

    public void ReadInput()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && !_isInvOpen)
            DisplayInventory();
        else if (Keyboard.current.eKey.wasPressedThisFrame && _isInvOpen)
            CloseInventory();
    }

    public void OnSortChanged(int index)
    {
        if (index == 0) InventoryManager.Instance.SortByRarity();
        else if (index == 1) InventoryManager.Instance.SortByName();
        RefreshCurrentTab();
    }

    private void DisplayInventory()
    {
        _isInvOpen = true;
        PlayerMovement.IsInventoryOpen = true;
        Time.timeScale = 0f;
        Inventory_Bg.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisplayPotions();
    }

    private void CloseInventory()
    {
        _isInvOpen = false;
        PlayerMovement.IsInventoryOpen = false;
        Time.timeScale = 1f;
        Inventory_Bg.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ClearSlots();
    }

    private void ClearSlots()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);
    }

    private void PopulateSlots<T>(ItemList<T> list) where T : Item
    {
        ClearSlots();
        for (int i = 0; i < list.Count(); i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            slot.GetComponent<Image>().sprite = list.Get(i).Icon;
        }
    }

    public void OnSortByRarity()
    {
        InventoryManager.Instance.SortByRarity();
        RefreshCurrentTab();
    }

    public void OnSortByName()
    {
        InventoryManager.Instance.SortByName();
        RefreshCurrentTab();
    }

    private void RefreshCurrentTab()
    {
        if (_currentTab == Tab.Potions)      DisplayPotions();
        else if (_currentTab == Tab.Hats)    DisplayHats();
        else if (_currentTab == Tab.Runes)   DisplayRunes();
    }

    public void DisplayPotions()
    {
        _currentTab = Tab.Potions;
        PopulateSlots(InventoryManager.Instance.GetPotions());
    }

    public void DisplayHats()
    {
        _currentTab = Tab.Hats;
        PopulateSlots(InventoryManager.Instance.GetHats());
    }

    public void DisplayRunes()
    {
        _currentTab = Tab.Runes;
        PopulateSlots(InventoryManager.Instance.GetRunes());
    }
}