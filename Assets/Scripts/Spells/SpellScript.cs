using UnityEngine;

public class Spell
{
    public string _element;

    public string name;

    private float _damage;

    private string _effect;

    public bool learned;
    public Spell(string element, string spellname, float? damage = null, string effect = null)
    {
        this.name = spellname;
        this._element = element;
        this._damage = (float)damage;
        this._effect = effect;
    }

    public void learn()
    {
        this.learned = true;
    }
    public void activateSpell(Entity target, Entity caster)
    {
        target.Damage(this._damage);
        target.inflict(this._effect);
    }
    
}
public class Effect
{
    private float _damage;
    private int _turntime;
    private string _effectName;
    private int _effectTier;

    public Effect(float damage, int turntime, string effectName, int effectTier)
    {
        this._damage = damage;
        this._turntime = turntime;
        this._effectName = effectName;
        this._effectTier = effectTier;
    }
}
