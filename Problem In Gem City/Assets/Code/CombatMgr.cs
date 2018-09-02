using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.SceneManagement;

public class CombatMgr : MonoBehaviour {

    /// <summary>
    /// The enemy party data.
    /// </summary>
    [SerializeField]
    public List<CharStatsData> EnemyPartyData;

    /// <summary>
    /// Empty character object for instantiation purposes
    /// </summary>
    public GameObject EmptyCharacter;

    /// <summary>
    /// The highlight arrow object prefab.
    /// </summary>
    public GameObject HighlightArrowObj;

    /// <summary>
    /// The hp ui text object prefab.
    /// </summary>
    public GameObject HPUITxtObj;

    /// <summary>
    /// The singleton instance of the combat manager
    /// </summary>
    public static CombatMgr _instance;

    /// <summary>
    /// The dist between chars in combat.
    /// </summary>
    [SerializeField]
    public Vector3 DistCharsCombat;

    /// <summary>
    /// The starting position for calculating party member positions in combat.
    /// </summary>
    public Transform CombatPartyStartPos;
    /// <summary>
    /// The starting position for calculating enemy positions in combat.
    /// </summary>
    public Transform CombatEnemyStartPos;

    /// <summary>
    /// The player party.
    /// </summary>
    public List<CharMgrScript> PlayerParty;

    /// <summary>
    /// The enemy party
    /// </summary>
    public List<CharMgrScript> EnemyParty;

    /// <summary>
    /// Character manager scripts for all combatants in the battle, sorted by speed, which is the key
    /// </summary>
    public List<CharMgrScript> Combatants;

    /// <summary>
    /// The type of initiative used for determining turn order in battle
    /// </summary>
    public GameConstants.TurnOrderType InitiativeType;

    /// <summary>
    /// The wait for player flag indicating if instance is waiting for player input.
    /// </summary>
    [SerializeField]
    bool _waitForPlayer = false;

    /// <summary>
    /// Gets a value indicating whether this <see cref="CombatMgr"/> is waiting for player input.
    /// </summary>
    /// <value><c>true</c> if wait for player; otherwise, <c>false</c>.</value>
    public bool WaitForPlayer
    {
        get{return this._waitForPlayer;}
    }

    /// <summary>
    /// The flag indicating the system is waiting for an NPC to take its turn.
    /// </summary>
    [SerializeField]
    bool _waitForNPC = false;

    public bool WaitForNPC
    {
        get{ return this._waitForNPC;}
    }

    /// <summary>
    /// The wait for player flag indicating if instance is waiting for player input.
    /// </summary>
    bool _initializing = false;

    /// <summary>
    /// Gets a value indicating whether this <see cref="CombatMgr"/> is waiting for player input.
    /// </summary>
    /// <value><c>true</c> if wait for player; otherwise, <c>false</c>.</value>
    public bool Initializing
    {
        get{return this._initializing;}
    }

    /// <summary>
    /// The wait for player flag indicating if instance is waiting for player input.
    /// </summary>
    bool _turnStarted = false;

    /// <summary>
    /// Gets a value indicating whether this <see cref="CombatMgr"/> has started a character's turn or not.
    /// </summary>
    /// <value><c>true</c> if a character's turn has started <c>false</c>.</value>
    public bool TurnStarted
    {
        get{return this._turnStarted;}
        set{ this._turnStarted = value; }
    }

    /// <summary>
    /// The wait for player flag indicating if instance is waiting for player input.
    /// </summary>
    bool _resolvingAction = false;

    /// <summary>
    /// Gets a value indicating whether this <see cref="CombatMgr"/> is waiting for player input.
    /// </summary>
    /// <value><c>true</c> if wait for player; otherwise, <c>false</c>.</value>
    public bool ResolvingAction
    {
        get{return this._resolvingAction;}
    }

    /// <summary>
    /// The UI elements currently displaying abilities in a box.
    /// </summary>
    public GameObject aBox;

    /// <summary>
    /// The character who is currently taking their turn
    /// </summary>
    public CharMgrScript currentTurnChar;

    /// <summary>
    /// The scene to which to return if the first party wins. If player vs. ai, then it's player party victory.
    /// </summary>
    public Scene PartyOneReturnScene;

