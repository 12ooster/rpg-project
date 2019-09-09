using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UICharacterEntryScript : MonoBehaviour {

    public CharStatsData charStatsData;
    public Text nameText;
    public Image charImg;
    public bool slotEmpty = true;
    public int selectedCharId = -1;

    public void Init( CharStatsData cStats) {
        this.InitLocalVariables();
        this.charStatsData = cStats;
        this.SetFields();
    }

    public void Init() {
        if( this.charStatsData == null) {
            Debug.LogWarning("Missing character");
        }
        this.InitLocalVariables();
        SetFields();
    }

    public void InitLocalVariables() {
        this.nameText = this.transform.Find("NameTxt").GetComponent<Text>();
        this.charImg = this.transform.Find("CharImg").GetComponent<Image>();
    }

    public void SetFields() {
        this.nameText.text = charStatsData.CharName;
        this.charImg.sprite = charStatsData.charSprite;
    }

    public void SetSelectedSlot( int charId, string charName, Sprite charImg ) {
        this.selectedCharId = charId;
        this.nameText.text = charName;
        this.charImg.sprite = charImg;
        this.slotEmpty = false;
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void OnToggle(bool toggleValue) {
        //Communicate to UI manager about toggle event
        CombatTestMenuManager._instance.UpdateSelectedCharactersList(toggleValue, this.charStatsData);
    }
}
