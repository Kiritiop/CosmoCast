using Unity.VisualScripting;
using UnityEngine;

public class Spell
{
    public string _element;

    public string name;

    private float _damage;

    private Effect _effect;

    public bool learned;
    public Spell(string element, string spellname, float? damage = null, Effect effect = null)
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
    private int _baseDamage;
    private int _count;
    private string _effectName;
    protected int _effectTier;

    public Effect(int damage, int count, string effectName, int effectTier)
    {
        this._baseDamage = damage;
        this._count = count;
        this._effectName = effectName;
        this._effectTier = effectTier;
    }
    
    public virtual double Activate()
    {
        this._count--;
        return this._baseDamage * (1 + (0.15 * this._effectTier)) ;
    }
}
public class Burn : Effect //deals set damage every round end
{
    public Burn(int count, int  effectTier) : base(5, count, "Immolate", effectTier) { }

    public override double Activate() // ac
    {

        return base.Activate();
    }
}

public class WaterPrism : Effect //stores damage over a set amount of turns, multiplies and applies them all at once when it expires
{
    private double _storedDamage;
    public WaterPrism(int count, int effectTier) : base(0, count, "WaterPrism", effectTier) { }

    public override double Activate() // activates once turn ends with one count
    {
        return _storedDamage * (1 + (0.2 * base._effectTier));
        
    }
    
}

public class Impaled : Effect //deals damage every time the enemy makes an action
{
    public int spikenumber;
    public Impaled(int count, int spikenumber) : base(5, count, "Impaled", 0)
    {
        this.spikenumber = spikenumber;
    }

    
}