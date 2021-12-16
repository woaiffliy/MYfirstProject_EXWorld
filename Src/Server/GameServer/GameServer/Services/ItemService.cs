using Network;
using System;
using SkillBridge.Message;
using Common;
using GameServer.Entities;
using GameServer.Managers;

namespace Services
{
    class ItemService : Singleton<ItemService>, IDisposable
    {

        public ItemService()
        {
            //MessageDistributer.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
        }

        public void Init()
        {

        }

        public void Dispose()
        {
            //MessageDistributer.Instance.Unsubscribe<ItemBuyRequest>(this.OnItemBuy);
        }


        void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("ItemBuyRequest: Character:{0} ShopId:{1} ShopItemId:{2}", character.Id,request.ShopId, request.ShopItemId);
            var result = ShopManager.Instance.BuyItem(sender, request.ShopId, request.ShopItemId);

            //过去的写法
            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.ItemBuy = new ItemBuyResponse();
            //message.Response.ItemBuy.Result = result;

            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
            //修改底层之后写法:

            sender.Session.Response.ItemBuy = new ItemBuyResponse();
            sender.Session.Response.ItemBuy.Result = result;
            sender.SendResponse();

        }


    }
}
