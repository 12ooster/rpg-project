using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Transform toggleGroup;

    // Start is called before the first frame update
    void Start()
    {
        this.uiCharSelectObject = this.transform.Find("CharacterSelectUi");
        Transform scrollViewObj = this.uiCharSelectObject.Find("CharacterSelectScrollView");
        //Create object to house character select toggles
        if( toggleGroup == null) {
            this.toggleGroup = scrollViewObj.Find("Viewport").Find("ToggleGroup");
        }
        List<GameObject> uiToggles = this.PopulateUiEntries();
        ToggleGroupCharacterSelect toggleGroupScript = toggleGroup.GetComponent<ToggleGroupCharacterSelect>();
        //Initialize needed array
        toggleGroupScript.characterToggles = new Toggle[uiToggles.Count];
        for ( int i = 0; i < uiToggles.Count; i++) {
            Toggle toggle = uiToggles[i].GetComponent<Toggle>();
            toggle.transform.SetParent(this.toggleGroup);
            toggleGroupScript.characterToggles[i] = toggle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //=========================Internals=========================
    List <GameObject> PopulateUiEntries() {
        List<GameObject> uiEntries = new List<GameObject>();
        //Get the data for the new enemy's party
        foreach (CharStatsScript charScript in this.selectableCharacters) {
            CharStatsData charData = new CharStatsData();
            charData = charScript.StatsAsData();
            this.characterData.Add(charData);
            //Create button for scrollview
            GameObject obj = GameObject.Instantiate(uiElemCharEntry);
            obj.GetComponent<UICharacterEntryScript>().Init( charData );
            uiEntries.Add(obj);
        }
        return uiEntries;
    }
}
