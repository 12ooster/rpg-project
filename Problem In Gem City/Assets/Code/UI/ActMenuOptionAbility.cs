using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ActMenuOptionAbility : UIActionMenuOptionScript {

	// Use this for initialization
	void Start () 
    {
        //ensure index matches expected option
        if(this.OptionIndex != (int)GameConstants.ActionOptionIndices.Ability)
        {
            this.OptionIndex = (int)GameConstants.ActionOptionIndices.Ability;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
