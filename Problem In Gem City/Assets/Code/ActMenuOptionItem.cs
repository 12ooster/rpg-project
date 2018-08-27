using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ActMenuOptionItem : UIActionMenuOptionScript {

	// Use this for initialization
	void Start () 
    {
        //ensure index matches expected option
        if(this.OptionIndex != (int)GameConstants.ActionOptionIndices.Item)
        {
            this.OptionIndex = (int)GameConstants.ActionOptionIndices.Item;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
