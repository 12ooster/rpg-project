using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class WorldItemScript : MonoBehaviour {

    [SerializeField]
    public InventoryItem thisItem;

    public bool PlayerInRange = false;
    public int ID = -1;

	// Use this for initialization
	void Start () 
    {
        if (thisItem == null)
        {
            thisItem = new InventoryItem(
                ID,
                this.GetComponent<SpriteRenderer>().sprite,
                this.gameObject.name,
                "Looks like a" + this.gameObject.name + ".",
                "You got a " + thisItem.ItemName + ".",
                this.transform.localScale,
                this.GetComponent<SpriteRenderer>().color
            );
        }
        else
        {
            //Fail safe for set item id
            if (thisItem.ID <= 0)
            {
                thisItem.ID = ID;
            }
            //Fail safe to populate image for item
            if (this.thisItem.ItemImage == null)
            {
                this.thisItem.ItemImage = this.GetComponent<SpriteRenderer>().sprite;
            }
            //Fail safe to populate name for item
            if (this.thisItem.ItemName == null || this.thisItem.ItemName == "")
            {
                this.thisItem.ItemName = this.gameObject.name;
            }
            //Fail safe to populate pickup text
            if (this.thisItem.PickupText == null || this.thisItem.PickupText == "")
            {
                thisItem.PickupText = "You got a " + thisItem.ItemName + ".";
            }
            //Fail safe for item description
            if (this.thisItem.ItemDescription == null || this.thisItem.ItemDescription == "")
            {
                thisItem.ItemDescription = "Looks like " + thisItem.ItemDescription + ".";
            }
            //Fail safe for item color
            if (this.thisItem.ItemColor != this.GetComponent<SpriteRenderer>().color)
            {
                this.thisItem.ItemColor = this.GetComponent<SpriteRenderer>().color;
            }
            if (thisItem.Count <= 0)
            {
                thisItem.Count = 1;
            }
        }


        //Fail safe to populate inventory representation's scale in case item is dropped
//        if(this.thisItem.item
//        {
//        }
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !PlayerInRange)
        {
            
            PlayerInRange = true;
            Debug.Log("Player entered collider.");

            /*Activate player's prompt to pickup text*/
            PopupTextUIManager.ItemUI.CreateItemCallout(other.transform, thisItem.PickupText + " Pick up?",Vector2.zero);

            /*Give player reference to this item to collect if wanted*/
            PlayerStateManager.Instance.CollectableItem = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && PlayerInRange)
        {
            PlayerInRange = false;
            Debug.Log("Player exited collider.");

            /*Dectivate player's prompt to pickup text*/
            PopupTextUIManager.ItemUI.RemoveItemCallout();
            /*Remove this item as collectable*/
            PlayerStateManager.Instance.CollectableItem = null;
        }
    }

    public void GetCollected()
    {
        /*Dectivate player's prompt to pickup text*/
        PopupTextUIManager.ItemUI.RemoveItemCallout();
        /*Remove this item as collectable*/
        PlayerStateManager.Instance.CollectableItem = null;
        /*Remove item from game world*/
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Copy this instance.
    /// </summary>
    public WorldItemScript Copy()
    {
        WorldItemScript newItem = (WorldItemScript)this.MemberwiseClone();
        return newItem;
    }

    public void InitItem(int id,Sprite itemImage,string itemName, string desc, string pickupText,Vector3 scale,Color color)
    {
        this.thisItem = new InventoryItem(id,itemImage, itemName, desc, pickupText,scale,color);
        //Populate image for item
        this.GetComponent<SpriteRenderer>().sprite = itemImage;
        //Set images color
        this.GetComponent<SpriteRenderer>().color = color;
        //Populate object name
        this.gameObject.name = itemName;
    }
}
