using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AssemblyCSharp
{
    public class SpeechText
    {
        public string[] Text;
        public GameObject UIText;
        public string ItemText;

        public SpeechText()
        {
            
        }

        public SpeechText(string[] t, GameObject uiText)
        {
            this.Text = t;
            this.UIText = uiText;
            uiText.GetComponent<TextBoxScript>().Init();
        }

        //TODO:Move this constructor into class that extends SpeechText class or Create generic UItext class and make both SpeechText and Itemtext extend it

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCSharp.SpeechText"/> class.
        /// This constructor is for use with item text only.
        /// </summary>
        /// <param name="t">T.</param>
        /// <param name="uiText">User interface text.</param>
        public SpeechText(string t, GameObject uiText)
        {
            this.ItemText = t;
            this.UIText = uiText;
        }
    }
}

