using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UICharacterEntryScript : MonoBehaviour {

    public CharStatsData charStatsData;
    public Text nameText;
    public Image charImg;

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

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }


}
