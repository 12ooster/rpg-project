using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class SpeechUIManager : MonoBehaviour {

    /*Scale of speech box*/
    public Vector3 boxScale;

    /**********Prefabs**********/

    /// <summary>
    /// The prefab of the text box for displaying dialogue.
    /// </summary>
    public GameObject charTextBox;
    /// <summary>
    /// The prefab of the ui text
    /// </summary>
    public GameObject uiText;

    /**********Reference variables**********/
    /// <summary>
    /// The currently displayed text box.
    /// </summary>
    public GameObject currTextBox;
    /// <summary>
    /// The currently displayed text.
    /// </summary>
    public GameObject currText;
    /// <summary>
    /// The currently displayed response box's response script.
    /// </summary>
    public ResponseBoxScript currResponseScript;
    /// <summary>
    /// The curr dialogue chain.
    /// </summary>
    public DialogueChain currDialogueChain;
    /// <summary>
    /// The currently displayed response box. This object is populated from the child of the diaglogue box prefab
    /// </summary>
    public GameObject rBox;

    /// <summary>
    /// The speaker.
    /// </summary>
    public CharacterDialogMgr speaker;

    /// <summary>
    /// The notification box. Used for things like signaling the player has received an item.
    /// </summary>
    public GameObject notifBox;

    /**********UI Canvas and objects**********/

    /// <summary>
    /// The canvas object.
    /// </summary>
    public GameObject CanvasObject;
    /// <summary>
    /// The user interface container.
    /// </summary>
    GameObject _uiContainer;


    float _textDelayTime = 0.05f;

    /// <summary>
    /// Gets a value indicating whether this <see cref="SpeechUIManager"/> is waiting for player input.
    /// </summary>
    /// <value><c>true</c> if wait for player; otherwise, <c>false</c>.</value>
    public bool WaitForPlayer
    {
        get{return this._waitForPlayer;}
    }
    /// <summary>
    /// The wait for player flag indicating if instance is waiting for player input.
    /// </summary>
    bool _waitForPlayer = false;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="SpeechUIManager"/> is waiting for input on a dialogue.
    /// </summary>
    /// <value><c>true</c> if dialogue choice; otherwise, <c>false</c>.</value>
    public bool DialogueChoice
    {
        get{ return this._dialogueChoice;}
        set{ this._dialogueChoice = value;}
    }

    bool _dialogueChoice = false;

    /// <summary>
    /// Flag indicating if a dialogue with the player is currently active
    /// </summary>
    public bool DialogueActive = false;

    /// <summary>
    /// Flag indicating if there is an additional dialogue chain to display
    /// </summary>
    bool newDialogueChain = false;

    /// <summary>
    /// Static instance of this object for singleton pattern
    /// </summary>
    [SerializeField]
    public static SpeechUIManager _instance;

    public int CurrDialogueIndex
    {
        get
        {
            return this._currDialogueIndex;
        }
        set
        {
            this._currDialogueIndex = value;
        }
    }

    private int _currDialogueIndex = 0;

    int newDialogueIndex = 0;

    void Init()
    {
        if (SpeechUIManager._instance != null && SpeechUIManager._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SpeechUIManager._instance = this;
        }
    }

    void Awake()
    {
        if (SpeechUIManager._instance != null && SpeechUIManager._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SpeechUIManager._instance = this;
        }
    }

    // Use this for initialization
	void Start () 
    {
        CanvasObject = GameObject.Find("Canvas");
        _uiContainer = GameObject.Find("UIContainer");
	}
	
	// Update is called once per frame
	void Update () 
    {
//        if (DialogueActive)
//        {
//            PopulateText();
//        }
	}

    /// <summary>
    /// Starts the dialogue between the player and an open world character by setting various references
    /// specific to the context.
    /// </summary>
    /// <param name="speakerPos">Speaker position.</param>
    /// <param name="dialogueChain">Dialogue chain.</param>
    /// <param name="offset">Offset.</param>
    /// <param name="currSpeaker">Curr speaker.</param>
    public void StartDialogue(Transform speakerPos, DialogueChain dialogueChain, Vector2 offset, CharacterDialogMgr currSpeaker)
    {
        //Get a reference to the current speaker
        this.speaker = currSpeaker;
        //Get reference to cuurent dialogue chain - string of text and objects for decision making
        currDialogueChain = dialogueChain;
        //Create UI elements
        currTextBox = CreateSpeechBubble(speakerPos, dialogueChain, offset, ref currText);
    }

    /// <summary>
    /// Creates the speech bubble.
    /// </summary>
    /// <returns>The speech bubble.</returns>
    /// <param name="speakerPos">Speaker position.</param>
    /// <param name="dialogue">Dialogue.</param>
    /// <param name="offset">Offset.</param>
    /// <param name="displayText">Display text.</param>
    public GameObject CreateSpeechBubble(Transform speakerPos, DialogueChain dialogue, Vector2 offset, ref GameObject displayText)
    {
        Debug.Log("CreateSpeechBubble called.");
        //Canvas canvas;
        GameObject tBox = (GameObject)GameObject.Instantiate(charTextBox);
        tBox.transform.SetParent(_uiContainer.transform,false);
        //Get reference to response box and its script then disable it for now
        rBox =  tBox.transform.Find("ResponseBox").gameObject;
        currResponseScript = tBox.GetComponentInChildren<ResponseBoxScript>();
        rBox.SetActive(false);

        //Rect transform for UI element
        RectTransform uiBoxRect = tBox.GetComponent<RectTransform>();

        //Get RectTransform for canvas
        RectTransform CanvasRect = CanvasObject.GetComponent<RectTransform>();

        //Calculate position for the UI Element - 0,0 for the canvas is at the cneter of the screen, whereas World to viewPortPoint
        //treats the lower left as 0,0.  Because of this, you need to subrat the height/width of the canvas * 0.5 to get the correct pos

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(speakerPos.position);
        Vector2 speakerScreenPos = new Vector2(
                                       ((viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                                       ((viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
                                            
        /*position over desired character. Y offset needs to be double as sprite's origin pos is at its center*/
        Vector2 uiObjPos = new Vector2(speakerScreenPos.x, speakerScreenPos.y + (offset.y * 2f)); 

        uiBoxRect.anchoredPosition = uiObjPos;

        /*Create Text*/ 
        displayText = (GameObject)GameObject.Instantiate(uiText);
        displayText.transform.SetParent(_uiContainer.transform,false);
        /*Set text position*/
        RectTransform textRect = displayText.GetComponent<RectTransform>();
        /*Zero it within parent*/
        textRect.anchoredPosition = uiObjPos;
       
        /*Set flag to display string arguments*/
        DialogueActive = true;
        StartCoroutine(PopulateText(dialogue));

        /*Return ui text box*/       
        return tBox;
    }

//    public void PopulateText()
//    {
//        DialogueChain dialogue = currDialogueChain;
//        /*Create object to pass to text coroutine*/
//        SpeechText sText = new SpeechText(dialogue.SpeechText[CurrDialogueIndex].Text, currText);
//
//        int count = 0;
//        //TODO: Add text population word by word with overflow detect
//        foreach (string s in sText.Text)
//        {
//            count++;
//
//            for (int i = 0; i < s.Length; i++)
//            {
//                if (CheckTextOverflowHeight(sText.UIText.GetComponent<RectTransform>()))
//                {
//                    Debug.Log("Text overflowed height!");
//                    yield return null;
//                }
//                else
//                {
//                    sText.UIText.GetComponent<Text>().text += s.Substring(i, 1); 
//                    yield return(new WaitForSeconds(_textDelayTime));
//                }
//
//            }
//
//
//
//            if (count < sText.Text.Length)
//            {
//                Debug.Log("Waiting for player input");
//
//                //Check for if this requires responses
//                if (currDialogueChain.SpeechText[this.CurrDialogueIndex].ReqResponse && !rBox.activeInHierarchy)
//                {
//                    Debug.Log("Response required detected!!");
//                    Debug.Log("Dialogue text at 0 is: "+ currDialogueChain.SpeechText[0].Text[0]);
//                    rBox.SetActive(true);
//
//                    currResponseScript.CreateResponses(currDialogueChain.SpeechText[this.CurrDialogueIndex].Responses);
//                    //Initialize references for UI elements for dialogue responses
//                    currResponseScript.Init();
//                    //Set flag to wait for response
//                    this.DialogueChoice = true;
//                }
//                //flag to wait for player input
//                _waitForPlayer = true;
//
//                //Turn on continue arrow
//                sText.UIText.GetComponent<TextBoxScript>().SwitchArrow();
//                /*String finished populating, now we wait for player's input to continue*/
//                while (_waitForPlayer)
//                {
//                    yield return null;
//                }
//                //turn off continue arrow that is parented to the UI text object
//                sText.UIText.transform.GetComponent<TextBoxScript>().SwitchArrow();
//                //Clear previous string from text box
//                sText.UIText.GetComponent<Text>().text = "";
//            }
//            else
//            {
//                Debug.Log("End of dialog reached. Waiting for player to close.");
//                //do nothing
//                _waitForPlayer = true;
//
//                while (_waitForPlayer)
//                {
//                    yield return null;
//                }
//                //Check to see if there is a new dialog chain to display
//                if (newDialogueChain)
//                {
//                    newDialogueChain = false;
//                }
//                else
//                {
//                    //if no new dialogue chain then End of dialog reached so close text box
//                    CloseTextBox();
//                }
//            }
//        }
//
//
//    }
   
    IEnumerator PopulateText(DialogueChain dialogue)
    {
        
        while (DialogueActive)
        {
            Debug.Log("Top of while loop");
            /*Create object to pass to text coroutine*/
            SpeechText sText = new SpeechText(dialogue.SpeechText[CurrDialogueIndex].Text, currText);
            /*set reference variable for debug*/
            currDialogueChain = dialogue;

    
            //TODO: Add text population word by word with overflow detect
            for(int count = 0; count <= sText.Text.Length; count++)
            {
                
                if (count < sText.Text.Length)
                {
                    for (int i = 0; i < sText.Text[count].Length; i++)
                    {
                        if (CheckTextOverflowHeight(sText.UIText.GetComponent<RectTransform>()))
                        {
                            Debug.Log("Text overflowed height!");
                            yield return null;
                        }
                        else
                        {
                            sText.UIText.GetComponent<Text>().text += sText.Text[count].Substring(i, 1); 
                            yield return(new WaitForSeconds(_textDelayTime));
                        }

                    }

                    Debug.Log("Waiting for player input");
                    Debug.Log("--Count is:" + count + "---");
                    Debug.Log("SText.Text.Length is:" + sText.Text.Length);
                    //Check if the character gives the player an item
                    if (currDialogueChain.SpeechText[this.CurrDialogueIndex].ItemIndex != -1)
                    {
                        int index = currDialogueChain.SpeechText[this.CurrDialogueIndex].ItemIndex;
                        //Call player state manager to add item
                        PlayerStateManager.Instance.AddToInventory(speaker.dialogueEventItems[index],true);
                        //Create notification
                        GameObject notif = (GameObject)GameObject.Instantiate(notifBox);
                        //Set parent to ui_container because the object is a ui elemetn
                        notif.transform.SetParent(_uiContainer.transform,false);
                        //position notification and set reference elements as part of initialization
                        notif.GetComponent<NotificationScript>().Init(speaker.dialogueEventItems[index].thisItem.PickupText,PlayerStateManager.Instance.LocalPosition);
                        //Trigger fade out
                        notif.GetComponent<NotificationScript>().StartFadeOut();
                    }
                    //Check for if this requires responses
                    if (currDialogueChain.SpeechText[this.CurrDialogueIndex].ReqResponse && !rBox.activeInHierarchy)
                    {
//                        Debug.Log("Response required detected!!");
//                        Debug.Log("Dialogue text at 0 is: " + currDialogueChain.SpeechText[0].Text[0]);
                        rBox.SetActive(true);

                        currResponseScript.CreateResponses(currDialogueChain.SpeechText[this.CurrDialogueIndex].Responses);
                        //Initialize references for UI elements for dialogue responses
                        currResponseScript.Init();
                        //Set flag to wait for response
                        this.DialogueChoice = true;
                    }
                    //flag to wait for player input
                    _waitForPlayer = true;

                    //Turn on continue arrow
                    sText.UIText.GetComponent<TextBoxScript>().SwitchArrow();
                    /*String finished populating, now we wait for player's input to continue*/
                    while (_waitForPlayer)
                    {
                        yield return null;
                    }
                    //turn off continue arrow that is parented to the UI text object
                    sText.UIText.transform.GetComponent<TextBoxScript>().SwitchArrow();
                    //Clear previous string from text box
                    sText.UIText.GetComponent<Text>().text = "";
                }
                else
                {
//                    Debug.Log("End of dialog reached. Waiting for player to close.");
//                    //do nothing
//                    _waitForPlayer = true;
//
//                    while (_waitForPlayer)
//                    {
//                        yield return null;
//                    }

                    if (newDialogueChain)
                    {
                        Debug.Log("New dialog chain created!");
                        this.CurrDialogueIndex = this.newDialogueIndex;
                    }
                    else
                    {
                        //Update the Dialogue Manager's state
                        this.DialogueActive = false;
                        //End of dialog reached so close text box
                        CloseTextBox();
                    }
                }
            }
        }

    }

    public void CloseTextBox()
    {
        //Clean up objects from dialogue
        GameObject tBox = currTextBox;
        GameObject text = currText;

        //Reset indices for dialog positions
        CurrDialogueIndex = 0;
        newDialogueIndex = 0;

        //TextBoxScript tScript= currTextBox.GetComponent<TextBoxScript>();
        //delete box
        currTextBox = null;
        rBox = null;
        currResponseScript = null;
        Destroy(tBox);
        //delete text
        currText = null;
        Destroy(text);
        //Reset speaker script reference;
        speaker = null;
        //Update player state
        PlayerController._instance.EndDialogue();
    }

    /// <summary>
    /// Checks if text will overflow the width of its containing rect.
    /// </summary>
    /// <returns><c>true</c> If text exceeds rectangle width. <c>false</c> otherwise.</returns>
    /// <param name="textRect">Text rect.</param>
    bool CheckTextOverflowWidth(RectTransform textRect)
    {
        float textPrefWidth = LayoutUtility.GetPreferredWidth(textRect);

        return(textPrefWidth > textRect.rect.width);
    }

    /// <summary>
    /// Checks if text will overflow the height of its containing rect.
    /// </summary>
    /// <returns><c>true</c> If text exceeds rectangle height. <c>false</c> otherwise.</returns>
    /// <param name="textRect">Text rect.</param>
    bool CheckTextOverflowHeight(RectTransform textRect)
    {
        float textPrefHeight = LayoutUtility.GetPreferredHeight(textRect);

        return(textPrefHeight > textRect.rect.height);
    }

    public void ContinueDialog(DialogueResponse response = null)
    {
        if (response == null)
        {
            Debug.Log("ContinueDialog upper!");
            //No response so no trigger for a new dialogue chain
            newDialogueChain = false;
            //No longer waiting for player input
            this._waitForPlayer = false;
        }
        else
        {
            Debug.Log("ContinueDialog lower!");
            //Update dialogue chain index based on response
            newDialogueIndex = response.DialogueIndex;
            //Set flag indicating there is a new dialogue chain
            newDialogueChain = true;
            //Close response box
            CloseResponseBox();
            //Reset flag for waiting on dialog choice
            this.DialogueChoice = false;
            //Continue
            this._waitForPlayer = false;
        }
    }

    public void CloseResponseBox()
    {
        //Destroy the list of current responses

        //Set response box UI inactive
        rBox.SetActive(false);
    }
}
