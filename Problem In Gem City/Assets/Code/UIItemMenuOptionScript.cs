using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIItemMenuOptionScript : MonoBehaviour {

    public Color StartingColor;
    public float SelectedAlpha =0.15f;
    public string OptionName = "";
    public int OptionIndex = -1;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    /// <summary>
    /// Sets the item as selected in player's inventory and updates UI to show this.
    /// </summary>
    public void SetSelected(bool selected)
    {
        if (selected)
        {
            this.GetComponent<Image>().color = new Color(StartingColor.r, StartingColor.g, StartingColor.b, SelectedAlpha);
        }
        else
        {
            this.GetComponent<Image>().color = StartingColor;
        }
    }

    /// <summary>
    /// Sets the display elements.
    /// </summary>
    /// <param name="item">The item to which this object corresponds.</param>
    public void SetDisplayElements()
    {
        //Set option name
        if (OptionName == null || OptionName == "")
        {
            OptionName = this.gameObject.name;
        }
        //Get color to be used for selection state
        StartingColor = this.GetComponent<Image>().color;
    }

    public virtual void Action(InventoryItemScript item)
    {
        //Action method overridden by menu options that inherit from this class
    }
}
