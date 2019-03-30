using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim : MonoBehaviour {

    public bool Animate = true;
    public bool AnimVert = true;

    public Vector2 StartPos;
    public Vector2 MoveSpeed;
    Vector2 _maxPos;
    Vector2 _minPos;

    public float VertAdjustDist = 5.0f;
    public float HorizAdjustDist = 1.0f;
    float dir = 1;

    // Use this for initialization
	void Start () 
    {
        StartPos = this.transform.position;	
        //_maxPos = new Vector2(StartPos.x + HorizAdjustDist, StartPos.y + VertAdjustDist);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Animate)
        {   
            Move();

            /*If animating vertical bob then check if Y is greater or less then max pos.*/
            if (AnimVert) 
            {
                if (this.transform.position.y >= StartPos.y + VertAdjustDist || this.transform.position.y <= StartPos.y - VertAdjustDist)
                {
                    dir *= -1;
                }
              
            }
            else  /*If animating horizontal bob then check if x is greater or less then max pos.*/
            {
                if (this.transform.position.x >= StartPos.x + HorizAdjustDist || this.transform.position.x <= StartPos.y - HorizAdjustDist)
                {
                    dir *= -1;
                } 
            }
        }
	}

    public void Move()
    {
        Vector2 currPos = this.gameObject.transform.position;
        Vector2 newPos = currPos + (dir * MoveSpeed * Time.deltaTime);
        this.gameObject.transform.position = newPos;
    }

    public void PositionElementOnSprite(GameObject spriteObj)
    {
        //Get this UI element's rect transform
        RectTransform rTrans = GetComponent<RectTransform>();
        //Get reference to the canvas
        Canvas canvas = CombatUIManager._instance.CanvasObject.GetComponent<Canvas>();
        //Calculate the screen offset
        Vector2 uiOffset = new Vector2((float) canvas.GetComponent<RectTransform>().sizeDelta.x/2f, (float)canvas.GetComponent<RectTransform>().sizeDelta.y/2f);

        /*Move the ui element to the world position of the gameObj passed as parameter*/

        //First get an adjusted world position to account for sprite height by adding half of the sprite's height and this arrow's height
        float spriteHeightOffset = spriteObj.GetComponent<SpriteRenderer>().bounds.extents.y * 2;
        Vector2 adjustedPos = new Vector2(spriteObj.transform.position.x,spriteObj.transform.position.y + spriteHeightOffset);
        //Then find the viewport pos
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(adjustedPos);
        //Find adjusted pos
        Vector2 proportionalPos = 
            new Vector2(viewportPos.x * canvas.GetComponent<RectTransform>().sizeDelta.x,
                viewportPos.y * canvas.GetComponent<RectTransform>().sizeDelta.y);
        //Set the position and remove the screen offset
        this.GetComponent<RectTransform>().localPosition = proportionalPos - uiOffset;
        Debug.Log("PositionElementOnSpirte called!");
    }

    public void SetText(string newText)
    {
        this.GetComponent<Text>().text = newText;
    }
}
