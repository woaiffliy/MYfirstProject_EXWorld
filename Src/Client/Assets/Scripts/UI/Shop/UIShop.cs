using Common.Data;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIShop : UIWindow
    {
        public Text gold;
        public Text title;

        public Transform[] itemRoot;

        public GameObject shopItem;//shopitem.perfab
        ShopDefine shop;


        void Start()
        {

            StartCoroutine(InitItems());
        }

        IEnumerator InitItems()//为什么用协程？以后再说
        {
            foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
            {
                if (kv.Value.Status > 0)
                {
                    GameObject go = Instantiate(shopItem, itemRoot[0]);//个人觉得itemRoot[0]指的是第一个content，如果以后做第二页商品，就可以再写一个itemRoot[1]的代码
                    UIShopItem ui = go.GetComponent<UIShopItem>();
                    ui.SetShopItemInfo(kv.Key, kv.Value, this);
                }
            }
            yield return null;
        }

        public void SetShop(ShopDefine shop)
        {
            this.shop = shop;
            this.title.text = shop.Name;
            this.gold.text = User.Instance.CurrentCharacter.Gold.ToString();
        }

        private UIShopItem selectedItem;

        public void SelectShopItem(UIShopItem item)
        {
            if (selectedItem != null)
            {
                selectedItem.Selected = false;
            }
            selectedItem = item;
        }

        public void OnClickBuy()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择你要购买的道具", "购买提示");
                return;
            }
            ShopManager.Instance.BuyItem(this.shop.ID, selectedItem.shopItemId);//12.12自己加的
        }
        public void OnClickReset()
        {
            this.selectedItem = null;
            Destroy(this.gameObject);
        }
    }
}
