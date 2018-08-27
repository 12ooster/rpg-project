using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class EnemyStatsScript : CharStatsScript
{

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
	
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
}

