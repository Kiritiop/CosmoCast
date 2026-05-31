using UnityEngine;

public enum PotionType { HealthPotion, StrengthPotion }

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
public class Potion : Item
{
    public int amount;
    public PotionType potionType;

    public void Drink()
    {
        if (potionType == PotionType.HealthPotion)
        {
            PlayerStats.Instance.Heal(amount);
        }
        else if (potionType == PotionType.StrengthPotion)
        {
            PlayerStats.Instance.strength *= amount;
        }
    }

    public void Wearoff()
    {
        if (potionType == PotionType.StrengthPotion)
        {
            PlayerStats.Instance.strength = 1;
        }
    }
}