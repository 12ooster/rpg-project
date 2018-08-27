using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxScript : MonoBehaviour {

    public GameObject ContinueArrow;

    public void Init()
    {
        if (ContinueArrow == null)
        {
            ContinueArrow = this.gameObject.transform.FindChild("ContinueArrow").gameObject;
        }

        ContinueArrow.gameObject.SetActive(false);
    }

    // Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    /// <summary>
    /// Reverses the arrow's current active state.
    /// </summary>
    public void SwitchArrow()
    {
        Debug.Log("Switch arrow called to turn on!");
        if (ContinueArrow != null)
        {
            ContinueArrow.SetActive(!ContinueArrow.activeSelf);
        }
    }
}
