using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject Inventory_Bg;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
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

    public void DisplayPotions()
    {
        PopulateSlots(InventoryManager.Instance.GetPotions());
    }

    public void DisplayHats()
    {
        PopulateSlots(InventoryManager.Instance.GetHats());
    }

    public void DisplayRunes()
    {
        PopulateSlots(InventoryManager.Instance.GetRunes());
    }
}