using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class CharMgrScript : MonoBehaviour {

	/// <summary>
    /// Data structure holding the character's dialogue.
    /// </summary>
    public CharacterDialogMgr dialogMgr;
    /// <summary>
    /// The start position of this character. Can be used for initial placement in cut scenes
    /// </summary>
    public Vector3 StartPos;
    /// <summary>
    /// Array of positions the character cna move to. Can be used for movements during cutscene in which index is scenestep.
    /// </summary>
    public Vector3[] MovementPos;
    /// <summary>
    /// The character's stats.
    /// </summary>
    public CharStatsScript stats;

    /// <summary>
    /// The character's combat abilities.
    /// </summary>
    //public CharAbility[] abilities;

    /// <summary>
    /// Flag used for combat. Indicates if the character has acted.
    /// </summary>
    public bool ActedThisRound = false;

    /// <summary>
    /// UI object used as highlight arrow during combat
    /// </summary>
    public GameObject HighlightArrow;

    /// <summary>
    /// The UI txt used for displaying character's hp vs. max hp in combat
    /// </summary>
    public GameObject HPUITxt;

    //TODO: add turn on for this UI element when it's the character's turn
    public GameObject UITurnEmphasisElem;

    // Use this for initialization
	void Start(){
        if (dialogMgr == null){
            dialogMgr = this.GetComponent<CharacterDialogMgr>();
        }
        if (stats == null){
            Debug.Log("Stats NULL! GETTING COMPONENT");
            stats = this.GetComponent<CharStatsScript>();
        }
        if(stats.charSprite ==null){
            //get reference to the character's sprite
            stats.charSprite = this.GetComponent<SpriteRenderer>().sprite;
        }
	}
	
    public void Init(){
        if (stats == null){
            Debug.Log("Stats NULL! GETTING COMPONENT");
            stats = this.GetComponent<CharStatsScript>();
        }
        if(stats.charSprite ==null){
            //get reference to the character's sprite
            stats.charSprite = this.GetComponent<SpriteRenderer>().sprite;
        }
    }

	// Update is called once per frame
	void Update () 
    {
		
	}

    public void StartDialog(){
        Debug.Log("---Start Dialog CALLED!!!---");
        dialogMgr.CreateDialog();
    }

    /// <summary>
    /// Gets the basic attack of this character.
    /// </summary>
    /// <returns>The basic attack.</returns>
    public CharAbility GetBasicAttack(){
        CharAbility attack = new CharAbility();
        //Iterate through the character's abilities and find the ability with type => basic attack
        foreach (CharAbility a in this.stats.CombatAbilities){
            if (a.aType == GameConstants.AbilityType.BasicAttack){
                attack = a;
                return a;
            }
        }

        Debug.LogWarning("No attack found!");
        return attack;
    }

    public void HighlightArrowStatus(bool enableOn){
        this.HighlightArrow.SetActive(enableOn);
    }

    public void HPTextStatus(bool enableOn){
        this.HPUITxt.SetActive(enableOn);
    }

    public void UITurnEmphasisStatus(bool enableOn){
        this.UITurnEmphasisElem.SetActive(enableOn);
    }
   
    public string HPText{
        get{
            return this.HPUITxt.GetComponent<Text>().text;
        }
        set{
            this.HPUITxt.GetComponent<Text>().text = value;
        }
    }

    public void EnterDownedState(){
        //Set state to downed
        this.stats.Status = GameConstants.StatusType.Downed;
        //Handle downed animation as necessary
        StartCoroutine("DownedAnimation");
    }

    IEnumerator DownedAnimation(){
        //Get the default color
        Color StartingColor = this.gameObject.GetComponent<SpriteRenderer>().color;

        //Set up the ending color that's faded out
        Color EndingColor = new Color(StartingColor.r, StartingColor.g, StartingColor.b, 0);

        //Timer
        float rate = 1.15f;
        float step = 0.0f;

        //Lerp the color from basic to transparent
        while (step < 1){
            Color currColor = Color.Lerp(StartingColor, EndingColor, step);
            this.gameObject.GetComponent<SpriteRenderer>().color = currColor;
            step += Time.deltaTime * rate;
            yield return null;
        }

//        if (TestScript._instance.TestMode)
//        {
//            Debug.Log("CHARACTER sprite faded out because it was downed!");
//        }
            
    }
}
