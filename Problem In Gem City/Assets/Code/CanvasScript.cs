using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

    public static CanvasScript CanvasScriptInstance;
    Canvas canvas;

    void Awake()
    {
        //Implement Singleton pattern and keep this object between scenes
        if (CanvasScriptInstance != null && CanvasScriptInstance != this)
        {
            Destroy(this.gameObject);    
        }
        else
        {
            CanvasScriptInstance = this;
            //Preserve object between scenes so can be used for battle UI
            DontDestroyOnLoad(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () 
    {
        if (canvas == null)
        {
            canvas = this.GetComponent<Canvas>();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
