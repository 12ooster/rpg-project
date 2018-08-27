using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlayerStateManager : MonoBehaviour {

    //TODO: PLAYER PARTY NEEDS TO BE CONVERTED INTO DATA OBJECTS AND STORED ON START

    [SerializeField]
    public Inventory PlayerInventory;

    public static PlayerStateManager Instance;

    private WorldItemScript _collectableItem;

    public WorldItemScript CollectableItem
    {
        get
        {
            return this._collectableItem;
        }
        set
        {
            this._collectableItem = value;
        }
    }

    private CharStatsData _playerStats;

    public CharStatsData PlayerStats
    {
        get
        {
            return this._playerStats;
        }
        set
        {
            this._playerStats = value;
        }
    }

    /// <summary>
    /// The player party data.
    /// </summary>
    [SerializeField]
    public List<CharStatsData> PlayerParty;

    void Awake()
    {
        /*Implement PlayerState manager as a singleton to allow global access and ensure only 1 exists*/
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Initialize necessary variables
            Init();
        }
    }

	// Use this for initialization
	void Start () 
    {
        //Initialize player inventory
        PlayerInventory = new Inventory();
        //Initialize player party
        if (PlayerParty == null)
        {
            PlayerParty = new List<CharStatsData>();
        }
        if (this._playerStats == null)
        {
            this.GetComponent<CharMgrScript>().Init();
            this._playerStats = this.GetComponent<CharMgrScript>().stats.StatsAsData();
        }
        //Add player to player party if not already in list
        if (!PlayerParty.Contains(this._playerStats))
        {
            PlayerParty.Add(this._playerStats);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}


    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public void Init()
    {
        if (this.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("Sprite renderer on player object not found!");
        }
        else
        {
            _playerSprite = this.GetComponent<SpriteRenderer>();
        }
        //Initialize necessary variables for player party, etc.
        if (this._playerStats == null)
        {
            this.GetComponent<CharMgrScript>().Init();
            this._playerStats = this.GetComponent<CharMgrScript>().stats.StatsAsData();
        }
        //Add player to player party if not already in list
        if (!PlayerParty.Contains(this._playerStats))
        {
            PlayerParty.Add(this._playerStats);
        }
    }

    /// <summary>
    /// Adds a item from the game world to the inventory. 
    /// Converts the item from "worldItem" to "inventoryItem" in the process.
    /// </summary>
    /// <param name="item">A World Item script instance.</param>
    public void AddToInventory(WorldItemScript item, bool fromDialogue)
    {
        if (item != null)
        {
            //If item being added from dialogue then eschew deleting world ui elements etc
            if (fromDialogue)
            {
                PlayerInventory.AddItem(item.thisItem); 
            }
            else
            {
                //TODO:Figure out how to make a deep copy of item to add
                PlayerInventory.AddItem(item.thisItem);
                //Remove the collected item from the game world
                item.GetCollected();
            }
        }
        else
        {
            Debug.LogError("NULL item passed to AddToInventory!");
        }
    }

    /// <summary>
    /// Adds an item directly to the player's inventory.
    /// </summary>
    /// <param name="item">An Inventory Item script instance.</param>
    public void AddToInventory(InventoryItem item)
    {
        if (item != null)
        {
            //TODO:Figure out how to make a deep copy of item to add
            PlayerInventory.AddItem(item);
        }
        else
        {
            Debug.LogError("NULL item passed to AddToInventory!");
        }
    }

    public void RemoveFromInventory(InventoryItem item)
    {
        if (item != null)
        {
            PlayerInventory.RemoveItem(item);
        }
        else
        {
            Debug.LogError("NULL item passed to RemoveFromInventory!");
        }
    }

    public Vector3 Position
    {
        get
        {
            return this.gameObject.transform.position;
        }
    }

    public Vector3 LocalPosition
    {
        get
        {
            return this.gameObject.transform.localPosition;
        }
    }
        
    private SpriteRenderer _playerSprite;

    public SpriteRenderer PlayerSprite
    {
        get
        {
            if (this.GetComponent<SpriteRenderer>() == null)
            {
                Debug.LogError("Sprite renderer on player object not found!");
                return new SpriteRenderer();
            }
            else
            {
                return this._playerSprite;
            }
        }
        set
        {
            this._playerSprite = value;
        }
    }

    /// <summary>
    /// Changes the state of the sprite's visibility. If shown, then it will be not shown and vice versa.
    /// </summary>
    public void ChangeSpriteVisibleState()
    {
        if (this.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("Sprite renderer on player object could not be hidden because it was not found!");
        }
        else
        {
            _playerSprite.enabled = !_playerSprite.enabled;
        }
    }

    /// <summary>
    /// Adds a given character to the party
    /// </summary>
    /// <returns><c>true</c>, if character was successfully added, <c>false</c> otherwise.</returns>
    /// <param name="charScript">Char script.</param>
    public bool AddCharacterToParty(CharMgrScript charScript)
    {
        if (PlayerParty.Count == GameConstants.MAX_PARTY_SIZE)
        {
            //party full
            //TODO: Add 'Party Full' failure use case
            return false;
        }
        else
        {
            //If party size is less then max then we add character to party
            //TODO:Is checking for duplicates necessary?
            PlayerParty.Add(charScript.stats.StatsAsData());
            return true;
        }
            
    }
}
