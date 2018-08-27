using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    [System.Serializable]
    public class DialogueChain
    {
        public int Type = 1;
        /// <summary>
        /// The 
        /// </summary>
        [SerializeField]
        private List<Dialogue> _speechText;

        public List<Dialogue> SpeechText
        {
            get
            {
                return this._speechText;
            }
            set
            {
                this._speechText = value;
            }
        }


        public DialogueChain()
        {
            
        }

        public DialogueChain(List<Dialogue> sText)
        {
            this.SpeechText = sText;
        }
    }
}

