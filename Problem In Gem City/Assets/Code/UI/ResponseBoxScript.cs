using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseBoxScript : MonoBehaviour {

    [SerializeField]
    public Dictionary<int,MenuOptionDialogue> CalloutActions; //TODO:Fix bug with the inheritied classes not being added to this dictionary. May need to use gameobjects? 
    //List UIItem scripts
    public List<MenuOptionDialogue> CallOutActionsList;
    public int CurrIndex = -1;
    public MenuOptionDialogue SelectedResponse;
    public GameObject dMenuItemObj;
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
        CalloutActions = new Dictionary<int, MenuOptionDialogue>(); 
        CallOutActionsList = new List<MenuOptionDialogue>();

        //Get actions from children
        foreach (MenuOptionDialogue s in this.transform.GetComponentsInChildren<MenuOptionDialogue>())
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
        CalloutActions = new Dictionary<int, MenuOptionDialogue>(); 
        //Get actions from children
        foreach (MenuOptionDialogue s in this.transform.GetComponentsInChildren<MenuOptionDialogue>())
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

    public DialogueResponse SelectedAction()
    {
        return this.CalloutActions[CurrIndex].Response;
    }

    public void CloseCallout()
    {
        //Set initial action back to default
        SetSelectedAction(0);
        //
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Creates the UI elements for dialogue responses.
    /// </summary>
    /// <param name="responses">Responses.</param>
    public void CreateResponses(List<DialogueResponse> responses)
    {
        //references used for positioning created UI elements
        float colXPos = 0;
        //float colXPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        //float firstYPos = 0-this.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        float firstYPos = 0;

        //Iterate through responses and create UI element for each one
        for (int i = 0; i < responses.Count; i++)
        {
            //Create the element
            GameObject obj = GameObject.Instantiate(dMenuItemObj);
            obj.transform.SetParent(this.gameObject.transform);
            //Set display elements
            obj.GetComponent<MenuOptionDialogue>().SetDisplayElements(responses[i]);
            //Set the order in list of UI items that are dialogue options
            obj.GetComponent<MenuOptionDialogue>().OptionIndex = i;
            //Set the corresponding response variable that will be used for reference
            obj.GetComponent<MenuOptionDialogue>().Response = responses[i];
            //Position the newly created UI element
            RectTransform responseRect = obj.GetComponent<RectTransform>();
            responseRect.anchoredPosition =  new Vector2(colXPos ,firstYPos - (responseRect.sizeDelta.y *i));
        }
    }
}
