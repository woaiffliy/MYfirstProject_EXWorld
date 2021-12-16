using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Assets.Scripts.UI
{
    public class UIShopItem : MonoBehaviour,ISelectHandler
    {

        //要实现功能：1.首先要能修改物品图片、名字、价钱（是根据读表得来的信息）
        //2. 要能被选中（选中有高亮效果）（鼠标点击反馈）（也可用button实现，但打算换种方式）
        //3.

        public Image Icon;
        public Text Name;
        public Text Price;
        public Text Count;


        public Image background;
        public Sprite normalBg;
        public Sprite selectedBg;

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                this.background.overrideSprite = selected ? selectedBg : normalBg;
            }
        }

        public int shopItemId;
        private UIShop shop;
        private ItemDefine item;
        //private ShopItemDefine shopitem { get; set; }

        public void SetShopItemInfo(int id, ShopItemDefine shopItemDefine, UIShop owner)
        {
            this.shop = owner;
            this.shopItemId = id;//1001-1012
            this.item = DataManager.Instance.Items[shopItemDefine.ItemID];
            this.Icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
            this.Name.text = this.item.Name;
            this.Count.text = shopItemDefine.Count.ToString();
            this.Price.text = shopItemDefine.Price.ToString();
        }

        public void OnSelect(BaseEventData eventData)
        {
            this.selected = true;
            this.shop.SelectShopItem(this);
        }

        //public void SetShopItemInfo(string iconName, ShopItemDefine item ,UIShop shop)
        //{
        //    this.ItemIcon.overrideSprite = Resloader.Load<Sprite>(iconName);
        //    //this.ItemPrice.text = price;
        //    //this.ItemName.text = name;
        //}
    }
}

