using UnityEngine;

public class Spell
{
    public string _element;

    public string name;

    private float _damage;

    private string _effect;

    public bool learned;
    public Spell(string element, string spellname, float? damage = null, string? effect = null)
    {
        this.name = spellname;
        this._element = element;
        this._damage = damage;
        this._effect = effect;
    }

    public void learn()
    {
        this.learned = true;
    }
    public void activateSpell(Entity target, Entity caster)
    {
        if (this._damage != null)
        {
            target.Damage(caster.damageOut(this._damage));
        }
        target.inflict(this._effect);
    }
    
}

//public class DOTSpell : Spell
//{
//    public int turntime;
//    public float tickdamage;

//    public DOTSpell(string element, int turntime, float damage, string name) : base(element,name)
//    {
//        this.turntime = turntime;
//        this.tickdamage = damage;
//    }
//    public float? activateDamage()
//    {
//        if (turntime > 0)
//        {
//            return tickdamage;
//        }
//        return null;
//    }
//}

public class DMGSpell : Spell
{
    private float _damage;
    private string _effect;

    public DMGSpell(string name, string element, string? effect, float damage) : base(element, name)
    {
        this._damage = damage;
        this._effect = effect;
    }

    public float? activateSpell(Entity entity)
    {
        entity.inflict(this._effect);
        return _damage;
    }

}


