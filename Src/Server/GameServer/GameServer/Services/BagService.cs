using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class BagService
    {

        public BagService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(this.OnBagSave);
        }

        void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest request)//背包道具存储
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("BagSaveRequest: Character:{0} Unlocked:{1}", character.Id, request.BagInfo.Unlocked);

            if (request.BagInfo!=null)
            {
                character.Data.Bag.Items = request.BagInfo.Items;
                DBService.Instance.Save();
            }
        }
    }
}
