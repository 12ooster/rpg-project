using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class MenuOptionAbility : MonoBehaviour {

    public Color StartingColor;
    public Image bgImage;
    public int OptionIndex = -1;
    public float SelectedAlpha =0.15f;
    public CharAbility Ability;

    // Use this for initialization
    void Start () 
    {
        if (bgImage == null)
        {
            bgImage = GetComponentInChildren<Image>();
        }
    }

    // Update is called once per frame
    void Update () 
    {

    }

    /// <summary>
    /// Sets the display elements.
    /// </summary>
    /// <param name="item">The item to which this object corresponds.</param>
    public void SetDisplayElements(CharAbility ability)
    {
        if (bgImage == null)
        {
            bgImage = GetComponentInChildren<Image>();
        }

        //Get color to be used for selection state
        StartingColor = bgImage.color;
        //Set display text to match response text
        this.gameObject.GetComponent<Text>().text = ability.AbilityName;
    }

    /// <summary>
    /// Action for this option in the menu.
    /// </summary>
    /// <param name="item">Item.</param>
    public void Action(CharAbility ability)
    {
        //Update index for next dialogue thread 

        //Dialogue option selected so close callout
        SpeechUIManager._instance.CloseResponseBox();
    }

    /// <summary>
    /// Sets the dialogue option as selected and updates UI to show this.
    /// </summary>
    public void SetSelected(bool selected)
    {
        if (selected)
        {
            //this.GetComponent<Image>().color = new Color(StartingColor.r, StartingColor.g, StartingColor.b, SelectedAlpha);
            bgImage.color = new Color(StartingColor.r, StartingColor.g, StartingColor.b, SelectedAlpha);
        }
        else
        {
            //this.GetComponent<Image>().color = StartingColor;
            bgImage.color = StartingColor;
        }
    }

    public void Action()
    {
        //Action method overridden by menu options that inherit from this class
    }
}
