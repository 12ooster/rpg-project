using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class NPCChoiceMgr : MonoBehaviour {

    public static NPCChoiceMgr _instance;

    //TODO:Implement probability numbers for action choices based on behavior and targeting type


    public int probFavored = 45;
    public int probNeutral = 25;
    public int probDisliked = 20;
    public int probStrongDis = 10;

	// Use this for initialization
	void Start () {
        
        if ( NPCChoiceMgr._instance != null && NPCChoiceMgr._instance != this ){
            
            Destroy( this.gameObject );
        }else{
            NPCChoiceMgr._instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public NPCCombatTurn ChooseCombatAction(CharMgrScript npc, List<CharMgrScript> possibleTargets){

        NPCCombatTurn thisTurn = new NPCCombatTurn();
        //TODO: Implement logic for NPC combat action selection 

        int actChoiceProb = Random.Range(0, 100);
        int subSelectionProb = Random.Range(0, 100);

        //Choose action based on the NPC's internal logic
        GameConstants.ActionOptionIndices selectedAction = this.ChooseAction(npc,actChoiceProb);
        //Create list that will hold targets and their priority
        List<PrioritizedTarget> weightedTargets = new List<PrioritizedTarget>();
        List<CharMgrScript> randomTargets = new List<CharMgrScript>();
        CharAbility turnAbility = new CharAbility();
        /*Generate list of all targets based on a conditional statement based off the chosen action by the NPC*/
        switch (selectedAction){
            case(GameConstants.ActionOptionIndices.Ability):

                break;
            case(GameConstants.ActionOptionIndices.Attack):
                //Get reference to the npc's attack
                turnAbility = npc.GetBasicAttack();

                //Get list of targets that the attack effects
                randomTargets = this.GetNPCTargets( turnAbility, CombatMgr._instance.PlayerParty);
                break;
            default:
                Debug.Log("Selected action was" + selectedAction.ToString() +" and NPC Turn Switch Fell through - Performing default action");
                break;
        }
       
        //Add priorities to targets from above selection and put them into a list TODO: prioitize via function
        foreach( CharMgrScript target in randomTargets){
            weightedTargets.Add( new PrioritizedTarget( 100, target));
        }
        //TODO perform some function to prioritize list of targets and grab the preffered one
        List<CharMgrScript> turnTargets = new List<CharMgrScript>();
        foreach (PrioritizedTarget t in weightedTargets) {
            turnTargets.Add(t.target);
        }
        //Set the fields of the created turn and return it
        thisTurn.SelectedAbility = turnAbility;
        thisTurn.SelectedTargets = turnTargets;
        thisTurn.SelectedAction = selectedAction;
        //return the created turn data
        return thisTurn;
    }

    public GameConstants.ActionOptionIndices ChooseAction( CharMgrScript npc, int actionProb ){

        GameConstants.ActionOptionIndices selectedAction = GameConstants.ActionOptionIndices.Default;

        switch (npc.stats.BehaviorType)
        {
            case(GameConstants.EnemyCombatBehaviorType.Standard):
                /*Standard enemies select between attack and abilities when health is greather than half of the max. When health is under half of the max, may use a healing item or ability
                * For the time being, 'standard' units just attack to keep things simple
                */
                Debug.Log("Current turn NPC is:" + npc.stats.CharName + " and has " + npc.stats.HP + "out of " + npc.stats.MaxHP + " remaining.");
                if (npc.stats.HP > (npc.stats.MaxHP/2))
                {
                    //CharAbility abilitySelection = ;
                    if (actionProb < probFavored) //Select favored action
                    {
                       
                       selectedAction = GameConstants.ActionOptionIndices.Attack;
                       
                       //Test DEBUG OUTPUT
                       if (TestScript._instance.TestMode)
                       {
                            Debug.Log("**Random number is:"+actionProb+"**Action selected:" + selectedAction.ToString());
                       }
                    }
                    else if (actionProb < probFavored + probNeutral)
                    {
                        selectedAction = GameConstants.ActionOptionIndices.Attack;
                    }
                    else if (actionProb < probFavored + probNeutral + probDisliked)
                    {
                        selectedAction = GameConstants.ActionOptionIndices.Attack;
                    }
                }
                else
                {
                    //CharAbility abilitySelection = ;
                    if (actionProb < probFavored) //Select favored action
                    {

                    }
                    else if (actionProb < probFavored + probNeutral)
                    {

                    }
                    else if (actionProb < probFavored + probNeutral + probDisliked)
                    {

                    }
                }
                break;
            case(GameConstants.EnemyCombatBehaviorType.Offensive):

                break;
            case(GameConstants.EnemyCombatBehaviorType.Defensive):

                break;
            case(GameConstants.EnemyCombatBehaviorType.Healer):

                break;
            default:
                //No conditions met and case fell through
                break;
        }

        return selectedAction;

    }

    //Is this being used? TODO
    public CharAbility ChooseAbility(){

        CharAbility selectedAbility = new CharAbility();

        return selectedAbility;
    }

    #region Utility Functions

    public List<CharMgrScript> GetNPCTargets( CharAbility currAbility, List<CharMgrScript> possibleTargets ){
        //populate the values from the list into an array so that there aren't unintended side effects
        List<CharMgrScript> pTargets = new List<CharMgrScript>();
        for (int i = 0; i < possibleTargets.Count; i++) {
            pTargets.Add(possibleTargets[i]);
        }
        //Instantiate a base number of targets to use 
        int numTargets = 0;

        //Determine the number of targets based off the given ability's targeting type
        switch (currAbility.TType){
            case(GameConstants.TargetType.SingleEnemy):
                numTargets = 1;
                break;
            case(GameConstants.TargetType.EnemyParty):
                numTargets = pTargets.Count;
                break;
            default:
                break;
        }

        //List to hold the targets selected
        List<CharMgrScript> selectedTargets = new List<CharMgrScript>();

        for (int i = 0; i < numTargets; i++){
            //Generate a random index to grab the associated unit
            int selectedIndex = Random.Range(0, pTargets.Count);
            //Add the unit associated with the randomly selected index
            selectedTargets.Add(pTargets[selectedIndex]);
            //remove the selected target from the list of possible targets to avoid overlapping selection
            pTargets.RemoveAt( selectedIndex );
        }

        //Return the list of generated targets
        return selectedTargets;
    }

    #endregion
}

public class PrioritizedTarget{

    public int Priority;
    public CharMgrScript target;

    public PrioritizedTarget(int p, CharMgrScript t)
    {
        this.Priority = p;
        this.target = t;
    }

}

public class NPCCombatTurn{

    public List<CharMgrScript> SelectedTargets;
    public CharAbility SelectedAbility;
    public GameConstants.ActionOptionIndices SelectedAction;

    public NPCCombatTurn( GameConstants.ActionOptionIndices sAction, List<CharMgrScript> targets, CharAbility cAbility ){
        this.SelectedAction = sAction;
        this.SelectedTargets = targets;
        this.SelectedAbility = cAbility;
    }

    public NPCCombatTurn() {}
}


