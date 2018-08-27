using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemScript : MonoBehaviour {

    public InventoryItem ThisItem;
    public Text UITextName;
    public Image ItemImage;
    public int PageIndex = 0;
    public Color StartingColor;
    public float SelectedAlpha =0.15f;
    public Text countText;
    /// <summary>
    /// Empty item prefab to be instantiated when this item is dropped, and populated with corresponding info
    /// </summary>
    public GameObject EmptyItem;

    void Awake()
    {
        //Get reference to empty item instance for instantiation purposes
        if (EmptyItem == null)
        {
            EmptyItem = (GameObject)Resources.Load("Prefabs/Collectible Items/CollectItemEmpty");
        }
        //Check for failed resource.load or asset not found
        if (EmptyItem == null)
        {
            Debug.LogError("Searched for resource not found! Called from" + this.name);
        }
        else //Empty Item prefab successfully loaded so populate fields with corresponding values
        {

        }
    }

    // Use this for initialization
	void Start () 
    {
        //Get color to be used for selection state
       // StartingColor = this.GetComponent<Image>().color;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    /// <summary>
    /// Initialize the UI elements and necessary variables of this instance
    /// </summary>
    public void Init()
    {
        //Get reference to text
        if (UITextName == null)
        {
            UITextName = this.transform.FindChild("ItemText").GetComponent<Text>();
        }
        if (ItemImage == null)
        {
            ItemImage = this.transform.FindChild("ItemImage").GetComponent<Image>();
        }
        //Get reference to count text in UI
        if (countText == null)
        {
            countText = transform.FindChild("Count").GetComponent<Text>();
        }

        //Get reference to empty item instance for instantiation purposes by loading from folder
        if (EmptyItem == null)
        {
            EmptyItem = (GameObject)Resources.Load("Prefabs/Collectible Items/CollectItemEmpty");
        }
        //Check for failed resource.load or asset not found
        if (EmptyItem == null)
        {
            Debug.LogError("Searched for resource not found! Called from" + this.name);
        }
        else //Empty Item prefab successfully loaded so populate fields with corresponding values
        {

        }

        //Turn off the UI for the count if player has only 1  of this item
        if (this.ThisItem.Count < 2)
        {
            countText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the display elements.
    /// </summary>
    /// <param name="item">The item to which this object corresponds.</param>
    public void SetDisplayElements(InventoryItem item)
    {
        ThisItem = item;
        UITextName.text = item.ItemName;
        ItemImage.sprite = item.ItemImage;
        ItemImage.color = item.ItemColor;
        //Get color to be used for selection state
        StartingColor = this.GetComponent<Image>().color;
        //item count
        if (ThisItem.Count > 1)
        {
            countText.gameObject.SetActive(true);
            countText.text = "x" + this.ThisItem.Count.ToString();
        }
       
    }

    /// <summary>
    /// Sets the item as selected in player's inventory and updates UI to show this.
    /// </summary>
     public void SetSelected(bool selected)
    {
        if (selected)
        {
            this.GetComponent<Image>().color = new Color(StartingColor.r, StartingColor.g, StartingColor.b, SelectedAlpha);
        }
        else
        {
            this.GetComponent<Image>().color = StartingColor;
        }
    }

    /// <summary>
    /// Drop this item to the ground from the player's inventory.
    /// Creates a corresponding world item instance near the player's position and 
    /// removes the corresponding instance from the player's inventory.
    /// </summary>
    public void Drop()
    {
        //Creates a corresponding world item instance near the player's position
        GameObject droppedItem = (GameObject)GameObject.Instantiate(EmptyItem,PlayerStateManager.Instance.transform.position,EmptyItem.transform.rotation);
        //Get reference to the dropped item's script
        WorldItemScript itemScript = droppedItem.GetComponent<WorldItemScript>();
        //Initialize fields for dropped item
        itemScript.InitItem(
            ThisItem.ID,
            ThisItem.ItemImage,
            ThisItem.ItemName,
            ThisItem.PickupText,
            ThisItem.PickupText,
            ThisItem.ItemScale,
            ThisItem.ItemColor
           );

        //Trigger menu to shift or close

        //Offset the item from the player's position so they don't overlap
        Sprite iSprite = PlayerController._instance.gameObject.GetComponent<SpriteRenderer>().sprite;
        droppedItem.transform.position = 
            new Vector3(droppedItem.transform.position.x + (iSprite.bounds.extents.x * 2.0f),
                        droppedItem.transform.position.y);
       
        //Remove item from inventory
        PlayerStateManager.Instance.RemoveFromInventory(this.ThisItem);
    }

}
