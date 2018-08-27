using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ActionMenuScript : MonoBehaviour {

    [SerializeField]
    public Dictionary<int,UIActionMenuOptionScript> ActionMenuItems;
    [SerializeField]
    public List<UIActionMenuOptionScript> ActionMenuItemsList;


    public int CurrIndex = 0;
	// Use this for initialization
	void Start () {
        //Initialize dictionary holding the combat UI menu options
        ActionMenuItems = new Dictionary<int, UIActionMenuOptionScript>();
        //Initialize List for testing and seeing objects
        ActionMenuItemsList = new List<UIActionMenuOptionScript>();

        //Add each combat UI menu item script to the dictionary - NOTE: need to verify if these objects must be children in the hierarchy
        foreach (UIActionMenuOptionScript a in this.transform.GetComponentsInChildren<UIActionMenuOptionScript>())
        {
            //Check to make sure we don't accidentally add duplicates
            if(ActionMenuItems.ContainsKey(a.OptionIndex))
            {
                Debug.LogError("Error! Tried to add duplicate entry in Callout Actions menu****GameObj:"+a.gameObject.name+"****Index:"+a.OptionIndex+"****Action:"+a.OptionName);

            }
            else
            {
                //If not a duplicate we add it to the dictionary
                ActionMenuItems.Add(a.OptionIndex, a);
                //Add to list for test and checking
                ActionMenuItemsList.Add(a);
            }

        }

        //Set initially selected index ot 0
        SetSelectedAction(true,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSelectedAction(bool wasMouse, int indexAdjustment)
    {
        if (TestScript._instance.TestMode)
        {
            Debug.LogWarning("~~~~3. Action Menu Script: Set Selected Action: Key pressed ="+indexAdjustment+"~~~~");
        }

        if (wasMouse)
        {
            /*If player input was via mouse then set the index and selection directly from the passed in indexAdjustement*/
            //Set flag from previously selected item to make sure it isn't left in a selected state
            if (CurrIndex != -1)
            {
                this.ActionMenuItems[CurrIndex].SetSelected(false);
            }
            //Update current index and set selected for the necessary item in the dictionary
            CurrIndex = 0;
            this.ActionMenuItems[0].SetSelected(true);
        }
        else
        {
            //Check to make sure index is within bounds
            if (indexAdjustment + CurrIndex < 0 || indexAdjustment + CurrIndex > ActionMenuItems.Count - 1)
            {
                return;
            }
            else
            {
                //if CurrIndex isn't the default value then deselect the item with that index
                if (CurrIndex != -1)
                {
                    this.ActionMenuItems[CurrIndex].SetSelected(false);
                }
                CurrIndex = (CurrIndex + indexAdjustment);
                this.ActionMenuItems[CurrIndex].SetSelected(true);
            }
        }
    }

    /// <summary>
    /// Sets specified action item to be selected UI state
    /// </summary>
    /// <param name="menuItem">Menu item.</param>
    public void SetSelectedState(GameConstants.ActionOptionIndices menuItem,bool selectionState)
    {
        switch (menuItem)
        {
            case(GameConstants.ActionOptionIndices.Ability):
                
                break;
            case(GameConstants.ActionOptionIndices.Attack):
                //enable ui image to indicate item is selected
                this.ActionMenuItems[(int)GameConstants.ActionOptionIndices.Attack].SetActive(selectionState);
                //Enable UI indicator for first applicable target
                //CombatUIManager._instance.c
                break;
            case(GameConstants.ActionOptionIndices.Flee):
                break;
            case(GameConstants.ActionOptionIndices.Item):
                break;
            default:
                break;
                
        }
    }
}
