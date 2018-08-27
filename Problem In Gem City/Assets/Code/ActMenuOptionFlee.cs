using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ActMenuOptionFlee : UIActionMenuOptionScript  {


	// Use this for initialization
	void Start () 
    {
        //ensure index matches expected option
        if(this.OptionIndex != (int)GameConstants.ActionOptionIndices.Flee)
        {
            this.OptionIndex = (int)GameConstants.ActionOptionIndices.Flee;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
