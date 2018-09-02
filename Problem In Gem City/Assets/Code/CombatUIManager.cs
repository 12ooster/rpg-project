using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CombatUIManager : MonoBehaviour {

    /// <summary>
    /// The UI elements currently displaying character abilities in a box.
    /// </summary>
    public GameObject aBox;

    /// <summary>
    /// The singleton instance of the Combat UI Manager.
    /// </summary>
    public static CombatUIManager _instance;

    /// <summary>
    /// The canvas object.
    /// </summary>
    public GameObject CanvasObject;

    /// <summary>
    /// The user interface container.
    /// </summary>
    GameObject _uiContainer;

    /// <summary>
    /// The prefab of the ui text
    /// </summary>
    public GameObject uiText;

    AbilityBoxScript currAbilityScript;

    /// <summary>
    /// The position for the action selection box
    /// </summary>
    public Vector2 ActionMenuPos;

    /// <summary>
    /// The user interface action menu.
    /// </summary>
    public GameObject uiActionMenu;
    /// <summary>
    /// The script for functionality of the user interface action menu.
    /// </summary>
    public ActionMenuScript actMenuScript;

    /// <summary>
    /// The user interface end combat screen. Displays xp or items gained and any other relevant info.
    /// </summary>
    public GameObject uiEndCombatScreen;

    /// <summary>
    /// The script for functionality of the screen displayed on combat end.
    /// </summary>
    public CombatEndScreenScript endScreenScript;

    /// <summary>
    /// The index of the currently selected action menu item.
    /// </summary>
    public int SelectedActionIndex = 0;

    /// <summary>
    /// The index of the currently character in relevant menus
    /// </summary>
    public int SelectedCharIndex = 0;

    //Flag for menu level
    public GameConstants.CombatMenuPage MenuLevel;

    public List<CharMgrScript> possibleTargets;
    public List<CharMgrScript> currTargets;

    //Handle object initial actions
    void Awake()
    {
        //Implement this as a singleton to ensure only one instance at any given time
        if (CombatUIManager._instance != null && CombatUIManager._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Set this as the instance of the singleton
            CombatUIManager._instance = this;
            //Get reference for canvas object
            if (CanvasObject == null)
            {
                CanvasObject = GameObject.Find("Canvas");

            }
            if (_uiContainer == null)
            {
                _uiContainer = GameObject.Find("UIContainer");
            }

            //Initialize MenuLevel to unopened at start
            this.MenuLevel = GameConstants.CombatMenuPage.Unopened;

            //Initialize dictionaries and lists
            possibleTargets = new List<CharMgrScript>();
            currTargets = new List<CharMgrScript>();

            //Grab references to the necessary prefabs
            if (uiEndCombatScreen == null)
            {
                uiEndCombatScreen = (GameObject)Resources.Load("Prefabs/UI/CollectItemEmpty");
            }
                
        }

    }

    // Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    /// <summary>
    /// Initialize necessary objects and references for combat when combat scene is loaded
    /// </summary>
    public void InitializeUI()
    {
        
    }

    /// <summary>
    /// Creates the action selection UI menu and g.
    /// </summary>
    /// <returns>The action menu.</returns>
    /// <param name="actionBoxPos">Action box position.</param>
    public GameObject CreateActionMenu(Vector2 actionBoxPos)
    {
        //Instantiate ability UI box
        GameObject actionMenuUI = (GameObject)GameObject.Instantiate(uiActionMenu);
        //Get reference to the script of the actin menu
        this.actMenuScript = actionMenuUI.GetComponent<ActionMenuScript>();

        //Print out for test
        if (TestScript._instance.TestMode)
        {
            Debug.Log("~~~~ Create Action Menu Called ~~~~");
        }

        //Parent action menu to the canvas
        actionMenuUI.transform.SetParent(_uiContainer.transform,false);
        //Return actionMenuUi object for chaining if necessary
        return actionMenuUI;
    }
        

    //TODO:Finish this method for displaying abilities for correct character and check logic / test
    public GameObject CreateAbilityBox(Transform combatantPos, ref GameObject displayText)
    {
        //Instantiate ability UI box
        GameObject abilityUIBox = (GameObject)GameObject.Instantiate(aBox);
        //Parent the UI box to the canvas
        abilityUIBox.transform.SetParent(_uiContainer.transform,false);
        //Get reference to script on UI object
        currAbilityScript = aBox.GetComponentInChildren<AbilityBoxScript>();


        //Rect transform for UI element
        RectTransform uiBoxRect = aBox.GetComponent<RectTransform>();

        //Get RectTransform for canvas
        RectTransform CanvasRect = CanvasObject.GetComponent<RectTransform>();

        //Calculate position for the UI Element - 0,0 for the canvas is at the center of the screen, whereas World to viewPortPoint
        //treats the lower left as 0,0.  Because of this, you need to subrat the height/width of the canvas * 0.5 to get the correct pos

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(combatantPos.position);
        Vector2 speakerScreenPos = new Vector2(
            ((viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        /*position the ability box over desired character. Y offset needs to be double as sprite's origin pos is at its center*/

        //Get reference to current combatant's sprite
        Sprite charSprite = combatantPos.gameObject.GetComponent<SpriteRenderer>().sprite;
        //Calculate offset for positioning
        Vector2 offset = new Vector2(charSprite.rect.width, charSprite.rect.height);
        //Position the box around the combatant's position
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
//        DialogueActive = true;
//        StartCoroutine(PopulateText(dialogue));

        /*Return ui text box*/       
        return abilityUIBox;
    }

    public void UpdateMenuFromSelection(GameConstants.ActionOptionIndices selection)
    {  

        if (TestScript._instance.TestMode)
        {
            Debug.Log("UPDATE menu from selection called in Combat UI mgr!!");
        }
        //Set the selected menu state as active
        CombatUIManager._instance.actMenuScript.SetSelectedState(selection,true);
        CharMgrScript character = CombatMgr._instance.currentTurnChar;

        switch(selection)
        {
            //Update menu level to reflect game state
            case(GameConstants.ActionOptionIndices.Attack):
                if (TestScript._instance.TestMode)
                {
                    Debug.Log("UpdateMenuFromSelection - Entered switch and refreshed targets!!");
                }
                //Update menu level to the match
                CombatUIManager._instance.MenuLevel = GameConstants.CombatMenuPage.Attack;
                //Update lists of targets - First check to see who is attacking, player party or enemy
                bool playerParty = CombatMgr._instance.PlayerParty.Contains(character);
                //Get list of all possible targets for moving selection UI
                possibleTargets = CombatMgr._instance.GetPossibleTargets(character,character.GetBasicAttack(),playerParty);
                //Get list of the currently higlighted targets for the start
                currTargets = CombatMgr._instance.GetNextTargets(character, character.GetBasicAttack(),currTargets,possibleTargets, playerParty,1);
                //Turn on highlight arrows for current targets
                foreach(CharMgrScript c in currTargets)
                {
                    c.HighlightArrowStatus(true);
                }
            break;
            default:
                
            break;
        }

    }

    public void UpdateSelectionAttackTarget(int dir)
    {
        CharMgrScript character = CombatMgr._instance.currentTurnChar;

        //Update lists of targets - First check to see who is attacking, player party or enemy
        bool playerParty = CombatMgr._instance.PlayerParty.Contains(character);
       
        //Get ref to targets to deactivate highlight arrow after new list is found
        List<CharMgrScript> prevTargets = currTargets;

        //TODO:Handle moving 'target' arrow for attack selection based on player input
        possibleTargets = CombatMgr._instance.GetPossibleTargets(character,character.GetBasicAttack(),playerParty);
        currTargets = CombatMgr._instance.GetNextTargets(character,character.GetBasicAttack(),currTargets,possibleTargets,playerParty,dir);

        //deactivate old arrows
        foreach (CharMgrScript p in prevTargets)
        {
            p.HighlightArrowStatus(false);
        }

        //Turn on highlight arrows for current targets
        foreach(CharMgrScript c in currTargets)
        {
            c.HighlightArrowStatus(true);
        }

    }

    /// <summary>
    /// Hides the selection arrows on all combatants by disabling the gameobject.
    /// </summary>
    public void HideSelectionArrows()
    {
        //Turn on highlight arrows for current targets
        foreach(CharMgrScript c in CombatMgr._instance.Combatants)
        {
            c.HighlightArrowStatus(false);
        }
    }

    public void HideHPText(List<CharMgrScript> characters)
    {
        foreach(CharMgrScript c in characters)
        {
            c.HPTextStatus(false);
        }
    }

    public void DisplayEndScreen(string endMessage, string xp, List<string> foundItems )
    {
        /*Create end screen UI object for player and parent it to the canvas*/

        //Instantiate ability UI box
        GameObject endScreen = (GameObject)GameObject.Instantiate(uiEndCombatScreen);
        //Get reference to the script of the actin menu
        this.endScreenScript = endScreen.GetComponent<CombatEndScreenScript>();

        //Print out for test
        if (TestScript._instance.TestMode)
        {
            Debug.Log("~~~~ Create Action End Screen Called ~~~~");
        }

        //Parent action menu to the canvas
        endScreen.transform.SetParent(_uiContainer.transform,false);
        endScreenScript.SetElements(endMessage,xp, foundItems);
    }

    /// <summary>
    /// Handles the input.
    /// </summary>
    /// <param name="keyPressed">Key pressed.</param>
    public void HandleInput(KeyCode keyPressed)
    {
//        if (TestScript._instance.TestMode)
//        {
//            Debug.LogWarning("~~~~2. Combat UI Handling Input ~~~~");
//        }

        if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.Action)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    //Reduce Index by 2 if under the right conditions. If index is over count
                    if (actMenuScript.CurrIndex >= (actMenuScript.ActionMenuItems.Count - actMenuScript.ActionMenuItems.Count / 2))
                    {
                        actMenuScript.SetSelectedAction(false, -1 * (actMenuScript.ActionMenuItems.Count / 2));
                    }
                    break;
                case(KeyCode.RightArrow):
                    if (actMenuScript.CurrIndex <= (actMenuScript.ActionMenuItems.Count / 2))
                    {
                        actMenuScript.SetSelectedAction(false, 2);
                    }
                    break;
                case(KeyCode.UpArrow):
                    actMenuScript.SetSelectedAction(false, -1);
                    break;
                case(KeyCode.DownArrow):
                    actMenuScript.SetSelectedAction(false, 1);
                    break;
                case(KeyCode.Space):
                    //Selection key, so set ui active state and start action menu
                    actMenuScript.ActionMenuItems[actMenuScript.CurrIndex].Action(CombatMgr._instance.currentTurnChar);

                    break;
                case(KeyCode.Escape):
                    break;
                default:
                    break;
            }
        }
        else if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.Attack)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    break;
                case(KeyCode.RightArrow):
                    break;
                case(KeyCode.UpArrow):
                    //Update the selected target
                    UpdateSelectionAttackTarget(-1);
                    break;
                case(KeyCode.DownArrow):
                    //Update the selected target
                    UpdateSelectionAttackTarget(1);
                    break;
                case(KeyCode.Space):
                    //Execute attack
                    CombatMgr._instance.PerformAttack(CombatMgr._instance.currentTurnChar.GetBasicAttack(), currTargets);
                    break;
                case(KeyCode.Escape):
                    /*Exit this menu level and go back to action select*/
                    //Revert UI states
                    CombatUIManager._instance.actMenuScript.SetSelectedState(GameConstants.ActionOptionIndices.Attack, false);
                    //Remove selection arrows and hp text
                    foreach (CharMgrScript c in currTargets)
                    {
                        c.HighlightArrowStatus(false);
                        c.HPTextStatus(false);
                    }
                    //reset target lists
                    possibleTargets.Clear();
                    currTargets.Clear();
                    //Set the flag for menu state
                    CombatUIManager._instance.MenuLevel = GameConstants.CombatMenuPage.Action;
                    break;
                default:
                    break;
            }
        }
        else if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.Ability)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    break;
                case(KeyCode.RightArrow):
                    break;
                case(KeyCode.UpArrow):
                    break;
                case(KeyCode.DownArrow):
                    break;
                case(KeyCode.Space):
                    break;
                case(KeyCode.Escape):
                    break;
                default:
                    break;
            }
        }
        else if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.Flee)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    break;
                case(KeyCode.RightArrow):
                    break;
                case(KeyCode.UpArrow):
                    break;
                case(KeyCode.DownArrow):
                    break;
                case(KeyCode.Space):
                    break;
                case(KeyCode.Escape):
                    break;
                default:
                    break;
            }
        }
        else if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.Item)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    break;
                case(KeyCode.RightArrow):
                    break;
                case(KeyCode.UpArrow):
                    break;
                case(KeyCode.DownArrow):
                    break;
                case(KeyCode.Space):
                    break;
                case(KeyCode.Escape):
                    break;
                default:
                    break;
            }
        }
        else if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.AnimationAction)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    break;
                case(KeyCode.RightArrow):
                    break;
                case(KeyCode.UpArrow):
                    break;
                case(KeyCode.DownArrow):
                    break;
                case(KeyCode.Space):
                    break;
                case(KeyCode.Escape):
                    break;
                default:
                    break;
            }
        }
        else if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.EndScreen)
        {
            switch (keyPressed)
            {
                case(KeyCode.LeftArrow):
                    break;
                case(KeyCode.RightArrow):
                    break;
                case(KeyCode.UpArrow):
                    break;
                case(KeyCode.DownArrow):
                    break;
                case(KeyCode.Space):
                    //TODO:Clean up combat UI. This can be handled here: Call methods needed to hide or destroy objects.

                    //Clean up action menu object and remove from screen
                    Destroy(this.actMenuScript.gameObject);
                    actMenuScript = null;
                    //Clean up end of combat UI
                    Destroy(this.endScreenScript.gameObject);
                    endScreenScript = null;
                    //Reset state flags as needed
                    this.MenuLevel = GameConstants.CombatMenuPage.Unopened;
                    //Leave combat scene and load overworld scene
                    CombatMgr._instance.LeaveCombatScene();

                    break;
                case(KeyCode.Escape):
                    break;
                default:
                    break;
            }
        }

    }

    public void UpdateHPText()
    {
        foreach (CharMgrScript c in CombatMgr._instance.Combatants)
        {
            c.HPText = c.stats.HP.ToString() + "/" + c.stats.MaxHP.ToString();
        }
    }
}
