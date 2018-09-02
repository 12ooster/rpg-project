using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class InputMgr : MonoBehaviour {


	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update ()
    {
        //debug player speech bubble
        //        if (Input.GetKeyDown(KeyCode.T))
        //        {
        //            _inDialogue = true;
        //            _speechMgr.StartDialogue(this.gameObject.transform,_speechTest,new Vector2(charSprite.rect.width,charSprite.rect.height));
        //        }
        /*If the player is in a dialouge check for confirmation key when necessary*/
        if (GameStateMgr._instance.CurrentState == GameConstants.GameState.Dialogue)
        {
            if (SpeechUIManager._instance.WaitForPlayer)
            {
                if (SpeechUIManager._instance.DialogueChoice)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        Debug.Log("In player controller: Calling speechMgr response set selected + 1!!");
                        SpeechUIManager._instance.currResponseScript.SetSelectedAction(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        Debug.Log("In player controller: Calling speechMgr response set selected - 1!!");
                        SpeechUIManager._instance.currResponseScript.SetSelectedAction(-1); 
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SpeechUIManager._instance.ContinueDialog(SpeechUIManager._instance.currResponseScript.SelectedAction());
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SpeechUIManager._instance.ContinueDialog();
                    }  
                }

            }
        }/*Adjust input handling for the combat state*/
        else if (GameStateMgr._instance.CurrentState == GameConstants.GameState.Combat)
        {
            //Let the combatMgr finish initializing before taking other combat related actions
            if (!CombatMgr._instance.Initializing)
            {
                //Make sure no combat action/animation is currently resolving before checking for input
                if (!CombatMgr._instance.ResolvingAction)
                {
                    if (CombatMgr._instance.TurnStarted)
                    {
                        //Handle input for changing selection and selecting combat action
//                        if (TestScript._instance.TestMode)
//                        {
//                            Debug.LogWarning("~~~~Combat Mgr Turn Started and waiting for player ~~~~")
//                        }

                        /*Input handling for highlighting action selection and combat animation action*/
                        if (CombatMgr._instance.WaitForPlayer)
                        {
                            CombatUIManager._instance.HandleInput(GetKeyCode());
                            Debug.Log( CombatMgr._instance.currentTurnChar.stats.CharName+"'s Turn! Waiting for player input in Inputmanager!!");
                        }
                        else if(CombatMgr._instance.WaitForNPC)
                        {

                            //TODO: or wait for Handle NPC turn to end
                            Debug.Log( CombatMgr._instance.currentTurnChar.stats.CharName+"'s Turn! Waiting for NPC INPUT in Inputmanager!!");
                        }

                    }
                    else if (!CombatMgr._instance.TurnStarted)//Otherwise if no character has had their turn started, start a turn
                    {
                        if (TestScript._instance.TestMode)
                        {
                            Debug.Log("--New turn started in input manager!!--");
                        }
                        CombatMgr._instance.TurnStarted = true;
                        CombatMgr._instance.StartNextTurn();
                    }
                   
                }

            }
        }
        else if (GameStateMgr._instance.CurrentState == GameConstants.GameState.Overworld)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //Move sprite and animate
                PlayerController._instance.Move(Vector3.up,GameConstants.AnimDir.Up);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                PlayerController._instance.Move(Vector3.right,GameConstants.AnimDir.Right);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if(TestScript._instance.TestMode)
                {
                    Debug.LogWarning("####### Down Arrow Pressed inside overworld check");
                }
                PlayerController._instance.Move(Vector3.down,GameConstants.AnimDir.Down);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                PlayerController._instance.Move(Vector3.left,GameConstants.AnimDir.Left);
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                PlayerController._instance.StopAnimation(GameConstants.AnimDir.Up);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                //Stop walk animation
                PlayerController._instance.StopAnimation(GameConstants.AnimDir.Right);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                //Stop walk animation
                PlayerController._instance.StopAnimation(GameConstants.AnimDir.Down);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                //Stop walk animation
                PlayerController._instance.StopAnimation(GameConstants.AnimDir.Left);
            }

            /*If player is in range of a collectable item, and presses the collect key then add it to their inventory*/
            if (PlayerStateManager.Instance.CollectableItem != null && Input.GetKeyDown(KeyCode.Space))
            {
                PlayerStateManager.Instance.AddToInventory(PlayerStateManager.Instance.CollectableItem,false);
            }
            else if (PlayerController._instance.NearbyChar != null && Input.GetKeyDown(KeyCode.Space))
            {
                GameStateMgr._instance.ChangeGameState(GameConstants.GameState.Dialogue);
                //Update game state manager
                GameStateMgr._instance.CurrentState = GameConstants.GameState.Dialogue;
                PlayerController._instance.NearbyChar.StartDialog();
            }

            /*Input for pause menu*/
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (MenuUIManager.MenuUIInstance.MenuOpen)
                {
                    MenuUIManager.MenuUIInstance.CloseMenu();
                }
                else
                {
                    MenuUIManager.MenuUIInstance.OpenMenu();
                }
            }
        }
	}

    /// <summary>
    /// Returns the key code of the currently pressed key.
    /// </summary>
    /// <returns>The key code.</returns>
    public KeyCode GetKeyCode()
    {
        KeyCode keyPressed = new KeyCode();


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            keyPressed = KeyCode.RightArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keyPressed = KeyCode.LeftArrow;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            keyPressed = KeyCode.UpArrow;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            keyPressed = KeyCode.DownArrow;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            keyPressed = KeyCode.Space;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            keyPressed = KeyCode.Escape;
        }

        return keyPressed;
    }
}
