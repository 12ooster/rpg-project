using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    [System.Serializable]
    public class InventoryItem
    {
        [SerializeField]
        private int _ID;
       
        public int ID
        {
            get{return this._ID;}
            set{this._ID = value;}
        }

        [SerializeField]
        private Sprite _itemImage;

        public Sprite ItemImage
        {
            get{return this._itemImage;}
            set{ this._itemImage = value; }
        }

        [SerializeField]
        private string _itemDescription;

        public string ItemDescription
        {
            get{return this._itemDescription;}
            set{this._itemDescription = value;}
        }

        [SerializeField]
        private string _itemName;

        public string ItemName
        {
            get{ return this._itemName;}
            set{ this._itemName = value;}
        }

        [SerializeField]
        private string _pickupText;
       
        public string PickupText
        {
            get { return this._pickupText;}
            set { this._pickupText = value;}
        }

        [SerializeField]
        private Vector3 _itemScale;

        public Vector3 ItemScale
        {
            get { return this._itemScale;}
            set { this._itemScale = value;}
        }

        [SerializeField]
        private Color _itemColor;

        public Color ItemColor
        {
            get{ return this._itemColor;  }
            set{ this._itemColor = value; }
        }

        [SerializeField]
        private int _count;

        public int Count
        {
            get{ return this._count;}
            set{ this._count = value;}
        }

        public InventoryItem()
        {
        }

        public InventoryItem(int id,Sprite img,string name, string desc, string pickupTxt,Vector3 scale, Color currColor,int count=1)
        {
            this.ID = id;
            this.ItemImage = img;
            this.ItemName = name;
            this.ItemDescription = desc;
            this.PickupText = pickupTxt;
            this.ItemScale = scale;
        }



    }
}

