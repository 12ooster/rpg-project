using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgrScript : MonoBehaviour
{
    public static GameDataMgrScript _instance;
    public GameDataManager gameData;

    // Start is called before the first frame update
    void Start(){
        if( GameDataMgrScript._instance != null && GameDataMgrScript._instance != this) {
            Destroy(this.gameObject);
        }else{
            GameDataMgrScript._instance = this;
            DontDestroyOnLoad( this.gameObject );
        }
    }

    //TODO: Load character data from prefab assets or files by other means
    public void LoadCharacterData() {
        
    }

    public void Init( CharStatsScript[] charData) {
        this.gameData = new GameDataManager( charData );
    }

    // Update is called once per frame
    void Update(){
        
    }
}
