using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Person : MonoBehaviour
{
    public static Person Instance { get; private set; }
    [SerializeField] private float maxHealth = 100;
    public float earthDMG = 1;
    public float fireDMG = 1;
    public float waterDMG = 1;
    public float airDMG = 1;
    protected float defense = 1;
    protected float strength = 1;
    public Hat hatWorn;
    public Rune equippedRune;
    protected List<Effect> activeEffects = new List<Effect>();
    protected List<Spell> spellbook = new List<Spell>();
    public float CurrentHealth { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        CurrentHealth = maxHealth;
        LearnSpell(new Spell("Fireball", new Damage(20, "fire")));
        LearnSpell(new Spell("Boulder", new Damage(20, "earth")));
        LearnSpell(new Spell("IceShards", new Damage(20, "water")));
        LearnSpell(new Spell("LightBreeze", new Damage(20, "air")));
    }

    
    public void TakeDamage(Damage damage)
    {
        if (damage != null)
        {
            this.CurrentHealth -= damage.GetDamage() * defense;
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

    public void LearnSpell(Spell spell)
    {
        if (!spellbook.Contains(spell))
        {
            spellbook.Add(spell);
            spell.Learn();
        }
    }


}
public class Player : Person
{
    [SerializeField] private string playerName = "Player";

    private void Start()
    {
        InitializeStarterSpells();
    }

    private void InitializeStarterSpells()
    {
        // Create starter spells
        Spell fireBall = ScriptableObject.CreateInstance<Spell>();
        fireBall.spellName = "Fireball";
        fireBall.damage = new Damage(20, "fire");
        LearnSpell(fireBall);

        Spell iceShards = ScriptableObject.CreateInstance<Spell>();
        iceShards.spellName = "Ice Shards";
        iceShards.damage = new Damage(20, "water");
        LearnSpell(iceShards);

        Spell earthSpike = ScriptableObject.CreateInstance<Spell>();
        earthSpike.spellName = "Earth Spike";
        earthSpike.damage = new Damage(20, "earth");
        LearnSpell(earthSpike);

        Spell lightBreeze = ScriptableObject.CreateInstance<Spell>();
        lightBreeze.spellName = "Light Breeze";
        lightBreeze.damage = new Damage(20, "air");
        LearnSpell(lightBreeze);
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame && BattleManager.Instance.IsInBattle)
        {
            spellbook[0].Cast(this, BattleManager.Instance.Enemy);
        }
        else if (Keyboard.current.xKey.wasPressedThisFrame && BattleManager.Instance.IsInBattle)
        {
            spellbook[1].Cast(this, BattleManager.Instance.Enemy);
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame && BattleManager.Instance.IsInBattle)
        {
            spellbook[2].Cast(this, BattleManager.Instance.Enemy);
        }
        else if (Keyboard.current.vKey.wasPressedThisFrame && BattleManager.Instance.IsInBattle)
        {
            spellbook[3].Cast(this, BattleManager.Instance.Enemy);
        }
    }
}
public class Spell : ScriptableObject
{

    public string spellName;

    public Damage damage;

    private Effect _effect;

    private bool learned;
    public Spell(string spellname, Damage damage = null, Effect effect = null)
    {
        this.spellName = spellname;
        this.damage = damage;
        this._effect = effect;
        this.learned = false;
    }

    public void Learn()
    {
        this.learned = true;
    }
    public bool IsLearned()
    {
        return learned;
    }
    public void Cast(Person target, Person caster)
    {
        if (this.damage == null)
        {
            target.Inflict(this._effect);
            return;
        }
        if (this.damage.GetElement() == "water")
        {
            target.TakeDamage( new Damage(this.damage.GetDamage() * caster.waterDMG, this.damage.GetElement()));
        }
        else if (this.damage.GetElement() == "fire")
        {
            target.TakeDamage(new Damage(this.damage.GetDamage() * caster.fireDMG, this.damage.GetElement()));
        }
        else if (this.damage.GetElement() == "earth")
        {
            target.TakeDamage(new Damage(this.damage.GetDamage() * caster.earthDMG, this.damage.GetElement()));
        }
        else if(this.damage.GetElement() == "air")
        {
            target.TakeDamage(new Damage(this.damage.GetDamage() * caster.airDMG, this.damage.GetElement()));
        }
        if (this._effect != null)
        { 
            target.Inflict(this._effect);
        }
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
        return this._baseDamage.GetDamage() * (1 + (0.15 * this._effectTier));
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
        return _storedDamage.GetDamage() * (1 + (0.2 * base._effectTier));

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
        return spikenumber * this._baseDamage.GetDamage();
    }

}

public class Damage
{
    private float _value;
    private string element;

    public Damage(float value, string element)
    {
        this._value = value;
        this.element = element;
    }

    public float GetDamage()
    {
        return this._value;
    }
    public string GetElement()
    {
        return this.element;
    }
}