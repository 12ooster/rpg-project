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
    public int maxPartyMembers = 0;

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
            foreach( GameObject charPanelObj in selectedCharsUiPanels) {
                charPanelObj.transform.SetParent(this.selectedCharsPanel);
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
}
