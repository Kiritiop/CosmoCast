using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]

public class Item : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public string Rarity; 

}
