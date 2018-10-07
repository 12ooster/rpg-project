using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCombatVisualManager : MonoBehaviour {

    public bool IsBusy{
        get{
            return this._isBusy;
        }
    }

    private bool _isBusy = false;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    #region Combat Movements

    public void HitTranslate () {
    }

    #endregion
}
