using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Item[] _objects;
    private int _cursor;
    
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

    public void DisplayInventory(Vector3 startPosition, Vector3 spacingOffset)
    {
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
