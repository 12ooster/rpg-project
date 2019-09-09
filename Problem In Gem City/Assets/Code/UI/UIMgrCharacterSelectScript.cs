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

    
}
