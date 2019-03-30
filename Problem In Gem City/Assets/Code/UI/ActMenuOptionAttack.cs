using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
public class ActMenuOptionAttack : UIActionMenuOptionScript {

	
    void Awake()
    {
        //ensure index matches expected option
        if(this.OptionIndex != (int)GameConstants.ActionOptionIndices.Attack)
        {
            this.OptionIndex = (int)GameConstants.ActionOptionIndices.Attack;
        }
    }
    // Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    /// <summary>
    /// Action for this option in the menu.
    /// </summary>
    /// <param name="item">Item.</param>
    public override void Action(CharMgrScript attackingChar)
    {
       //Set this UI button element active

        Debug.LogWarning("Act Menu Option called from Attack!");
        //Set attack menu item to selected state
        CombatUIManager._instance.UpdateMenuFromSelection((GameConstants.ActionOptionIndices)this.OptionIndex);
    }
}
