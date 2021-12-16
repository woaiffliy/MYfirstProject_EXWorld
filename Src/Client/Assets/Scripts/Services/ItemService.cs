using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using Managers;

namespace Assets.Scripts.Services
{
    class ItemService : Singleton<ItemService>, IDisposable
    {

        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
        }


        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
        }

        public void SendBuyItem(int shopId, int shopItemId)
        {
            Debug.Log("SendBuyItem");

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.ItemBuy = new ItemBuyRequest();
            message.Request.ItemBuy.ShopId = shopId;
            message.Request.ItemBuy.ShopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);    
        }

        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果：" + message.Result + "\n" + message.Errormsg,"购买完成");
            //背包刷新逻辑
            BagManager.Instance.Reset();
            //
        }


    }
}
