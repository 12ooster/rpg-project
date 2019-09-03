using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

/// <summary>
/// This object manages data structures holding all the dialog for a character in the game.
/// Serves dialog text to the UI manager upon request.
/// </summary>
[System.Serializable]
public class CharacterDialogMgr : MonoBehaviour {


    /// <summary>
    /// The ID of this speaker
    /// </summary>
    public int SpeakerID = -1;

    /// <summary>
    /// Nested dictionary holding all the dialogue for characters spoken when walking around the world
    /// </summary>
    public Dictionary<GameConstants.InteractionType, List<DialogueChain>> WorldDialogue;

    /// <summary>
    /// Nested dictionary holding all the dialogue for characters spoken during events
    /// </summary>
    public Dictionary<GameConstants.InteractionType, List<DialogueChain>> EventDialogue;

    /// <summary>
    /// Array holding character's dialogue to be read into dictionary's at runtime
    /// </summary>
    [SerializeField]
    public DialogueContainers[] CharWorldDialogueContainers;

    /// <summary>
    /// Array holding character's dialogue to be read into dictionary's at runtime
    /// </summary>
    [SerializeField]
    public DialogueContainers[] CharEventDialogueContainers;

    public List<int> numbers;

    public Sprite charSprite;

    public GameObject[] dialogueItemPrefabs;

    [SerializeField]
    public WorldItemScript[] dialogueEventItems;

    void Awake()
    {
        //Initialize data structure variables
        WorldDialogue = new Dictionary<GameConstants.InteractionType, List<DialogueChain>>();
        EventDialogue = new Dictionary<GameConstants.InteractionType, List<DialogueChain>>();

        //If there are items in the arrays then cycle through and add each list of dialog to the dictionary using their interaction type as the key
        if (CharWorldDialogueContainers != null && CharWorldDialogueContainers.Length > 0)
        {
            for (int i = 0; i < CharWorldDialogueContainers.Length; i++)
            {
                WorldDialogue.Add(CharWorldDialogueContainers[i].InteractionType,CharWorldDialogueContainers[i].dialogueChains);
            }
        }

        //If there are dialogue event prefabs then add those to the array
        if (dialogueItemPrefabs != null && dialogueItemPrefabs.Length > 0)
        {
            for (int i = 0; i < dialogueItemPrefabs.Length; i++)
            {
                dialogueEventItems[i] = dialogueItemPrefabs[i].GetComponent<WorldItemScript>();
            }
        }
    }

    // Use this for initialization
	void Start () 
    {
        //get reference to the character's sprite
        charSprite = this.GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void CreateDialog()
    {
        
        Debug.Log("Creating DIALOGUE!!!");
        //Get dialogType corresponding to current story / world state from game state manager
        GameConstants.InteractionType iType = GameConstants.InteractionType.Normal;

        //Get random dialog of the current type
        DialogueChain currChain;

        //Get random number to use as index
        int rKey = Random.Range(0, WorldDialogue[iType].Count);

        //Get dialog using the randomly generated key
        currChain = WorldDialogue[iType][rKey];

        //TODO: Handle Displaying the dialogue in UI
        SpeechUIManager._instance.StartDialogue(this.transform,currChain,new Vector2(charSprite.rect.width,charSprite.rect.height),this);

//        for (int i = 0; i < currChain.SpeechText.Count; i++)
//        {
//            for(int j = 0; j < currChain.SpeechText[i].Text.Length;j++)
//            {
//                Debug.Log(currChain.SpeechText[i].Text[j]);
//            }
//        }

    }
}
