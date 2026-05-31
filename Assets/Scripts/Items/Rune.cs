using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Scriptable Objects/Rune")]
public class Rune : Item
{
    public float earthMulti;
    public float waterMulti;
    public float airMulti;
    public float fireMulti;
    public float defense;

    public void Equip()
    {
        if (PlayerStats.Instance.equippedRune != null)
            PlayerStats.Instance.equippedRune.Unequip();

        PlayerStats.Instance.equippedRune = this;
        Use();
    }

    private void Use()
    {
        PlayerStats.Instance.earthDMG *= earthMulti;
        PlayerStats.Instance.waterDMG *= waterMulti;
        PlayerStats.Instance.airDMG *= airMulti;
        PlayerStats.Instance.fireDMG *= fireMulti;
        PlayerStats.Instance.defense += defense;
    }

    public void Unequip()
    {
        PlayerStats.Instance.equippedRune = null;
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