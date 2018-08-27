using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCalloutScript : MonoBehaviour {

    [SerializeField]
    public Dictionary<int,UIItemMenuOptionScript> CalloutActions; //TODO:Fix bug with the inheritied classes not being added to this dictionary. May need to use gameobjects? 
    //List UIItem scripts
    public List<UIItemMenuOptionScript> CallOutActionsList;
    public int CurrIndex = -1;
    public InventoryItemScript SelectedItem;
    public static ItemCalloutScript Instance;

    void Awake()
    {
        if (ItemCalloutScript.Instance != null && ItemCalloutScript.Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ItemCalloutScript.Instance = this;
        }

    }

    // Use this for initialization
	void Start () 
    {
        //Initialize dictionary holding the actions of callout
        CalloutActions = new Dictionary<int, UIItemMenuOptionScript>(); 
        CallOutActionsList = new List<UIItemMenuOptionScript>();

        //Get actions from children
        foreach (UIItemMenuOptionScript s in this.transform.GetComponentsInChildren<UIItemMenuOptionScript>())
        {
            if(CalloutActions.ContainsKey(s.OptionIndex))
            {
                    Debug.LogError("Error! Tried to add duplicate entry in Callout Actions menu!");
            }
            else
            {
                CalloutActions.Add(s.OptionIndex, s);
                CallOutActionsList.Add(s);
            }

        }
        //Set initial action to item list 1
        SetSelectedAction(1);
        //Set this inactive so it doesn't appear
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void Init()
    {
        //Initialize dictionary holding the actions of callout
        CalloutActions = new Dictionary<int, UIItemMenuOptionScript>(); 
        //Get actions from children
        foreach (UIItemMenuOptionScript s in this.transform.GetComponentsInChildren<UIItemMenuOptionScript>())
        {
            CalloutActions.Add(s.OptionIndex, s);
        }
        //Set initial action
        SetSelectedAction(0);
    }

    public void SetSelectedAction(int indexAdjustment)
    {
        //Check to make sure index is within bounds
        if (indexAdjustment + CurrIndex < 0 || indexAdjustment + CurrIndex > CalloutActions.Count - 1)
        {
            return;
        }
        else
        {
            //if CurrIndex isn't the default value then deselect the item with that index
            if (CurrIndex != -1)
            {
                this.CalloutActions[CurrIndex].SetSelected(false);
            }
            CurrIndex = (CurrIndex + indexAdjustment);
            this.CalloutActions[CurrIndex].SetSelected(true);
       }
    }

    public void SelectedAction()
    {
        this.CalloutActions[CurrIndex].Action(SelectedItem);
    }

    public void CloseCallout()
    {
        //Set initial action back to default
        SetSelectedAction(0);
        //
        this.gameObject.SetActive(false);
    }
}
