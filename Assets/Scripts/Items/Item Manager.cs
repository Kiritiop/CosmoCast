using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string rarity; 

    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");
    }
}
