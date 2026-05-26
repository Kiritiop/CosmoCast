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
            Item[] result = new Item[_objects.Length + 3];

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
