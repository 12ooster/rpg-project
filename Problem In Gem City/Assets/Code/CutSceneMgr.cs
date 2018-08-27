using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneMgr : MonoBehaviour {

    public static CutSceneMgr _instance;
    [SerializeField]
    public CutSceneScript[] Cutscenes;

    void Awake()
    {
        //Implement as singleton to ensure only 1 in the scene
        if (CutSceneMgr._instance != null && CutSceneMgr._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            CutSceneMgr._instance = this;
        }
    }

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
