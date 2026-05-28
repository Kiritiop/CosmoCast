using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryManager : MonoBehaviour
{
    public Item[] _objects;
    private int _cursor;
    [SerializeField] public GameObject Inventory_Bg;
    private bool _isOpen = false;

    
    public InventoryManager()
    {
        this._objects = new Item[0];
    }

    public void Add(Item item)
    {
        if (_objects.Length == _cursor)
        {
            Item[] result = new Item[_objects.Length + 1];

            for (int i = 0; i < _objects.Length; i++)
            {
                result[i] = _objects[i];
            }
            result[_cursor++] = item;
            this._objects = result;
        }
        else
        {
            _objects[_cursor++] = item;
        }
    }

    public void Update()
    {
        ReadInput();
    }

    public void ReadInput()
    {
        var keyboard = Keyboard.current;
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Inventory_Bg.SetActive (true);

        for (int i = 0; i < _cursor; i++)
        {
            Item currentItem = _objects[i];
            if (currentItem != null && currentItem.itemPrefab != null)
            {
                Vector3 spawnPosition = startPosition + (spacingOffset * i);

                GameObject spawnedItem = Instantiate(currentItem.itemPrefab, spawnPosition, Quaternion.identity);
                
                spawnedItem.name = currentItem.itemName + "_" + i;
            }
            else
            {
                Debug.LogWarning($"Item at index {i} is missing a assigned prefab!");
            }
        }
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

    public Item Getint(int index)
    {
        if (index >= 0 && index < _cursor)
        {
            return _objects[index];
        }
        else
        {
            throw new Exception("Index not in range");
        }
    }

    public override string ToString()
    {
        string resultstring = "";
        for (int i = 0; i < _cursor; i++)
        {
            if (i == _cursor - 1)
            {
                resultstring = resultstring + _objects[i];
            }
            else
            {     
                resultstring = resultstring + _objects[i] + ",";
            }
        }
        return resultstring;
    }
}
