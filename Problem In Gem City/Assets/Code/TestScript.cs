using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class TestScript : MonoBehaviour {

    public List<WorldItemScript> InventoryPopulateItems;
    public static TestScript _instance;
    public bool TestMode = true;
    /// <summary>
    /// The enemy party data.
    /// </summary>
    [SerializeField]
    public List<CharMgrScript> TestEnemyParty;

    /// <summary>
    /// The characters used to test population of the player's party
    /// </summary>
    [SerializeField]
    public List<CharMgrScript> PlayerPartyCharacters;

	// Use this for initialization
	void Start () 
    {
        if (TestScript._instance != null && TestScript._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            TestScript._instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*Press 1 to populate inventory from list for testing purposes*/
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestPopulateInventory();
        }
        /*Press 2 to load the battle screen for testing*/
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestLoadBattleScene();
        }
        /*Press 3 to load the game scene again for testing*/
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestLoadGameScene();
        }
        /*Press 4 to test populating the player's party from debug object's list*/
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestPopulatePlayerParty();    
        }
        /*Press 5 to test combat resolution*/
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            TestCombatEnd();
        }
	}

    public void TestPopulateInventory()
    {
        /*Add items to inventory to populate it for testing purposes*/
        foreach (WorldItemScript s in InventoryPopulateItems)
        {
            PlayerStateManager.Instance.CollectableItem = s;
            PlayerStateManager.Instance.AddToInventory(PlayerStateManager.Instance.CollectableItem,false);
        }

    }

    /// <summary>
    /// Tests population of player's party with test list of characters
    /// </summary>
    public void TestPopulatePlayerParty()
    {
        Debug.Log("Populate Player Party Called.");

        //Iterate through list of character scripts for test, convert to data, add to Player party
        foreach (CharMgrScript character in PlayerPartyCharacters)
        {
            PlayerStateManager.Instance.AddCharacterToParty(character);
        }
    }

    public void TestLoadBattleScene()
    {
        Debug.Log((int)GameConstants.SceneIndices.CombatScene);
        GameStateMgr._instance.StartLoadingCombat(TestEnemyParty);
    }

    public void TestLoadGameScene()
    {
        Debug.Log((int)GameConstants.SceneIndices.GameScene);
        GameStateMgr._instance.StartLoadingOverworld();
    }

    /// <summary>
    /// Tests the resolution of combat.
    /// </summary>
    public void TestCombatEnd()
    {
        Debug.Log("TestCombatEnd() Called!");
        if (CombatMgr._instance != null)
        {
            //This test case assumes Party one was the victor so use their scene as the return point
            CombatEndData d = new CombatEndData(GameConstants.CombatEndType.Party1Win,CombatMgr._instance.PartyOneReturnScene,CombatMgr._instance.FoundItems,5,"You won, hoo hoo rayyy!");
            CombatMgr._instance.EndCombat(d);
        }
        else
        {
            Debug.LogError("No combat manager found!");
        }
    }
}
