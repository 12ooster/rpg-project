using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

[System.Serializable]
public class CharStatsData
{
    /// <summary>
    /// The character's status effect.
    /// </summary>
    [SerializeField]
    private GameConstants.StatusType _status;

    public GameConstants.StatusType Status
    {
        get
        {
            return this._status;
        }
        set
        {
            this._status = value;
        }
    }

    /// <summary>
    /// The character's health. If reduced to 0, the character dies.
    /// </summary>
    [SerializeField]
    private int _hp;

    /// <summary>
    /// Gets or sets the character's health.
    /// </summary>
    /// <value>The H.</value>
    public int HP
    {
        get
        {
            return this._hp;
        }
        set
        {
            this._hp = value;
        }
    }

    /// <summary>
    /// The character's max possible health.
    /// </summary>
    [SerializeField]
    private int _maxHp;

    /// <summary>
    /// Gets or sets the character's current health.
    /// </summary>
    /// <value>The H.</value>
    public int MaxHP
    {
        get
        {
            return this._maxHp;
        }
        set
        {
            this._maxHp = value;
        }
    }

    /// <summary>
    /// Finite resource used by abilities.
    /// </summary>
    [SerializeField]
    private int _ap;

    /// <summary>
    /// Gets or sets the character's ability resource.
    /// </summary>
    /// <value>A.</value>
    public int AP
    {
        get
        {
            return this._ap;
        }
        set
        {
            this._ap = value;
        }
    }

    /// <summary>
    /// Determines damage dealt by physical abilities.
    /// </summary>
    [SerializeField]
    private int _strength;

    /// <summary>
    /// Gets or sets the character's strength
    /// </summary>
    /// <value>The strength.</value>
    public int Strength
    {
        get
        {
            return this._strength;
        }
        set
        {
            this._strength = value;
        }

    }

    /// <summary>
    /// Determines physical damage received and health gained per level
    /// </summary>
    [SerializeField]
    private int _stamina;

    /// <summary>
    /// Gets character's stamina
    /// </summary>
    /// <value>The stamina.</value>
    public int Stamina
    {
        get
        {
            return this._stamina;
        }
        set
        {
            this._stamina = value;
        }
    }

    /// <summary>
    /// Determines ability damage dealt : spell power
    /// </summary>
    [SerializeField]
    private int _intelligence;

    public int Intelligence
    {
        get
        {
            return this._intelligence;
        }
        set
        {
            this._intelligence = value;
        }
    }

    /// <summary>
    /// Determines ability damage received and ap gained : spell defense
    /// </summary>
    [SerializeField]
    private int _willpower;

    /// <summary>
    /// Gets or sets the character's willpower
    /// </summary>
    /// <value>The will power.</value>
    public int WillPower
    {
        get
        {
            return this._willpower;
        }
        set
        {
            this._willpower = value;
        }
    }

    /// <summary>
    /// The character's speed. Determines initiative in combat
    /// </summary>
    [SerializeField]
    private int _speed;

    /// <summary>
    /// Gets or sets the speed of the character.
    /// </summary>
    /// <value>The speed.</value>
    public int Speed
    {
        get
        {
            return this._speed;
        }
        set
        {
            this._speed = value;
        }
    }

    public CharAbility[] CombatAbilities;

    [SerializeField]
    private string _charName;

    public string CharName
    {
        get
        {
            return this._charName;
        }
        set
        {
            this._charName = value;
        }
    }

    [SerializeField]
    /// <summary>
    /// The xp this character gives when defeated in pve encounter.
    /// </summary>
    private int _xp;

    public int XP
    {
        get
        {
            return this._xp;
        }
        set
        {
            this._xp = value;
        }
    }

    /// <summary>
    /// If this character is an NPC, this leads to its evaluation of targets in combat
    /// </summary>
    [SerializeField]
    private GameConstants.EnemyFocusType _focusType;

    public GameConstants.EnemyFocusType FocusType
    {
        get
        {
            return this._focusType;
        }
        set
        {
            this._focusType = value;
        }
    }

    /// <summary>
    /// If this character is an NPC, this leads to its evaluation of action selection
    /// </summary>
    [SerializeField]
    private GameConstants.EnemyCombatBehaviorType _behaviorType;

    public GameConstants.EnemyCombatBehaviorType BehaviorType
    {
        get
        {
            return this._behaviorType;
        }
        set
        {
            this._behaviorType = value;
        }
    }

    [SerializeField]
    /// <summary>
    /// This character's id number. Used for checking cases against character's party, and for party management.
    /// </summary>
    private int _id = -1;

    /// <summary>
    /// The character's sprite.
    /// </summary>
    public Sprite charSprite;

    [SerializeField]
    private string _animatorType;

    public string AnimatorType
    {
        get
        {
            return this._animatorType;
        }
        set
        {
            this._animatorType = value;
        }
    }

    public int ID
    {
        get
        {
            return this._id;
        }
        set
        {
            this._id = value;
        }
    }

    /// <summary>
    /// Data structure holding the character's dialogue.
    /// </summary>
    public CharacterDialogMgr DialogMgr;

    public void Copy(CharStatsData StatsToCopy)
    {
        this.Status = StatsToCopy.Status;
        this.HP = StatsToCopy.HP;
        this.MaxHP = StatsToCopy.MaxHP;
        this.AP = StatsToCopy.AP;
        this.Strength = StatsToCopy.Strength;
        this.Stamina = StatsToCopy.Stamina;
        this.WillPower = StatsToCopy.WillPower;
        this.Intelligence = StatsToCopy.Intelligence;
        this.Speed = StatsToCopy.Speed;
        this.CombatAbilities = StatsToCopy.CombatAbilities;
        this.CharName = StatsToCopy.CharName;
        this.ID = StatsToCopy.ID;
        this.DialogMgr = StatsToCopy.DialogMgr;
        this.charSprite = StatsToCopy.charSprite;
        this.FocusType = StatsToCopy.FocusType;
        this.BehaviorType = StatsToCopy.BehaviorType;
        this.AnimatorType = StatsToCopy.AnimatorType;
    }

    public void StatsAsScript(ref CharStatsScript statsScript)
    {
        statsScript.Status = this.Status;
        statsScript.HP = this.HP; 
        statsScript.MaxHP = this.MaxHP;
        statsScript.AP = this.AP;  
        statsScript.Strength = this.Strength; 
        statsScript.Stamina = this.Stamina;
        statsScript.WillPower = this.WillPower; 
        statsScript.Intelligence = this.Intelligence;
        statsScript.Speed = this.Speed;
        statsScript.CombatAbilities = this.CombatAbilities;
        statsScript.CharName = this.CharName;
        statsScript.ID = this.ID;
        statsScript.DialogMgr = this.DialogMgr;
        statsScript.charSprite = this.charSprite;
        statsScript.FocusType = this.FocusType;
        statsScript.BehaviorType = this.BehaviorType;
        statsScript.AnimatorType = this.AnimatorType;
    }

    public CharStatsData()
    {
    }
}
