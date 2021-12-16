
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }


        public void Init()
        {

        }


        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0} Pass:{1}", request.User, request.Passward);

            //NetMessage message = new NetMessage();
            //message.Response = new NetMessageResponse();
            //message.Response.userLogin = new UserLoginResponse();
            //12.14修改
            sender.Session.Response.userLogin = new UserLoginResponse();
            //

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();//完全不懂
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户名不存在！请重新输入！";
            }
            else if (user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误！请重新输入！";
            }
            else
            {
			//这个else整体都是抄的，以后要看看！！！！！
                sender.Session.User = user;
                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";


                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = 1;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }

                
            }
            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
            sender.SendResponse();

        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0} Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();//完全不懂
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在！";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "None";
            }
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);

        }

        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest: CharName:{0} CharClass:{1}", request.Name, request.Class);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820,
                Gold = 200000
            };

            //追加背包
            var bag = new TCharacterBag();
            bag.Owner = character;
            bag.Items = new byte[0];
            bag.Unlocked = 30;
            character.Bag = DBService.Instance.Entities.TCharacterBags.Add(bag);//虽然这里写的是TCharacterBags但是是一对一的，1个character只有一个bag



            character = DBService.Instance.Entities.Characters.Add(character);//为保证是最新版本，要返回 character值更新一下
            sender.Session.User.Player.Characters.Add(character);//内存角色增加（不是很懂）
            DBService.Instance.Entities.SaveChanges();

            //已解决！！！！——!!!!!!!!!!!!导致user为空，是因为创建角色后没有把characters发送给客户端，客户端无法添加角色信息
            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = c.ID;
                info.Name = c.Name;
                info.Class = (CharacterClass)c.Class;
                info.Tid = c.TID;
                message.Response.createChar.Characters.Add(info);
            }
            //已解决！！！！——!!!!!!!!!!!!等待修正


            message.Response.createChar.Result = Result.Success;
            message.Response.createChar.Errormsg = "None";

            //因为创建角色一般没有错误，所以这里没写返回错误信息的情况

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);

        }


        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: TcharacterID:{0} Name:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;
            message.Response.gameEnter.Errormsg = "None";

            //进入成功，发送角色初始信息
            message.Response.gameEnter.Character = character.Info; //道具系统一课添加该句代码

            //道具系统测试逻辑：
            //int itemid = 1;
            //bool hasItem = character.ItemManager.HasItem(itemid);
            //Log.InfoFormat("HasItem:[{0}:{1}]", itemid, hasItem);
            //if (hasItem)
            //{
            //    //character.ItemManager.RemoveItem(itemid, 1);
            //}
            //else
            //{
            //    character.ItemManager.AddItem(1, 100);
            //    character.ItemManager.AddItem(2, 100);
            //    character.ItemManager.AddItem(3, 60);
            //    character.ItemManager.AddItem(4, 60);
            //    character.ItemManager.AddItem(5, 2);
            //    character.ItemManager.AddItem(6, 2);
            //    character.ItemManager.AddItem(7, 100);
            //    character.ItemManager.AddItem(8, 100);
            //    character.ItemManager.AddItem(9, 2);
            //    character.ItemManager.AddItem(10, 60);
            //    character.ItemManager.AddItem(11, 60);
            //    character.ItemManager.AddItem(12, 2);
            //}
            //Models.Item item = character.ItemManager.GetItem(itemid);
            //Log.InfoFormat("Item:[{0}:{1}],count:{2}", itemid, item,item.ItemCount);
            //DBService.Instance.Save();
            ////道具系统测试逻辑结束


            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
            sender.Session.Character = character;
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest: TcharacterID:{0} Name:{1} Map:{2} entityID:{3} character.id:{4}", character.Info.Id, character.Info.Name, character.Info.mapId, character.entityId, character.Id);
            CharacterLeave(character);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameLeave = new UserGameLeaveResponse();
            message.Response.gameLeave.Result = Result.Success;
            message.Response.gameLeave.Errormsg = "None";

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);


        }

        public void CharacterLeave(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.entityId);//此处修改为entityid 11.19——————character.info.ID和character.data.ID,还有数据库里的TCharacter.id是一样的，character.id和character.Entityid是一样的（character.id是啥？）
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }

}
