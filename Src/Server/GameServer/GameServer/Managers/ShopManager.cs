using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        public Result BuyItem(NetConnection<NetSession> sender, int shopId, int shopItemId)//这里把sender弄过来，是因为，谁找我买东西，我给谁服务，必须搞清楚sender是谁
        {
            if (!DataManager.Instance.Shops.ContainsKey(shopId))
            {
                return Result.Failed;
            }
            ShopItemDefine shopItem;
            if (DataManager.Instance.ShopItems[shopId].TryGetValue(shopItemId,out shopItem))//通过商店物品ID 可以屏蔽修改物品ID的外挂
            {
                Log.InfoFormat("BuyItem: :Character:{0}: Item:{1} ShopHadCount:{2} BuyCount:{3} Price:{4}", sender.Session.Character.Id, shopItem.ItemID, shopItem.Count,1, shopItem.Price);
                if (sender.Session.Character.Info.Gold > shopItem.Price)
                {
                    
                    sender.Session.Character.ItemManager.AddItem(shopItem.ItemID, 1);//这里暂时固定是每次点击购买，买一个道具
                    Character character = sender.Session.Character;
                    character.Gold -= shopItem.Price;//Ncharacter 的 gold 变动，并发送变动到客户端
                    sender.Session.Character.Info.Gold -= shopItem.Price;//Tcharacter 的gold 变动
                    shopItem.Count--;//商店道具-1
                    DBService.Instance.Save();
                    return Result.Success;
                }
                
            }
            return Result.Failed;
        }
    }
}
