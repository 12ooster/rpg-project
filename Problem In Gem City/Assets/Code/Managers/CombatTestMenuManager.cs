using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class CombatTestMenuManager : MonoBehaviour
{
    /// <summary>
    /// Data for all selectable characters that can be assigned to combat parties.
    /// </summary>
    [SerializeField]
    public List<CharStatsScript> selectableCharacters;

    public List<CharStatsData> characterData;
    public Transform uiCharSelectObject;
    public GameObject uiElemCharEntry;
    public GameObject uiElemPartyEntry;
    public Transform toggleGroup;
    public Transform selectedCharsPanel;
    public List<UICharacterEntryScript> selectedCharsUiScripts;
    public int maxPartyMembers = 0;
    public List<CharStatsData> selectedCharacters;

    public static CombatTestMenuManager _instance;

    // Start is called before the first frame update
    void Start()
    {
        //Implement singleton pattern
        if( CombatTestMenuManager._instance != null && CombatTestMenuManager._instance != this) {
            Destroy(this.gameObject);
        } else {
            CombatTestMenuManager._instance = this;

            this.uiCharSelectObject = this.transform.Find("CharacterSelectUi");
            Transform scrollViewObj = this.uiCharSelectObject.Find("CharacterSelectScrollView");
            //Create object to house character select toggles
            if (toggleGroup == null) {
                this.toggleGroup = scrollViewObj.Find("Viewport").Find("ToggleGroup");
            }
            //Create and parent UI toggles
            List<GameObject> uiToggles = this.PopulateCharacterSelectUiEntries();
            ToggleGroupCharacterSelect toggleGroupScript = toggleGroup.GetComponent<ToggleGroupCharacterSelect>();
            //Initialize needed array
            toggleGroupScript.characterToggles = new Toggle[uiToggles.Count];
            for (int i = 0; i < uiToggles.Count; i++) {
                Toggle toggle = uiToggles[i].GetComponent<Toggle>();
                toggle.transform.SetParent(this.toggleGroup);
                toggleGroupScript.characterToggles[i] = toggle;
            }
            //Find reference to Ui element that will house selected chars
            this.selectedCharsPanel = this.transform.Find("SelectedCharactersPanel");
            //Create and parent Ui elems for selected chars
            List<GameObject> selectedCharsUiPanels = this.PopulateSelectedPartyUiEntries();
            float colXPos = 0.0f;
            float yPos = 0.0f;
            int itemNum = 0;
            foreach ( GameObject charPanelObj in selectedCharsUiPanels) {
                charPanelObj.transform.SetParent(this.selectedCharsPanel);
                RectTransform itemRect = charPanelObj.GetComponent<RectTransform>();
                //Update anchored position
                itemRect.anchoredPosition = new Vector2(colXPos + (itemRect.sizeDelta.x * itemNum), yPos);
                itemNum++;
                //Make sure local variables are initialized
                UICharacterEntryScript charScript = charPanelObj.GetComponent<UICharacterEntryScript>();
                charScript.InitLocalVariables();
                this.selectedCharsUiScripts.Add( charScript );
            }

            //TODO Move this character loading into GameDataMgrScript
            GameDataMgrScript._instance.Init(this.selectableCharacters.ToArray());
        }
    }

    // Update is called once per frame
    void Update(){
        
    }


    //=========================Internals=========================
    List <GameObject> PopulateCharacterSelectUiEntries() {
        List<GameObject> uiEntries = new List<GameObject>();
        //Get the data for the player's party 
        foreach (CharStatsScript charScript in this.selectableCharacters) {
            CharStatsData charData = new CharStatsData();
            charData = charScript.StatsAsData();
            this.characterData.Add(charData);
            //Create button for scrollview
            GameObject obj = GameObject.Instantiate(this.uiElemCharEntry);
            obj.GetComponent<UICharacterEntryScript>().Init( charData );
            uiEntries.Add(obj);
        }
        //TODO: Get the data for the new enemy's party 
        return uiEntries;
    }

    List<GameObject> PopulateSelectedPartyUiEntries() {
        List<GameObject> currentPartyUiEntries = new List<GameObject>();
        for( int i = 0; i < GameConstants.MAX_PARTY_SIZE; i++) {
            GameObject obj = GameObject.Instantiate(this.uiElemPartyEntry);
            currentPartyUiEntries.Add(obj);
        }
        return currentPartyUiEntries;
    }

    public void UpdateSelectedCharactersList(bool toggleVal, CharStatsData data) {
        Debug.Log("UpdateSelectedCharactersListCalled" + toggleVal.ToString());
        Debug.Log("Character selected/deselected! : " + data.CharName);
        if (toggleVal) {
            this.selectedCharacters.Add(data);
            //Add Image to UI
            this.UpdateDisplayAddSelectedCharacter(data.ID, data.charSprite, data.CharName);
        } else {
            this.selectedCharacters.Remove(data);
            //Remove Image from UI
        }
        Debug.Log("Number of characters currently selected is:" + this.selectedCharacters.Count.ToString());
    }

    public void UpdateDisplayAddSelectedCharacter( int charId, Sprite charImg, string charName) {
        for( int i = 0; i < this.selectedCharsUiScripts.Count; i++) {
            if( this.selectedCharsUiScripts[i].slotEmpty) {
                this.selectedCharsUiScripts[i].SetSelectedSlot(charId, charName, charImg);
                break;
            }
        }
    }

    public void UpdateDisplayRemoveSelectedCharacter() {

    }
}
