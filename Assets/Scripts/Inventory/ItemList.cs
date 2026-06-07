using UnityEngine;

public class ItemList<T> where T : Item
{
    private T[] _items;
    private int _cursor;

    public ItemList()
    {
        this._items = new T[0];
        this._cursor = 0;
    }

    public void Add(T item)
    {
        if (_items.Length == _cursor)
        {
            T[] result = new T[_items.Length + 1];

            for (int i = 0; i < _items.Length; i++)
            {
                result[i] = _items[i];
            }
            result[_cursor++] = item;
            this._items = result;
        }
        else
        {
            _items[_cursor++] = item;
        }
    }

    public void Remove(T item)
    {
        int index = -1;
        for (int i = 0; i < _cursor; i++)
        {
            if (_items[i] == item)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Debug.Log("Item not found");
            return;
        }

        for (int i = index; i < _cursor - 1; i++)
            _items[i] = _items[i + 1];
        _items[_cursor - 1] = null;
        _cursor--;
    }

    public int GetIndex(T item)
    {
        for (int i = 0; i < _cursor; i++)
        {
            if (_items[i] == item)
                return i;
        }
        Debug.Log("Item not found");
        return -1;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= _cursor)
        {
            Debug.Log("Invalid index");
            return null;
        }
        return _items[index];
    }

    public int Count()
    {
        return _cursor;
    }

}