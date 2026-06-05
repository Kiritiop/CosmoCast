using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] public GameObject Inventory_Bg;
    [SerializeField] public Sprite HealthPotionSprite;
    [SerializeField] public Sprite StrengthPotionSprite;
    [SerializeField] public Sprite FireHatSprite;
    [SerializeField] public Sprite EarthHatSprite;
    [SerializeField] public Sprite WaterHatSprite;
    [SerializeField] public Sprite AirHatSprite;
    [SerializeField] public Sprite StrengthRuneSprite;
    [SerializeField] public Sprite DefenseRuneSprite;
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
        if (Keyboard.current.eKey.wasPressedThisFrame && _isInvOpen == false) 
        {
            DisplayInventory(new Vector3(0,0,0), new Vector3(0,0,0));
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && _isInvOpen == true) 
        {
            CloseInventory();
        }
    }

    public void DisplayInventory(Vector3 startPosition, Vector3 spacingOffset)
    {
        this._isInvOpen = true;
        PlayerMovement.IsInventoryOpen = true;
        Time.timeScale = 0f;
        Inventory_Bg.SetActive (true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseInventory()
    {
        this._isInvOpen = false;
        PlayerMovement.IsInventoryOpen = false;
        Time.timeScale = 1f;
        Inventory_Bg.SetActive (false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisplayPotions()
    {
        
    }
    public void DisplayHats()
    {
        // Display the inventory icons as per the amount of hats
    }
    public void DisplayRunes()
    {
        // Display the inventory icons as per the amount of runes
    }
}
