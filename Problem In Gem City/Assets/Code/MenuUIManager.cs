using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class MenuUIManager : MonoBehaviour {

    public static MenuUIManager MenuUIInstance;
    public GameObject MenuContainer;
    public GameObject ItemUIObj;
    public GameObject MenuBG;
    /// <summary>
    /// UI elements that appear to give options on a selected item.
    /// </summary>
    public GameObject SelectedItemCallout;
    /// <summary>
    /// A flag indicating if the menu is currently open
    /// </summary>
    public bool MenuOpen = true;
    /// <summary>
    /// A flag indicating if the callout for a selected item is open
    /// </summary>
    public bool CalloutOpen = false;

    public enum MenuTabs{Inventory,Party,System};
    public MenuTabs CurrentTab;
    /// <summary>
    /// The max items allowed in a column.
    /// </summary>
    public int MaxItemsCol = 6;
    /// <summary>
    /// The max items allowed on a page.
    /// </summary>
    public int MaxItemsPage = 30;
    /// <summary>
    /// The current inventory page.
    /// </summary>
    public int CurrInventoryPage = 0;
    /// <summary>
    /// The total pages of items in the inventory.  Each page is a list of GameObjects.
    /// </summary>
    public List<List<GameObject>> InventoryPages;
    /// <summary>
    /// The index of the currently selected item.
    /// </summary>
    public int SelectedItemIndex;
    

    void Awake()
    {
        if (MenuUIInstance != null && MenuUIInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            MenuUIInstance = this;
        }
    }

	// Use this for initialization
	void Start () 
    {
        if (MenuContainer == null)
        {
            InventoryPages = new List<List<GameObject>>();
            //Get reference to the container holding the UI elements of the menu for activation/deactivation
            MenuContainer = this.transform.FindChild("Container").gameObject;
            //Get reference to background of the menu
            MenuBG = MenuContainer.transform.FindChild("MenuBG").gameObject;
            //Deactivate the UI elements of the Menu
            CloseMenu();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (MenuOpen)
        {
           
                //Input check for changing selected item in menu, right arrow goes to the next selected item
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Debug.LogWarning("****DOWN KEY PRESSED IN MENU UI MANAGER****");
                    if (CalloutOpen)
                    {
                        
                        //Increment the index of selected action by 1 to adjust which UI item appears selected
                        SelectedItemCallout.GetComponent<ItemCalloutScript>().SetSelectedAction(1);
                    }
                    else
                    {
                        SelectItem(SelectedItemIndex + 1);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    if (CalloutOpen)
                    {

                        //Increment the index of selected action by 1 to adjust which UI item appears selected
                        SelectedItemCallout.GetComponent<ItemCalloutScript>().SetSelectedAction(-1);
                    }
                    else
                    {
                        SelectItem(SelectedItemIndex - 1); 
                    }
                }
                //If there is a selected item, and the player presses the "A"/use button the bring up context menu
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (CalloutOpen)
                    {
                        SelectedItemCallout.GetComponent<ItemCalloutScript>().SelectedAction();
                    }
                    else if(InventoryPages[CurrInventoryPage][SelectedItemIndex] != null)
                    {
                        OpenCallout(InventoryPages[CurrInventoryPage][SelectedItemIndex]);
                    }
                }

                //TODO: When callout is open and escape key is pressed, close callout
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (CalloutOpen)
                    {
                        CloseCallout();
                    }
                    else
                    {
                        CloseMenu();
                    } 
                }
                //TODO: When callout is open and  and action key is pressed again, excecute selected action
                //TODO: Add 'page' buttons that are shown only when numItems exceeds page size.  Pressing button calls Display inventory on currPageIdex + or - 1
        }

		
	}

    public void OpenCallout(GameObject selectedItem)
    {
        //Create context menu at teh location of the selected item
        Transform selectedTrans = InventoryPages[CurrInventoryPage][SelectedItemIndex].transform;
        SelectedItemCallout.transform.position = 
            new Vector3(selectedTrans.position.x + selectedTrans.gameObject.GetComponent<RectTransform>().rect.width,
                        selectedTrans.position.y);
        SelectedItemCallout.SetActive(true);
        SelectedItemCallout.GetComponent<ItemCalloutScript>().SelectedItem = selectedItem.GetComponent<InventoryItemScript>();
        CalloutOpen = true;
    }

    /// <summary>
    /// Closes the ui callout with options for actions on inventory items.
    /// </summary>
    public void CloseCallout()
    {
        //set flag for input handling
        this.CalloutOpen = false;
        //Call instance to handle closing the callout
        ItemCalloutScript.Instance.CloseCallout();
    }

    public void OpenMenu()
    {
        //Set the container object that holds menu UI objects
        MenuContainer.SetActive(true);
        //Build lists for inventory pages
        PopulateInventory();
        //Start the menu open to the first tab in list of pages (first page basically, if there are more than 1)
        SetActiveTab(CurrentTab);
        //Start menu with first item in menu highlighted
        SearchSetSelectedItem(0);
        MenuOpen = true;


    }

    public void RefreshMenu()
    {
        //TODO:Write function for "refreshing" the position of items after one has been removed
    }

    public void CloseMenu()
    {

        //Depopulate each list in pages
        foreach (List<GameObject> list in InventoryPages)
        {
            //Destroy each item in list
            foreach (GameObject g in list)
            {
                Destroy(g);
            }

            list.Clear();
        }

        //Clear list holding pages
        InventoryPages.Clear();
        //Deactivate gameobjects
        MenuContainer.SetActive(false);
        CurrentTab = MenuTabs.Inventory;
        MenuOpen = false;

    }

    public void SetActiveTab(MenuTabs tab)
    {
        switch (tab)
        {
            case MenuTabs.Inventory:
                DisplayInventory(CurrInventoryPage);
                break;
        }
    }

    public void SearchSetSelectedItem(int itemIndex)
    {   
        //Handle min/max page item check to make sure item asked for doesn't go out of bounds
        if (itemIndex < 0 || itemIndex > InventoryPages[CurrInventoryPage].Count-1)
        {
            return;
        }

        //Set previously selected item as false
        if (InventoryPages[CurrInventoryPage][SelectedItemIndex] != null)
        {
            InventoryPages[CurrInventoryPage][SelectedItemIndex].GetComponent<InventoryItemScript>().SetSelected(false);
        }

        InventoryItemScript searchedForItem = null;

        //Find currently selected item
        foreach (GameObject obj in InventoryPages[CurrInventoryPage])
        {
            InventoryItemScript thisItemScript = obj.GetComponent<InventoryItemScript>();

            /*If the items set index matches the index we are looking for we set it to be selected*/
            if (obj.GetComponent<InventoryItemScript>().PageIndex == itemIndex)
            {
                
                this.SelectedItemIndex = itemIndex;
                thisItemScript.SetSelected(true);
                //set variable to test if result was found successfully
                searchedForItem = thisItemScript;
            }
        }

        if (searchedForItem == null)
        {
            Debug.LogError("Selected item index not found in menu and inventory!");
        }
    }

    public void SelectItem(int itemIndex)
    {
        //Handle min/max page item check to make sure item asked for doesn't go out of bounds
        if (itemIndex < 0 || itemIndex > InventoryPages[CurrInventoryPage].Count-1)
        {
            return;
        }

        //Set previously selected item as false
        if (InventoryPages[CurrInventoryPage][SelectedItemIndex] != null)
        {
            InventoryPages[CurrInventoryPage][SelectedItemIndex].GetComponent<InventoryItemScript>().SetSelected(false);
        }

        //Set selected item
        SelectedItemIndex = itemIndex;
        InventoryPages[CurrInventoryPage][SelectedItemIndex].GetComponent<InventoryItemScript>().SetSelected(true); 
    }

    public void SelectAction()
    {
        ItemCalloutScript calloutScript = SelectedItemCallout.GetComponent<ItemCalloutScript>();

    }

    /// <summary>
    /// Displays the inventory UI for the specified index page.
    /// </summary>
    /// <param name="index">Index.</param>
    public void DisplayInventory(int index)
    {
        //Deactivate current page
        foreach (GameObject itemUI in InventoryPages[CurrInventoryPage])
        {
            itemUI.SetActive(false);
        }

        //Activate each item in current page
        CurrInventoryPage = index;
        foreach (GameObject itemUI in InventoryPages[CurrInventoryPage])
        {
            itemUI.SetActive(true);
        }
    }

    public void PopulateInventory()
    {
        Debug.Log("Display Inventory called.");
       
        //Variables holding total items displayed so far and items on current page
        int totalItemsCount=0,pageItemsCount = 0;

        RectTransform canvasRect = CanvasScript.CanvasScriptInstance.GetComponent<RectTransform>();
        //Rect Y pos is calculated from bottom to top but we want to start from top so inverse
        float firstYPos = 0.0f;
        //Col pos variable holds the x position of the current column
        float colXPos = CanvasScript.CanvasScriptInstance.GetComponent<CanvasScaler>().referenceResolution.x * -1;
        //Create the initial page of the inventory
        List<GameObject> InventoryPage0 = new List<GameObject>();
        //Add page to list of pages
        InventoryPages.Add(InventoryPage0);
        //Loop through each item in player's inventory and add it to a list for a page
        foreach (InventoryItem i in PlayerStateManager.Instance.PlayerInventory.Items.Values)
        {   
            //Insantiate object
            GameObject obj = GameObject.Instantiate(ItemUIObj);
            //Initialize script to get refrences to display elements
            InventoryItemScript s = obj.GetComponent<InventoryItemScript>();
            s.Init();
            /*Set display Info*/
            s.SetDisplayElements(i);
            /*Set index used for managing what item is currently selected*/
            s.PageIndex = pageItemsCount;
            /*Make child of UI container*/
            obj.transform.SetParent(this.MenuContainer.transform);
            //TODO: Add code to position correctly within window
            RectTransform itemRect = obj.GetComponent<RectTransform>();
            int colNum = pageItemsCount / MaxItemsCol;
            itemRect.anchoredPosition =  new Vector2(colXPos + (itemRect.sizeDelta.x *colNum),firstYPos - (itemRect.sizeDelta.y * (pageItemsCount % MaxItemsCol)));
            //Add newly created item Rect to list for this page
            InventoryPages[pageItemsCount/MaxItemsPage].Add(obj);
            //Increment item UI obj count for current page and total
            pageItemsCount++;
            totalItemsCount++;

            //Debug.Log("count/maxItemsCol = " + count / maxItemsCol);
            /*If the count of items for this current page exceeds page size
             * then we create list for an additional page
             */
            if (pageItemsCount > MaxItemsPage)
            {
                //Create a new page for inventory which should be at index : totalItemsCount/MaxItemsPage
                InventoryPages.Add(new List<GameObject>()); 
                //Reset page count
                pageItemsCount = 0;
            }
            //TODO: Add code to destroy these on close or otherwise avoid repeats
        }

        //All items have been added, so deactivate all pages except for the first

    }

}
