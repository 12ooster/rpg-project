using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class AbilityBoxScript : MonoBehaviour {

    [SerializeField]
    public Dictionary<int,MenuOptionAbility> CalloutActions; //TODO:Fix bug with the inheritied classes not being added to this dictionary. May need to use gameobjects? 
    //List UIItem scripts
    public List<MenuOptionAbility> CallOutActionsList;
    public int CurrIndex = -1;
    public MenuOptionAbility SelectedResponse;
    public GameObject aMenuItemObj;
    //   public static ResponseBoxScript Instance;

    void Awake()
    {
        //        if (ResponseBoxScript.Instance != null && ResponseBoxScript.Instance != this)
        //        {
        //            Destroy(this.gameObject);
        //        }
        //        else
        //        {
        //            ResponseBoxScript.Instance = this;
        //        }

    }

    // Use this for initialization
    void Start () 
    {
        //Initialize dictionary holding the actions of callout
        CalloutActions = new Dictionary<int, MenuOptionAbility>(); 
        CallOutActionsList = new List<MenuOptionAbility>();

        //Get actions from children
        foreach (MenuOptionAbility s in this.transform.GetComponentsInChildren<MenuOptionAbility>())
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
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () 
    {

    }

    public void Init()
    {
        Debug.Log("Response box script Init called!");
        //Initialize dictionary holding the actions of callout
        CalloutActions = new Dictionary<int, MenuOptionAbility>(); 
        //Get actions from children
        foreach (MenuOptionAbility s in this.transform.GetComponentsInChildren<MenuOptionAbility>())
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

    public CharAbility SelectedAction()
    {
        return this.CalloutActions[CurrIndex].Ability;
    }

    public void CloseCallout()
    {
        //Set initial action back to default
        SetSelectedAction(0);
        //
        this.gameObject.SetActive(false);
    }

    //***************************************************************
    /// <summary>
    /// Creates the UI elements for the character's.
    /// </summary>
    /// <param name="responses">Responses.</param>
    public void CreateActionMenu(List<CharAbility> abilities)
    {
        //references used for positioning created UI elements
        float colXPos = 0;
        //float colXPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        //float firstYPos = 0-this.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        float firstYPos = 0;

        //Iterate through responses and create UI element for each one
        for (int i = 0; i < abilities.Count; i++)
        {
            //Create the element
            GameObject obj = GameObject.Instantiate(aMenuItemObj);
            obj.transform.SetParent(this.gameObject.transform);
            //Set display elements
            obj.GetComponent<MenuOptionAbility>().SetDisplayElements(abilities[i]);
            //Set the order in list of UI items that are dialogue options
            obj.GetComponent<MenuOptionAbility>().OptionIndex = i;
            //Set the corresponding response variable that will be used for reference
            obj.GetComponent<MenuOptionAbility>().Ability = abilities[i];
            //Position the newly created UI element
            RectTransform responseRect = obj.GetComponent<RectTransform>();
            responseRect.anchoredPosition =  new Vector2(colXPos,firstYPos - (responseRect.sizeDelta.y *i));
        }
    }
}
