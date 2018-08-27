using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class PopupTextUIManager : MonoBehaviour {

    /*Speech box for character*/
    public GameObject itemCallout;

    /*Text to be displayed in box*/
    public GameObject itemText;

    /*UI Canvas and objects*/
    GameObject _canvasObject;
    GameObject _uiContainer;

    public static PopupTextUIManager ItemUI;

    float _textDelayTime = 0.01f;

    public GameObject CurrentItemText;

    void Awake()
    {
        /*Implement Item UI manager as a singleton*/
        if (ItemUI != null && ItemUI != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ItemUI = this;
        }
    }

	// Use this for initialization
	void Start () 
    {
        if (_canvasObject == null)
        {
            _canvasObject = GameObject.Find("Canvas");
        }
        if (_uiContainer == null)
        {
            _uiContainer = GameObject.Find("UIContainer");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

  

    public void CreateItemCallout(Transform speakerPos, string text, Vector2 offset)
    {
        Debug.Log("CreateItemCallout called.");
        //Instantiate object for UI text with item pickup text
        GameObject textObj = (GameObject)GameObject.Instantiate(itemText);
        //Create reference to current text for removal later
        CurrentItemText = textObj;
        //Set parent for UI container
        textObj.transform.SetParent(_uiContainer.transform,false);
        //Rect transform for UI element
        RectTransform textRect = textObj.GetComponent<RectTransform>();

        //Get RectTransform for canvas
        RectTransform CanvasRect = _canvasObject.GetComponent<RectTransform>();

        //Calculate position for the UI Element - 0,0 for the canvas is at the cneter of the screen, whereas World to viewPortPoint
        //treats the lower left as 0,0.  Because of this, you need to subrat the height/width of the canvas * 0.5 to get the correct pos

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(speakerPos.position);
        Vector2 speakerScreenPos = new Vector2(
            ((viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        /*position over desired character. Y offset needs to be double as sprite's origin pos is at its center*/
        Vector2 uiObjPos = new Vector2(speakerScreenPos.x, speakerScreenPos.y + (offset.y * 2f)); 
        /*Zero it within parent*/
        textRect.anchoredPosition = uiObjPos;
        /*Create object to pass to text coroutine*/
        SpeechText sText = new SpeechText(text, textObj);
        /*Display string arguments*/
        StartCoroutine("PopulateText",sText);

    }

    IEnumerator PopulateText(SpeechText sText)
    {
        int count = 0;
        //TODO: Add text population word by word with overflow detect
        for (int i = 0; i < sText.ItemText.Length; i++)
            {
                if (CheckTextOverflowHeight(sText.UIText.GetComponent<RectTransform>()))
                {
                    Debug.Log("Text overflowed height!");
                    yield return null;
                }
                else
                {
                sText.UIText.GetComponent<Text>().text += sText.ItemText.Substring(i, 1); 
                    yield return(new WaitForSeconds(_textDelayTime));
                }

            }

    }

    public void RemoveItemCallout()
    {
        StopCoroutine("PopulateText");
        if (CurrentItemText != null)
        {
            GameObject itemText = CurrentItemText;
            CurrentItemText = null;
            Destroy(itemText);
        }
    }

    /// <summary>
    /// Checks if text will overflow the width of its containing rect.
    /// </summary>
    /// <returns><c>true</c> If text exceeds rectangle width. <c>false</c> otherwise.</returns>
    /// <param name="textRect">Text rect.</param>
    bool CheckTextOverflowWidth(RectTransform textRect)
    {
        float textPrefWidth = LayoutUtility.GetPreferredWidth(textRect);

        return(textPrefWidth > textRect.rect.width);
    }

    /// <summary>
    /// Checks if text will overflow the height of its containing rect.
    /// </summary>
    /// <returns><c>true</c> If text exceeds rectangle height. <c>false</c> otherwise.</returns>
    /// <param name="textRect">Text rect.</param>
    bool CheckTextOverflowHeight(RectTransform textRect)
    {
        float textPrefHeight = LayoutUtility.GetPreferredHeight(textRect);

        return(textPrefHeight > textRect.rect.height);
    }


}
