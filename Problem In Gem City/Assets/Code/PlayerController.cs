using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlayerController : MonoBehaviour 
{
	float _moveSpeed = 1.15f;

	/*Animation Strings*/
	string _walkUp = "WalkUp";
	string _walkRight = "WalkRight";
	string _walkLeft = "WalkLeft";
	string _walkDown = "WalkDown";

	//public enum AnimDir {Up,Right,Left,Down};

    SpeechUIManager _speechMgr;

    public CharMgrScript NearbyChar;

    List<string> _speechTest = new List<string>();

    Sprite charSprite;

    public bool InDialogue
    {
        get
        {
            return this._inDialogue;
        }
        set
        {
            this._inDialogue = value;
        }
    }

    bool _inDialogue = false;

    public static PlayerController _instance;



    void Awake()
    {
        if (PlayerController._instance != null && PlayerController._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PlayerController._instance = this;
        }
    }

	// Use this for initialization
	void Start () 
	{
        _speechMgr = GameObject.FindGameObjectWithTag("SpeechMgr").GetComponent<SpeechUIManager>();
        _speechTest.Add("Hello Jeremiah...");
        _speechTest.Add("welcome to Cybon's world.");
        _speechTest.Add("It's really fun here. I can't wait to see you when you wake up. It will be great:)");
        _speechTest.Add("I will never forget you because you are a tiny teeny angel bibby boy. You must remember that.");


        //get reference to the character's sprite
        charSprite = this.GetComponent<SpriteRenderer>().sprite;
 
	}
	
	// Update is called once per frame
	void Update () 
	{
        
	}

    public void Move(Vector3 dir,GameConstants.AnimDir animDir)
	{
        //Turn off root motion in animator
        //this.GetComponent<Animator>().applyRootMotion = false;
        //Handle animation  
        AnimateWalk(animDir);
		Vector3 currPos = this.gameObject.transform.position;
		Vector3 newPos = currPos + dir * this._moveSpeed * Time.deltaTime;
		this.gameObject.transform.position = newPos;
	}

	public void AnimateWalk(GameConstants.AnimDir dir)
	{
		switch (dir)
		{
			case(GameConstants.AnimDir.Up):
				this.GetComponent<Animator>().SetBool(_walkUp, true);
				//set other flags off
				this.GetComponent<Animator>().SetBool(_walkRight, false);
				this.GetComponent<Animator>().SetBool(_walkLeft, false);
				this.GetComponent<Animator>().SetBool(_walkDown, false);
				break;
			case(GameConstants.AnimDir.Right):
				this.GetComponent<Animator>().SetBool(_walkRight, true);
				//set other flags off
				this.GetComponent<Animator>().SetBool(_walkUp, false);
				this.GetComponent<Animator>().SetBool(_walkLeft, false);
				this.GetComponent<Animator>().SetBool(_walkDown, false);
				break;
			case(GameConstants.AnimDir.Left):
				this.GetComponent<Animator>().SetBool(_walkLeft, true);
				//set other flags off
				this.GetComponent<Animator>().SetBool(_walkUp, false);
				this.GetComponent<Animator>().SetBool(_walkRight, false);
				this.GetComponent<Animator>().SetBool(_walkDown, false);
				break;
			case(GameConstants.AnimDir.Down):
				this.GetComponent<Animator>().SetBool(_walkDown, true);
				//set other flags off
				this.GetComponent<Animator>().SetBool(_walkUp, false);
				this.GetComponent<Animator>().SetBool(_walkRight, false);
				this.GetComponent<Animator>().SetBool(_walkLeft, false);
				break;
			default:
				//empty case
				break;
		}

	}

    public void StopAnimation(GameConstants.AnimDir dir)
    {
        switch (dir)
        {
            case(GameConstants.AnimDir.Up):
                this.GetComponent<Animator>().SetBool(_walkUp, false);
                break;
            case(GameConstants.AnimDir.Right):
                this.GetComponent<Animator>().SetBool(_walkRight, false);
                break;
            case(GameConstants.AnimDir.Left):
                this.GetComponent<Animator>().SetBool(_walkLeft, false);
                break;
            case(GameConstants.AnimDir.Down):
                this.GetComponent<Animator>().SetBool(_walkDown, false);
                break;
            default:
                //empty case
                break;
        }
        //Turn on root motion capability
        //this.GetComponent<Animator>().applyRootMotion=true;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Check to see if collider belongs to interactive character
        if (other.tag == "InteractiveChar")
        {
            //Player is in range of a interactive character. Set script reference for use.
            this.NearbyChar = other.GetComponent<CharMgrScript>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Check to see if collider belongs to interactive character
        if (other.tag == "InteractiveChar")
        {
            this.NearbyChar = null;
        }
    }

    public void EndDialogue()
    {
        this._inDialogue = false;
        //Update game state manager
        GameStateMgr._instance.CurrentState = GameConstants.GameState.Overworld;
    }
}
