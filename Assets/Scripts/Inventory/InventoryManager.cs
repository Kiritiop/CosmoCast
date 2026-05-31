using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private ItemList<Potion> _potions = new ItemList<Potion>();
    private ItemList<Hat> _hats = new ItemList<Hat>();
    private ItemList<Rune> _runes = new ItemList<Rune>();

    public void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
    }

    public void AddItem(Item item)
    {
        if (item is Rune r) _runes.Add(r);
        else if (item is Hat h) _hats.Add(h);
        else if (item is Potion p) _potions.Add(p);
        else Debug.Log("Unknown item type");
    }

    public void RemoveItem(Item item)
    {
        if (item is Rune r) _runes.Remove(r);
        else if (item is Hat h) _hats.Remove(h);
        else if (item is Potion p) _potions.Remove(p);
        else Debug.Log("Unknown item type");
    }
    public ItemList<Potion> GetPotions() { return _potions; }
    public ItemList<Hat> GetHats() { return _hats; }
    public ItemList<Rune> GetRunes() { return _runes; }
}
