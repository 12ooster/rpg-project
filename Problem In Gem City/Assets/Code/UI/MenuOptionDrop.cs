using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionDrop : UIItemMenuOptionScript {

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Action for this option in the menu.
    /// </summary>
    /// <param name="item">Item.</param>
    public override void Action(InventoryItemScript item)
    {
        //ObjectManagerScript.Instance.CreateItemGameWorld(item);
        item.Drop();
        //Selected item removed from inventory so close callout
        MenuUIManager.MenuUIInstance.CloseCallout();
    }
}