    /// <summary>
    /// The scene to which to return if the first party wins. If player vs. ai, then it's player party victory.
    /// </summary>
    public Scene PartyTwoReturnScene;

    /// <summary>
    /// The items the player could find from battle
    /// </summary>
    public List<InventoryItemScript> FoundItems;

    /// <summary>
    /// The variables and data relevant to resolving the exit from this scene
    /// </summary>
    public CombatEndData EndData;

	// Use this for initialization
	void Start () 
    {
        //Enforce singleton pattern. If another combat manager exists, destroy this
        if (CombatMgr._instance != null && CombatMgr._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Otherwise make this the static instance
            CombatMgr._instance = this;

            if (FoundItems == null)
            {
                FoundItems = new List<InventoryItemScript>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    public void Init(Transform partyStartPos, Transform enemyStartPos,List<CharStatsData> EnemyData, GameObject emptyChar)
    {
        /********Set flag to track game state that manager is currently initializing********/
        this._initializing = true;
        /****************/
        //Set player party start pos
        this.CombatPartyStartPos = partyStartPos;
        //Set enemy's start pos
        this.CombatEnemyStartPos = enemyStartPos;
        //Set data for enemy
        this.EnemyPartyData = EnemyData;
        //Set empty char object
        this.EmptyCharacter = emptyChar;
        //Initialize item list
        if (FoundItems == null)
        {
            FoundItems = new List<InventoryItemScript>();
        }
    }

    public void StartCombat(Scene pOneWin, Scene pTwoWin)
    {
        
        Vector3 partyPos, enemyPos;
        this.PartyOneReturnScene = pOneWin;
        this.PartyTwoReturnScene = pTwoWin;

        /*Check referenc to tagged starting pos objects*/
        if (CombatPartyStartPos == null)
        {
            partyPos = new Vector3(GameConstants.CombatPartyStartPosX, GameConstants.CombatPartyStartPosY, 0);
        }
        else
        {
            partyPos = CombatPartyStartPos.position;
        }
        if (CombatEnemyStartPos == null)
        {
            enemyPos = new Vector3(GameConstants.CombatPartyStartPosX, GameConstants.CombatPartyStartPosY, 0);
        }
        else
        {
            enemyPos = CombatEnemyStartPos.position;
        }

        Debug.Log("StartCombat() called!");

        //Counter to use for positioning
        int count = 0;

        //Instantiate an object for each member of the player's party and set their variables accordingly
        foreach (CharStatsData characterStats in PlayerStateManager.Instance.PlayerParty)
        {
            Debug.Log("Party Member"+characterStats.CharName +"Instantiated called!");
            //Debug.Log("Creating character" + character.stats.CharName + "for combat scene.");
            //Create empty object from prefab
            GameObject partyMemberCopy = GameObject.Instantiate(EmptyCharacter);
            //Set position for the party member
            partyMemberCopy.transform.position = partyPos + (DistCharsCombat*(float)count);

            //Grab the new script
            CharMgrScript partyMemberCopyScript = partyMemberCopy.GetComponent<CharMgrScript>();
            //Init necessary variables
            partyMemberCopyScript.Init();
            /* --- Set the variables of the script --- */
            //Dialogue
            partyMemberCopyScript.dialogMgr = characterStats.DialogMgr;
            //Stats & Abilities
            characterStats.StatsAsScript(ref partyMemberCopyScript.stats);
            //Character sprite
            partyMemberCopy.GetComponent<SpriteRenderer>().sprite = characterStats.charSprite;
            //change object name in hierarchy
            partyMemberCopy.name = characterStats.CharName;

            /*Create UI objects (highlight arrow, hp counter) and parent them to the canvas, assign them to the characters script and disable via script*/

            //Create or recreate menu objects and refresh references


            //Instantiate the HP counter UI text prefab
            GameObject hpUIText = GameObject.Instantiate(HPUITxtObj);
            //Parent the ui text to the canvas for positioning purposes
            hpUIText.transform.SetParent(CombatUIManager._instance.CanvasObject.transform,false);
            //Assign the prefab as a member of the charMgr script
            partyMemberCopyScript.HPUITxt = hpUIText;
            //Move the hp text under the character
            partyMemberCopyScript.HPUITxt.GetComponent<UIAnim>().PositionElementOnSprite(partyMemberCopyScript.gameObject);
            //Set HP text's initial values
            partyMemberCopyScript.HPUITxt.GetComponent<UIAnim>().SetText(partyMemberCopyScript.stats.HP.ToString()+"/"+partyMemberCopyScript.stats.MaxHP.ToString());
            //Disable HP Text for now
            partyMemberCopyScript.HPTextStatus(false);

            //Instantiate the arrow prefab
            GameObject highlightArrow = GameObject.Instantiate(HighlightArrowObj);
            //Parent the highlight arrow to the canvas
            highlightArrow.transform.SetParent(CombatUIManager._instance.CanvasObject.transform,false);
            //Assign the sprite component as a member of the charMgr script
            partyMemberCopyScript.HighlightArrow = highlightArrow;
            //Move highlight arrow over character's head via setting it = to pos, + half the sprite's height
            partyMemberCopyScript.HighlightArrow.GetComponent<UIAnim>().PositionElementOnSprite(partyMemberCopyScript.gameObject);
            //Disable highlight arrow for now
            partyMemberCopyScript.HighlightArrowStatus(false);

            /*Add the character script to the total list of combatants for determining turn order*/
            Combatants.Add(partyMemberCopyScript);
            PlayerParty.Add(partyMemberCopyScript);
            //Increment counter for spacing
            count++;
        }

        //Clear counter
        count = 0;

        //Instantiate each enemy party member
        foreach (CharStatsData characterStats in this.EnemyPartyData)
        {
            Debug.Log("Enemy Member"+characterStats.CharName +"Instantiated called!");
            //Create empty enemy object from prefab
            GameObject enemyCharCopy = GameObject.Instantiate(EmptyCharacter);
            //Set position for the enemy
            enemyCharCopy.transform.position = CombatEnemyStartPos.position + (DistCharsCombat*(float)count);
            //Get reference to prefab's script
            CharMgrScript enemyScript = enemyCharCopy.GetComponent<CharMgrScript>();
            //init necessary variables
            enemyScript.Init();
            /* --- Set script variables --*/
            //Dialogue
            enemyScript.dialogMgr = characterStats.DialogMgr;
            //Stats & Abilities
            characterStats.StatsAsScript(ref enemyScript.stats);
            //Character sprite
            enemyScript.GetComponent<SpriteRenderer>().sprite = characterStats.charSprite;
            //change object name in hierarchy
            enemyCharCopy.name = enemyScript.stats.CharName;
            /*Create UI objects, parent them to the canvas, assign them to the the characters scripts and disable via script*/
           
            //Instantiate the HP counter UI text prefab
            GameObject hpUIText = GameObject.Instantiate(HPUITxtObj);
            //Parent the ui text to the canvas for positioning purposes
            hpUIText.transform.SetParent(CombatUIManager._instance.CanvasObject.transform,false);
            //Assign the prefab as a member of the charMgr script
            enemyScript.HPUITxt = hpUIText;
            //Move the hp text under the character
            enemyScript.HPUITxt.GetComponent<UIAnim>().PositionElementOnSprite(enemyScript.gameObject);
            //Set HP text's initial values
            enemyScript.HPUITxt.GetComponent<UIAnim>().SetText(enemyScript.stats.HP.ToString()+"/"+enemyScript.stats.MaxHP.ToString());
            //Disable HP Text for now
            enemyScript.HPTextStatus(false);
           
            //Instantiate the highlight arrow prefab
            GameObject highlightArrow = GameObject.Instantiate(HighlightArrowObj);
            //Parent the highlight arrow to the canvas
            highlightArrow.transform.SetParent (CombatUIManager._instance.CanvasObject.transform,false);
            //Assign the sprite component as a member of the charMgr script
            enemyScript.HighlightArrow = highlightArrow;
            //Move highlight arrow over character's head via setting it = to pos, + half the sprite's height
            enemyScript.HighlightArrow.GetComponent<UIAnim>().PositionElementOnSprite(enemyScript.gameObject);
            //Disable highlight arrow for now
            enemyScript.HighlightArrowStatus(false);

            /*Add the character script to the total list of combatants for determining turn order*/
            Combatants.Add(enemyScript);
            EnemyParty.Add(enemyScript);
            //Increment counter for spacing
            count++;
        }

        /********Set flag to track game state that manager is done initializing and waiting for player input********/
        this._initializing = false;
        //this._waitForPlayer = true;
        /****************/
    }

    /// <summary>
    /// Finds the combatant whose turn it is. Looks through the list for the fastest charcter
    /// </summary>
    /// <returns>The combatant turn.</returns>
    public CharMgrScript GetCombatantTurn(List<CharMgrScript> playerParty, List<CharMgrScript> enemyParty)
    {
        //add the lists of both parties together to get a list of total combatants
        List<CharMgrScript> currCombatants = new List<CharMgrScript>();

        foreach (CharMgrScript c in playerParty)
        {
            currCombatants.Add(c);
        }

        foreach (CharMgrScript enemyChar in enemyParty)
        {
            currCombatants.Add(enemyChar);
        }

        //Get subset of chars that haven't moved yet
        List<CharMgrScript> unmovedCombatants = currCombatants.FindAll(x => x.ActedThisRound == false);
        //Test DEBUG OUTPUT
        if (TestScript._instance.TestMode)
        {
            Debug.Log("UNMOVED COMBATANTS COUNT:" + unmovedCombatants.Count);
        }
        //Get the first list item as a starting point
        CharMgrScript currTurnChar = unmovedCombatants[0];

        switch(InitiativeType)
        {
            case(GameConstants.TurnOrderType.IndividualSpeed):
                //Iterate through each script and compare speed
                foreach (CharMgrScript combatant in unmovedCombatants)
                {
                    //Test DEBUG OUTPUT
                    if (TestScript._instance.TestMode)
                    {
                        Debug.Log("CurrTurnChar has speed of:" + currTurnChar.stats.Speed);
                        Debug.Log("Combatant being compared has speed of:" + currTurnChar.stats.Speed);
                    }

                    //if this character is faster than current script, then this character gets initiative
                    if (combatant.stats.Speed > currTurnChar.stats.Speed)
                    {
                        currTurnChar = combatant;
                    }
                    else if (combatant.stats.Speed == currTurnChar.stats.Speed)//If speed is equal need to determine initiative another way
                    {
                        //TODO:Implement secondary stat check
                        //Until secondary stat check just use random selection between the two
                        int num = Random.Range(0,100);
                        if(num>=50)
                        {
                            //If coin flip is over fifty or equal
                            currTurnChar = combatant;
                        }
                    }
                }
            break;
            case(GameConstants.TurnOrderType.FastestTeammate):
                Debug.LogWarning("Fastest teammate iniative type not implemented!!");
            break;
            default:
                Debug.LogWarning("No turn order type found! Case fell through");
            break;
        }

        return currTurnChar;
    }

    /// <summary>
    /// Starts the turn of the next character. Initializes UI elements, etc.
    /// </summary>
    public void StartNextTurn()
    {

        //Get reference to the script whose turn is next
        CharMgrScript currentChar = this.GetCombatantTurn(this.PlayerParty, this.EnemyParty);
        //Set it to local variable
        currentTurnChar = currentChar;
        //Set flag for which team is taking its turn - In a PVE encounter, either payer party or NPC party
        if (this.PlayerParty.Contains(currentChar))
        {
            /*Debug statement for test*/
            if (TestScript._instance.TestMode)
            {
                Debug.Log("--StartNextTurn() called and character from *Player Party* found--");
            }

            this._waitForPlayer = true;
            //Display the action selection menu if it's not already open and highlight current character
            if (CombatUIManager._instance.MenuLevel == GameConstants.CombatMenuPage.Unopened)
            {
                //Create the action menu
                CombatUIManager._instance.CreateActionMenu(CombatUIManager._instance.ActionMenuPos);
                //Update menu state
                CombatUIManager._instance.MenuLevel = GameConstants.CombatMenuPage.Action;
            }
        }
        else if(this.EnemyParty.Contains(currentChar))
        {

            /*Debug statement for test*/
            if (TestScript._instance.TestMode)
            {
                Debug.Log("--StartNextTurn() called and character from ~~Enemy Party~~ found--");
            }

            this._waitForNPC = true;
            //Setup npc turn and pass in targets
            SetupNPCTurn( currentChar, this.PlayerParty);
        }

        //Handle  w/e character specific highlighting is involved
    }

    /// <summary>
    /// Gets the next targets.
    /// </summary>
    /// <returns>The next targets.</returns>
    /// <param name="currChar">Curr char.</param>
    /// <param name="ability">Ability.</param>
    /// <param name="currTargets">Curr targets.</param>
    /// <param name="playerParty">If set to <c>true</c> then attacking character is a member of the player party.</param>
    /// <param name="dir">Value should be either 1 or -1. This is the direction the selection is moving in the list</param>
    public List<CharMgrScript> GetNextTargets(CharMgrScript currChar, CharAbility ability, List<CharMgrScript> currTargets, List<CharMgrScript> possTargets, bool playerParty,int dir)
    {
        //Pass in ability script, check who is taking their tun and the ability to get the possible targets, then return them as a list
        List<CharMgrScript> nextTargets = new List<CharMgrScript>();
        int numTargets;
        /*Check the target type of the ability and based on the type return the correct targets*/
        switch (ability.TType)
        {
            case(GameConstants.TargetType.EnemyParty):
                numTargets = EnemyParty.Count;
                //Case for hitting whole party
                break;
            case(GameConstants.TargetType.SingleEnemy):
                //For now, use hard coded value, later can look into building this into the ability class
                numTargets = 1;
                //Single enemy attack type so get next item in correct list and return it
                if (playerParty)
                {
                    //If it's the player's party then get a script from npc party as the enemy
                    if (currTargets == null || currTargets.Count == 0)
                    {
                        if (TestScript._instance.TestMode)
                        {
                            Debug.Log("GetNextTargets -> retreived first from list because currTargets was null");
                        }

                        //IF no targets currently just grab the first item from the list
                        //CharMgrScript nTarget = this.EnemyParty[0];
                        CharMgrScript nTarget = possTargets[0];
                        nextTargets.Add(nTarget);
                    }
                    else
                    {
                        /*
                         * Otherwise find the greatest index of the curr targets, 
                         * use it as starting index, increase count by 'number of targets'
                        */
                        int startingIndex = -1;
                        //Find the 'largest' index from current targets to use as a starting point
                        foreach (CharMgrScript c in currTargets)
                        {
//                            if (this.EnemyParty.IndexOf(c) > startingIndex)
//                            {
//                                startingIndex = this.EnemyParty.IndexOf(c);
//                            }
                              if(possTargets.IndexOf(c) > startingIndex)
                              {
                                  startingIndex = possTargets.IndexOf(c);
                              }
                        }

                        for(int i = 0; i < numTargets; i++)
                        {
                            if (startingIndex + dir >= possTargets.Count)
                            {
                                //Reset starting index to avoid being out of bounds
                                startingIndex = 0;
                                //Remove dir because we've rotated around the list
                                dir = 0;
                            }
                            else if (startingIndex + dir < 0)
                            {
                                //Reset starting index to avoid being out of bounds
                                startingIndex = possTargets.Count - 1;
                                //Remove dir because we've rotated around the list
                                dir = 0;
                            }

                            CharMgrScript nTarget = possTargets[startingIndex+dir];
                            nextTargets.Add(nTarget);
                        }
                    }
                }
                else
                {
                    //Handle enemy attack selection?
                }

                break;
            case(GameConstants.TargetType.Party):
                numTargets = PlayerParty.Count;
                break;
            case(GameConstants.TargetType.Ally):
                numTargets = 1;
                break;
            case(GameConstants.TargetType.Self):
                break;
            default:
                break;
        }

        return nextTargets;
    }

    public List<CharMgrScript> GetPossibleTargets(CharMgrScript currChar, CharAbility ability, bool playerParty)
    {
        //Pass in ability script, check who is taking their tun and the ability to get the possible targets, then return them as a list
        List<CharMgrScript> possTargets = new List<CharMgrScript>();

        /*Check the target type of the ability and based on the type return the correct targets*/
        switch (ability.TType)
        {
            case(GameConstants.TargetType.EnemyParty):
                //Case for hitting whole party
                break;
            case(GameConstants.TargetType.SingleEnemy):
                //Single enemy attack type so get next item in correct list and return it
                if (playerParty)
                {
                    //If it's the player's party then the npc party of enemies are possible targets - NOTE: Excluding downed/dead characters 
                    possTargets = this.EnemyParty.FindAll(x =>x.stats.Status != GameConstants.StatusType.Downed);
                    if (TestScript._instance.TestMode)
                    {
                        Debug.Log("POSSIBLE TARGETS OBTAINED! cOUNT IS:" + possTargets.Count);
                    }
                }
                else
                {

                }

                break;
            case(GameConstants.TargetType.Party):
                break;
            case(GameConstants.TargetType.Ally):
                break;
            case(GameConstants.TargetType.Self):
                break;
            default:
                break;
        }

        return possTargets;
    }

    public void SetupNPCTurn(CharMgrScript npcCharacter,List<CharMgrScript> possibleTargets)
    {
        Debug.Log("*&@#$ The NPC: "+this.currentTurnChar.stats.CharName+" TOOK A TURN! *#$&");
        //TODO:Handle logic for selecting action and targets etc, then calling the appropriate method for the NPC
        StartCoroutine(TakeNPCTurn(npcCharacter,possibleTargets));
    }

    public IEnumerator TakeNPCTurn(CharMgrScript npc,List<CharMgrScript> pTargets)
    {
        
        /*Select Character's action*/
        NPCCombatTurn npcTurnData = NPCChoiceMgr._instance.ChooseCombatAction(npc, pTargets);

        //this.PerformAttack( npcTurnData.SelectedTargets, npcTurnData.ability );
        this.PerformAttack( npcTurnData.SelectedAbility, npcTurnData.SelectedTargets);


        yield return null;

        this.EndCombatTurn();
    }

    /// <summary>
    /// Handles the logic of a character making an attack.
    /// </summary>
    /// <param name="attack">Attack.</param>
    /// <param name="targets">Targets.</param>
    public void PerformAttack(CharAbility attack, List<CharMgrScript> targets)
    {
        //Hide selection arrows
        CombatUIManager._instance.HideSelectionArrows();
        //Handle animation and dynamic input in coroutine
        CombatUIManager._instance.MenuLevel = GameConstants.CombatMenuPage.AnimationAction;
        StartCoroutine(CombatAnimInput(attack, targets));
    }
   

    /// <summary>
    /// Handle cleanup and resolution of data on combat end.
    /// </summary>
    public void EndCombat(CombatEndData endData)
    {
        //Clear enemy list
        EnemyPartyData.Clear();
        /*Need to display combat end screen and give player confirmation prompt*/

        //Save data used for resolving end of combat
        this.EndData = endData;
        //Change menu state
        CombatUIManager._instance.MenuLevel = GameConstants.CombatMenuPage.EndScreen;
        //Create list of items as strings
        List<string> itemsText = new List<string>();
        foreach (InventoryItemScript i in endData.FoundItems)
        {
            itemsText.Add(i.ThisItem.ItemName);
        }

        //Call UIMgr to display relevant info
        CombatUIManager._instance.DisplayEndScreen(endData.EndMessage, endData.XP.ToString(),itemsText);
        //Trigger flag to wait for player input
        this._waitForPlayer = true;

    }

    public void LeaveCombatScene()
    {
        //Load Game State
        GameStateMgr._instance.StartLoadingOverworld();
    }

    public void CloseResponseBox()
    {
        //Destroy the list of current responses

        //Set response box UI inactive
        aBox.SetActive(false);
    }

    IEnumerator CombatAnimInput(CharAbility attack, List<CharMgrScript> targets)
    {
        float runTime = 1.0f;
        float step = 0.0f;
        float speed = 0.65f;
        bool combatantsChecked = false;
        List<CharMgrScript> downedCombatants = new List<CharMgrScript>();
        //Handle the lerp pushback of affected character
        while(step < runTime)
        {
            step += speed * Time.deltaTime;
            yield return null;
        }

        if (TestScript._instance.TestMode)
        {
            Debug.Log("Combat Anim Input timer finished");
        }

        foreach (CharMgrScript c in targets)
        {
            c.stats.HP -= attack.BaseDamage;
        }

        //Check for any combatants who were downed in the ability
        while (combatantsChecked == false)
        {
            downedCombatants = CheckDownedCombatants(ref combatantsChecked);
            yield return null;
        }

       
        //Iterate through list of downed combatants, if there are any, play dying animation
        if (downedCombatants != null && downedCombatants.Count != 0)
        {
            foreach (CharMgrScript c in downedCombatants)
            {
               c.EnterDownedState(); 
               //Remove downed enemies for now - note that may need to keep them here if we want to revive them

               //Note - may need to add a check for animation completions
                yield return null;
            }
        }

        CombatUIManager._instance.UpdateHPText();
        CombatUIManager._instance.HideHPText(targets);
        //TODO: Handle reset of UI and end of turn checks (character's removed from battle, battle win/loss conditions) ****

        /*Handle UI menu reset and other flag resolutions so next character can start thier turn*/
        EndCombatTurn();
    }


    public List<CharMgrScript> CheckDownedCombatants(ref bool checkComplete)
    {
        List<CharMgrScript> downed = new List<CharMgrScript>();

        foreach (CharMgrScript c in Combatants)
        {
            //If a character's health points have been reduced to 0 then add it to the list
            if (c.stats.HP <= 0)
            {
                downed.Add(c);
            }
        }

        //Update flag that the check has completed
        checkComplete = true;
        //Return the list of combatants whose HP was reduced to 0
        return downed;
    }

    public void EndCombatTurn()
    {
        /*Check victory and loss conditions - If either is met then end combat in the appropriate fashion*/

        //If all player party is down then trigger combat ending as a loss for party 1 and game over reset if vs. ai
        if (PlayerParty.FindAll(x => x.stats.Status != GameConstants.StatusType.Downed).Count == 0)
        {
            //TODO:Handle player loss

        } //If all enemy party is down then trigger combat ending for party 2 as a loss for party 2 and loading the passed in scene if vs. ai
        else if (EnemyParty.FindAll(x => x.stats.Status != GameConstants.StatusType.Downed).Count == 0)
        {
           
            //TODO:Calculate xp from enemy party by adding up the exp from enemy party or some other format
            int winXp = 0;
            foreach(CharMgrScript c in EnemyParty)
            {
                winXp += c.stats.XP;
            }

            //TODO: Determine retrieved items based off enemy party script
            string battleEndMessage = "Victory!!";

            //Party one was the victor so use their scene as the return point
            CombatEndData d = new CombatEndData(GameConstants.CombatEndType.Party1Win,this.PartyOneReturnScene,FoundItems,winXp,battleEndMessage);
            //Handle procedures for end of combat : displaying UI messages, etc.
            EndCombat(d);
        }
        else
        {
            //Reset button active state using the stored action menu index
            GameConstants.ActionOptionIndices index = (GameConstants.ActionOptionIndices)CombatUIManager._instance.actMenuScript.CurrIndex;
            //Reset menu selected state
            CombatUIManager._instance.actMenuScript.SetSelectedState(index,false);
            //Update menu status - if player didn't win
            CombatUIManager._instance.MenuLevel = GameConstants.CombatMenuPage.Action;
            //Wipe list of current targets from Combat UI mgr
            CombatUIManager._instance.currTargets.Clear();
            //Wipe list of possible targets from Combat UI mgr
            CombatUIManager._instance.possibleTargets.Clear();
            //Flag the current chracter as having acted this round - NOTE: May need to add additional initiative logic here for multiple actors, etc.
            this.currentTurnChar.ActedThisRound = true;

            //add the lists of both parties together to get a list of total combatants
//            List<CharMgrScript> currCombatants = this.PlayerParty;
//            foreach (CharMgrScript enemyChar in this.EnemyParty)
//            {
//                currCombatants.Add(enemyChar);
//            }

            //If all characters have moved this round, then reset their flags
            if (this.PlayerParty.FindAll(x => x.ActedThisRound == false).Count == 0 ||
                this.EnemyParty.FindAll(x=>x.ActedThisRound == false).Count == 0)
            {
                foreach (CharMgrScript c in this.PlayerParty)
                {
                    c.ActedThisRound = false;
                }

                foreach (CharMgrScript e in this.EnemyParty)
                {
                    e.ActedThisRound = false;
                }
            }

            //Reset flags for which team is taking its turn - In a PVE encounter, either payer party or NPC party
            this._waitForNPC = false;
            this._waitForPlayer = false;
            /* **Reset flag so that next player can take their turn** */
            this._turnStarted = false;
        }
    }


}
