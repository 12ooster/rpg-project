using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class GameStateMgr : MonoBehaviour {

    /// <summary>
    /// The static singleton instance of this object.
    /// </summary>
    public static GameStateMgr _instance;
	
    /// <summary>
    /// The current game state for determining types of possible interaction
    /// </summary>
    public GameConstants.GameState CurrentState;

    public GameObject EmptyCharacter;

    /*Combat Variables*/

    /// <summary>
    /// The enemy party data.
    /// </summary>
    [SerializeField]
    public List<CharStatsData> EnemyPartyData;

    [SerializeField]
    public Vector3
    DistCharsCombat;
    /// <summary>
    /// The starting position for calculating party member positions in combat.
    /// </summary>
    public Transform CombatPartyStartPos;
   /// <summary>
   /// The starting position for calculating enemy positions in combat.
   /// </summary>
    public Transform CombatEnemyStartPos;


    // Use this for initialization
	void Start () 
    {
	    //Make this class a singleton
        if (GameStateMgr._instance != null && GameStateMgr._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            GameStateMgr._instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Handle the rest of initialization of this object.
            Init();
        }
	}

    public void Init()
    {
        //Handle any necessary initialization in here  
    }

	// Update is called once per frame
	void Update () 
    {
		
	}

    public void LoadScene(GameConstants.SceneIndices Scene)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)Scene)
        {
            Debug.LogWarning("Game is trying to reload open scene.");
        }
        else
        {
            SceneManager.LoadScene((int)Scene);
        }
    }

    public void StartLoadingOverworld()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)GameConstants.SceneIndices.GameScene)
        {
            Debug.LogWarning("Game is already in overworld but StartLoadingOverworld() Called.");
        }
        else
        {
            //Start loading combat scene
            StartCoroutine(LoadOverworldAsync());
        }
    }

    IEnumerator LoadOverworldAsync()
    {
        //Load the scene in the background at the same time as the current scene to avoid choppiness
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)GameConstants.SceneIndices.GameScene);

        //Wait here until  the load operation finishes
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Change game state to combat
        ChangeGameState(GameConstants.GameState.Overworld);
    }

    public void StartLoadingScene(Scene scene)
    {
        //Start loading combat scene
        StartCoroutine(LoadSceneAsyncRoutine(scene));
    }

    IEnumerator LoadSceneAsyncRoutine(Scene scene)
    {
        //Load the scene in the background at the same time as the current scene to avoid choppiness
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.name);

        //Wait here until  the load operation finishes
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Change game state to the appropiate state
        ChangeGameState(GameConstants.GameState.Overworld);
    }

    public void StartLoadingCombat(List<CharMgrScript> EnemyParty)
    {
        //Check to make sure combat scene isn't already loaded
        if (SceneManager.GetActiveScene().buildIndex == (int)GameConstants.SceneIndices.CombatScene)
        {
            Debug.LogWarning("Game is already in combat but StartLoadingCombat() Called.");
        }
        else
        {
            //Clear enemy party of old data if necessary before loading new enemies
            if (this.EnemyPartyData.Count > 0)
            {
               this.EnemyPartyData.Clear();
            }
           
            //Get the data for the new enemy's party
            foreach (CharMgrScript enemyScript in EnemyParty)
            {
                CharStatsData enemyData = new CharStatsData();
                enemyData = enemyScript.stats.StatsAsData();
                EnemyPartyData.Add(enemyData);
            }
            //Start loading combat scene
            StartCoroutine(LoadCombatAsync());
        }
    }

    IEnumerator LoadCombatAsync()
    {
        //Load the scene in the background at the same time as the current scene to avoid choppiness
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)GameConstants.SceneIndices.CombatScene);

        //Wait here until  the load operation finishes
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Change game state to combat
        ChangeGameState(GameConstants.GameState.Combat);

        //Give the combat manager the necessary data
        CombatMgr._instance.Init(this.CombatPartyStartPos,this.CombatEnemyStartPos,this.EnemyPartyData,EmptyCharacter);

        /*Handle start of combat*/
        //Get active scene for win return
        Scene winReturnScene = SceneManager.GetActiveScene();
        //Pass in return scenes and start combat
        CombatMgr._instance.StartCombat(winReturnScene, winReturnScene);
    }

   
    public void ChangeGameState(GameConstants.GameState newState)
    {
        switch (newState)
        {
            case(GameConstants.GameState.Overworld):
                //Change sprite state if needed
                if (GameStateMgr._instance.CurrentState == GameConstants.GameState.Combat)
                {
                    //hide the player sprite
                    PlayerStateManager.Instance.ChangeSpriteVisibleState();
                }
                //Update game state
                GameStateMgr._instance.CurrentState = GameConstants.GameState.Overworld;
                break;
            case(GameConstants.GameState.Combat):
                if (GameStateMgr._instance.CurrentState != GameConstants.GameState.Combat)
                {
                    //hide the player sprite
                    PlayerStateManager.Instance.ChangeSpriteVisibleState();
                }
                //Update game state
                GameStateMgr._instance.CurrentState = GameConstants.GameState.Combat;
                break;
            default:
                //No match, so exit case
                break;
        }
    }
}
