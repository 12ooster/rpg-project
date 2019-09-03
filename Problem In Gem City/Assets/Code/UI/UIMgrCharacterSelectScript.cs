using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class UIMgrCharacterSelectScript: MonoBehaviour
{
    public static UIMgrCharacterSelectScript _instance;
    public List<CharStatsData> selectedCharacters;

    // Start is called before the first frame update
    void Start()
    {
        if(UIMgrCharacterSelectScript._instance != null && UIMgrCharacterSelectScript._instance != this) {
            Destroy(this.gameObject);
        }else{
            UIMgrCharacterSelectScript._instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void UpdateSelectedCharactersList( bool toggleVal, CharStatsData data ) {
        Debug.Log("UpdateSelectedCharactersListCalled" + toggleVal.ToString());
        Debug.Log("Character selected/deselected! : " + data.CharName);
        if( toggleVal) {
            this.selectedCharacters.Add(data);
            //Add Image to UI
        }else{
            this.selectedCharacters.Remove(data);
            //Remove Image from UI
        }
        Debug.Log("Number of characters currently selected is:" + this.selectedCharacters.Count.ToString());
    }
}
