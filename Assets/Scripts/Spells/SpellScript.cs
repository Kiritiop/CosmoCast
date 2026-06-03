using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Person : MonoBehaviour
{
    private float maxHealth;
    private float earthDMG = 1;
    private float fireDMG = 1;
    private float waterDMG = 1;
    private float airDMG = 1;
    private float defense = 1;
    private float strength = 1;
    public Hat hatWorn;
    public Rune equippedRune;
    private List<Effect> activeEffects = new List<Effect>();
    private List<Spell> spellbook = new List<Spell>();
    
    public void takeDamage(Damage damage)
    {
        if (damage != null)
        {
            if (damage.getElement() == "earth")
            {
                this.maxHealth -= damage.getDamage() * defense;
            }
            else if (damage.getElement() == "fire")
            {
                this.maxHealth -= damage.getDamage() * defense;
            }
            else if (damage.getElement() == "water")
            {
                this.maxHealth -= damage.getDamage() * defense;
            }
            else if (damage.getElement() == "air")
            {
                this.maxHealth -= damage.getDamage() * defense;
            }
        }
    }

    public void Inflict(Effect effect)
    {
        if (effect != null)
        {
            activeEffects.Add(effect);
        }
    }

    public void Cast(Spell spell, Person target)
    {
        if (spellbook.Contains(spell))
        {
            spell.Cast(this, target);
        }
    }


}
public class Spell : ScriptableObject
{

    public string spellName;

    public Damage damage;

    private Effect _effect;

    public bool learned;
    public Spell(string spellname, Damage damage = null, Effect effect = null)
    {
        this.spellName = spellname;
        this.damage = damage;
        this._effect = effect;
    }

    public void learn()
    {
        this.learned = true;
    }
    public void Cast(Person target, Person caster)
    {
        target.takeDamage(this.damage);
        target.Inflict(this._effect);
    }

}
public class Effect
{
    protected Damage _baseDamage;
    protected int _count;
    private string _effectName;
    protected int _effectTier;

    public Effect(Damage damage, int count, string effectName, int effectTier)
    {
        this._baseDamage = damage;
        this._count = count;
        this._effectName = effectName;
        this._effectTier = effectTier;
    }

    public virtual double Activate()
    {
        this._count--;
        return this._baseDamage.getDamage() * (1 + (0.15 * this._effectTier));
    }
}
public class Burn : Effect //deals set damage every round end
{
    public Burn(int count, int effectTier) : base(new Damage(5, "fire"), count, "Immolate", effectTier) { }

    public override double Activate() // ac
    {

        return base.Activate();
    }
}

public class WaterPrism : Effect //stores damage over a set amount of turns, multiplies and applies them all at once when it expires
{
    private Damage _storedDamage;
    public WaterPrism(int count, int effectTier) : base(new Damage(0, "water"), count, "WaterPrism", effectTier) { }

    public override double Activate() // activates once turn ends with one count
    {
        return _storedDamage.getDamage() * (1 + (0.2 * base._effectTier));

    }

}

public class Impaled : Effect //deals damage every time the enemy makes an action
{
    public int spikenumber;
    public Impaled(int count, int spikenumber) : base(new Damage(5, "normal"), count, "Impaled", 0)
    {
        this.spikenumber = spikenumber;
    }

    public override double Activate()
    {
        return spikenumber * this._baseDamage.getDamage();
    }

}

public class Damage
{
    private int _value;
    private string element;

    public Damage(int value, string element)
    {
        this._value = value;
        this.element = element;
    }

    public int getDamage()
    {
        return this._value;
    }
    public string getElement()
    {
        return this.element;
    }
}