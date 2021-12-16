using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        internal Map(MapDefine define)
        {
            this.Define = define;

        }

        internal void Update()
        {
        }



        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} entityId:{1}", this.Define.ID, character.entityId);

            character.Info.mapId = this.ID;//把Tcharacter的mapid给当前character里，准备发送给客户端

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;//直发直用
            message.Response.mapCharacterEnter.Characters.Add(character.Info);//把要进入游戏的“我”的角色信息发给“我”的客户端。这里面也有mapid

            foreach (var kv in this.MapCharacters)//每个已经进入游戏的，当前在线的玩家信息都会存在MapCharacters中
            {
                message.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);//将MapCharacters中已经进入游戏的玩家信息添加到“我”的客户端的[待添加Characters]名单里
                this.SendCharacterEnterMap(kv.Value.connection, character.Info);//给当前在线的其他角色每人发一份“我”进入游戏的信息
            }

            this.MapCharacters[character.entityId] = new MapCharacter(conn, character);//把我自己的在线信息存进当前在线玩家列表MapCharacters中去——注意！这里名单的key是character的entityID

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        void SendCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;//把“我”的mapid、character信息发给全部的在线玩家的客户端（one by one）
            message.Response.mapCharacterEnter.Characters.Add(character);//是替代了吗？还是response里的characters不保存？————答（推测）：response里的characters是待添加characters名单，和客户端里的characters是分开的
            //将当前地图的当前角色添加到其他玩家客户端的[待添加Characters]名单里
            //这里发名单而不是直接发character的原因，【据推测】是考虑到同时进入游戏时服务器批次执行的问题，发名单就可以同时执行
            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        internal void CharacterLeave(Character cha)//传参由原NCharacterInfo格式改为Character格式——11.24
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1},entityID:{2} character.id = character.entityID", this.Define.ID, cha.Id, cha.entityId);
            //this.MapCharacters.Remove(cha.Id);//改了顺序 11.18//这里角色离开用的是character的id，注意！
            foreach (var kv in MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, cha);
            }
            this.MapCharacters.Remove(cha.entityId);//是对的，老师也改了——改了顺序 11.18
        }

        void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character cha)//传参由原NCharacterInfo格式改为Character格式——11.24
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            message.Response.mapCharacterLeave.characterId = cha.entityId;//这里发的是EntityID


            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        internal void UpdateEntity(NEntitySync entitySync)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.entityId == entitySync.Id)
                {
                    kv.Value.character.Position = entitySync.Entity.Position;
                    kv.Value.character.Direction = entitySync.Entity.Direction;
                    kv.Value.character.Speed = entitySync.Entity.Speed;
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(kv.Value.connection, entitySync);
                }
            }
        }

    }
}
