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
	void Start () 
    {
        if (NPCChoiceMgr._instance != null && NPCChoiceMgr._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            NPCChoiceMgr._instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public NPCCombatTurn ChooseCombatAction(CharMgrScript npc, List<CharMgrScript> possibleTargets)
    {
        
        //TODO: Implement logic for NPC combat action selection 

        int actChoiceProb = Random.Range(0, 100);
        int subSelectionProb = Random.Range(0, 100);

        GameConstants.ActionOptionIndices action = ChooseAction(npc,actChoiceProb);
        //Create list that will hold targets and their priority
        List<PrioritizedTarget> weightedTargets = new List<PrioritizedTarget>();

        //Add priorities to targets based on 
        foreach(
    }

    public GameConstants.ActionOptionIndices ChooseAction(CharMgrScript npc, int actionProb)
    {
        GameConstants.ActionOptionIndices selectedAction;

        switch (npc.stats.BehaviorType)
        {
            case(GameConstants.EnemyCombatBehaviorType.Standard):
                /*Standard enemies select between attack and abilities when health is greather than 
                half of the max. When health is under half of the max, may use a healing item or ability*/
                if (npc.stats.HP > npc.stats.MaxHP)
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
                        
                    }
                    else if (actionProb < probFavored + probNeutral + probDisliked)
                    {
                        
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

    public CharAbility ChooseAbility()
    {
        CharAbility selectedAbility;

        return selectedAbility;
    }
}

public struct PrioritizedTarget
{
    public int Priority;
    public CharMgrScript target;

    public PrioritizedTarget(int p, CharMgrScript t)
    {
        this.Priority = p;
        this.target = t;
    }

}

public struct NPCCombatTurn
{
    public List<CharMgrScript> SelectedTargets;
    public CharAbility ability;

    public NPCCombatTurn(List<CharMgrScript> targets, CharAbility cAbility)
    {
        this.SelectedTargets = targets;
        this.ability = cAbility;

    }
}


