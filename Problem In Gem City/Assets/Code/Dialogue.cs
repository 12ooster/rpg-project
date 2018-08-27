using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

namespace AssemblyCSharp
{
    /// <summary>
    /// Dialogue class.  Holds lists of each dialogue
    /// </summary>
    [System.Serializable]
    public class Dialogue
    {

        /// <summary>
        /// Flag for whether or not this piece of dialogue requires a response
        /// </summary>
        [SerializeField]
        private bool _reqResponse = false;

        public bool ReqResponse
        {
            get
            {
                return this._reqResponse;
            }
            set
            {
                _reqResponse = value;
            }
        }

        [SerializeField]
        private int _itemIndex = -1;

        public int ItemIndex 
        {
            get
            {
                return _itemIndex;
            }
            set
            {
                this._itemIndex = value;
            }
        }


        [SerializeField]
        private int _responseType = -1;

        public int ResponseType
        {
            get
            {
                return _responseType;
            }
            set
            {
                _responseType = value;
            }
        }

        [SerializeField]
        private List<DialogueResponse> _responses;

        public List<DialogueResponse> Responses
        {
            get{ return this._responses;}
            set{ this.Responses = value;}
        }

        [SerializeField]
        private string[] _text;

        public string[] Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
 
        public Dialogue()
        {
            
        }
    }
}

