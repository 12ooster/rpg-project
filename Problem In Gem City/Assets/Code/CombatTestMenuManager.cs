using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        this.uiCharSelectObject = this.transform.Find("CharacterSelectUi");
        Transform scrollViewObj = this.uiCharSelectObject.Find("CharacterSelectScrollView");
        Transform content = scrollViewObj.Find("Viewport").Find("Content");

        List<GameObject> uiBtns = this.PopulateUiButtons();
        foreach( GameObject btn in uiBtns) {
            btn.transform.SetParent(content);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //=========================Internals=========================
    List <GameObject> PopulateUiButtons() {
        List<GameObject> uiBtns = new List<GameObject>();
        //Get the data for the new enemy's party
        foreach (CharStatsScript charScript in this.selectableCharacters) {
            CharStatsData charData = new CharStatsData();
            charData = charScript.StatsAsData();
            this.characterData.Add(charData);
            //Create button for scrollview
            GameObject obj = GameObject.Instantiate(uiElemCharEntry);
            obj.GetComponent<UICharacterEntryScript>().Init( charData );
            uiBtns.Add(obj);
        }
        return uiBtns;
    }
}
