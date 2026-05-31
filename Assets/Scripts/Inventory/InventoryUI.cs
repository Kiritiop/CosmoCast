using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject Inventory_Bg;
    private bool _isOpen;
    
    public void Start()
    {
        _isOpen = false;
    }
    
    public void Update()
    {
        ReadInput();
    }

    public void ReadInput()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && _isOpen == false) 
        {
            DisplayInventory(new Vector3(-950,-301,0), new Vector3(-10,0,0));
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && _isOpen == true) 
        {
            CloseInventory();
        }
    }

    private void DisplayInventory(Vector3 startPosition, Vector3 spacingOffset)
    {
        this._isOpen = true;
        PlayerMovement.IsInventoryOpen = true;
        Time.timeScale = 0f;
        Inventory_Bg.SetActive (true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseInventory()
    {
        this._isOpen = false;
        PlayerMovement.IsInventoryOpen = false;
        Time.timeScale = 1f;
        Inventory_Bg.SetActive (false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
