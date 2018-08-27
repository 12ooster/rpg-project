using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatEndScreenScript : MonoBehaviour {

    public string ExperienceText;
    public List<string> ItemText;
    public GameObject uiText;
    public Text endBattleMessage;

    // Use this for initialization
    void Start () 
    {
        if (endBattleMessage == null)
        {
            endBattleMessage = this.transform.GetComponentInChildren<Text>();
        }
    }

    // Update is called once per frame
    void Update () 
    {

    }

    public void SetElements(string endMessage, string xpText, List<string> items)
    {
        if (endBattleMessage == null)
        {
            endBattleMessage = this.transform.GetComponentInChildren<Text>();
        }

        endBattleMessage.text = endMessage;
        this.ExperienceText = xpText;
        this.ItemText = items;
    }

    public void DisplayAllText()
    {
        //references used for positioning created UI elements
        float colXPos = 0;
        float firstYPos = 0;
        //Create a list aggregating all strings to display
        List<string> TextToDisplay = new List<string>();
        TextToDisplay.Add(ExperienceText);

        //Add items if the list isn't empty
        if (ItemText.Count > 0)
        {
            foreach (string s in ItemText)
            {
                TextToDisplay.Add(s);
            }
        }

        //Iterate through responses and create UI element for each one
        for (int i = 0; i < TextToDisplay.Count; i++)
        {
            //Create the element
            GameObject obj = GameObject.Instantiate(uiText);
            obj.transform.SetParent(this.gameObject.transform);
            //Set display elements
            obj.GetComponent<Text>().text = TextToDisplay[i];
            //Position the newly created UI element
            RectTransform responseRect = obj.GetComponent<RectTransform>();
            responseRect.anchoredPosition =  new Vector2(colXPos,firstYPos - (responseRect.sizeDelta.y *i));
        }
    }
}
