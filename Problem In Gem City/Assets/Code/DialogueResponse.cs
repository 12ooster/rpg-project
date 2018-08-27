using System;
using UnityEngine;

namespace AssemblyCSharp
{
    [Serializable]
    public class DialogueResponse
    {
        public DialogueResponse()
        {
            
        }

        [SerializeField]
        private string _text;

        public string Text
        {
            get{ return this._text;}

            set{ this._text = value;}
        }

        [SerializeField]
        private int _dialogueIndex;

        public int DialogueIndex
        {
            get{ return this._dialogueIndex;}
            set {this._dialogueIndex = value;}
        }
    }
}

