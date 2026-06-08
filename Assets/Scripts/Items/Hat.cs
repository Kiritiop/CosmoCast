using UnityEngine;

[CreateAssetMenu(fileName = "Hat", menuName = "Scriptable Objects/Hat")]
public class Hat : Item
{
    public float earthMulti;
    public float waterMulti;
    public float airMulti;
    public float fireMulti;
    public float defense;

    public void Equip()
    {
        if (PlayerStats.Instance.equippedHat != null)
            PlayerStats.Instance.equippedHat.Unequip();
        else
            PlayerStats.Instance.equippedHat = this;
            Use();
    }

    private void Use()
    {
        PlayerStats.Instance.earthDMG *= earthMulti;
        PlayerStats.Instance.waterDMG *= waterMulti;
        PlayerStats.Instance.airDMG *= airMulti;
        PlayerStats.Instance.fireDMG *= fireMulti;
        PlayerStats.Instance.defense += defense;
        Debug.Log($"Equipped {Name} | Earth: {PlayerStats.Instance.earthDMG} Fire: {PlayerStats.Instance.fireDMG} Defense: {PlayerStats.Instance.defense}");
    }

    public void Unequip()
    {
        PlayerStats.Instance.equippedHat = null;
        Unuse();
    }

    private void Unuse()
    {
        PlayerStats.Instance.earthDMG /= earthMulti;
        PlayerStats.Instance.waterDMG /= waterMulti;
        PlayerStats.Instance.airDMG /= airMulti;
        PlayerStats.Instance.fireDMG /= fireMulti;
        PlayerStats.Instance.defense -= defense;
    }
}