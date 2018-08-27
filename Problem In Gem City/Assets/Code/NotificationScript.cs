using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationScript : MonoBehaviour {

    public Image bgImage;
    public Text text; 
    //speed at which item received text fades out
    float fadeSpeed = .5f;
    //Offset for text from player
    public Vector2 posOffset;

    void Awake()
    {
    }

    // Use this for initialization
	void Start () 
    {
	}
	
    public void Init(string notifText, Vector3 playerPos)
    {
        if (this.text == null)
        {
            text = this.GetComponent<Text>();
        }

        if (posOffset == null)
        {
            posOffset = new Vector2(1.0f, 1.0f);
        }

        //Set notification text
        SetText(notifText);
        //Set position
        SetPosition(playerPos);
    }

    public void SetText(string newText)
    {
        this.text.text = newText;
    }

	// Update is called once per frame
	void Update () 
    {
		
	}

    public void StartFadeOut()
    {
        StartCoroutine("FadeOut");
    }

    public void SetPosition(Vector3 playerPos)
    {
        Debug.Log("---------- FindPosition for notification called! ----------");
        GameObject _canvasObject = SpeechUIManager._instance.CanvasObject;
        //Get RectTransform for canvas
        RectTransform CanvasRect = _canvasObject.GetComponent<RectTransform>();

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(playerPos);
        Vector2 playerScreenPos = new Vector2(
            ((viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        Vector2 newPos = new Vector2(playerScreenPos.x + posOffset.x, playerScreenPos.y + posOffset.y);
        /*Rect*/ 
        this.text.GetComponent<RectTransform>().anchoredPosition = newPos;
    }

    IEnumerator FadeOut()
    {
        Debug.Log("*****^^^^^ FadeOut called! ^^^^^*****");
        //Text starting color
        Color startColor = text.color;
        Color targetColor = new Color(text.color.r, text.color.b, text.color.g, 0);

        //ratio between 1 and 0 for lerping
        float ratio = 0;

        Debug.Log("Start color alpha: " + startColor.a);
        Debug.Log("End color alpha: " + targetColor.a);

        while(ratio < 1)
        {
            Color newColor = Color.Lerp(startColor, targetColor,ratio/1);
            Debug.Log("New color alpha$$$$$: " + newColor.a);
            text.color = newColor;
            ratio += Time.deltaTime*fadeSpeed;
            Debug.Log("Current color fade ratio: " + ratio);
            yield return null;
        }
        //Object faded out so remove it
        Destroy(this.gameObject, .01f);       
    }

}
