using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;

namespace Network
{
    class NetSession:INetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }

        internal void DisConnected()
        {
            if (this.Character != null)
            {
                UserService.Instance.CharacterLeave(this.Character);
            }
        }


        //老师重写的STATUS 12.12
        NetMessage response;
        
        public NetMessageResponse Response
        {
            get
            {
                if (response == null)
                {
                    response = new NetMessage();
                }
                if (response.Response == null)
                {
                    response.Response = new NetMessageResponse();
                }
                return response.Response;
            }
        }


        public byte[] GetResponse()
        {
            if (response != null)
            {
                if (this.Character != null && this.Character.StatusManager.HasStatus)//这是在判断啥
                {
                    this.Character.StatusManager.ApplyResponse(Response);
                }
                byte[] data = PackageHandler.PackMessage(response);
                response = null;
                return data;

            }
            return null;
        }
        //以上            sender.Session.Response.ItemBuy = new ItemBuyResponse();
        //sender.Session.Response.ItemBuy.Result = result;
    }
}
