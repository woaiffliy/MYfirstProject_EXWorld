using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Common.Data;
using Services;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {


       public int CurrentMapId;//问题找到了，是这个参数在UserService接收到OnMapCharacterEnter通知时没有更新MapId


        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }



        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);

        }


        public void Init()
        {

        }


        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map{0} Count:{1}", response.mapId, response.Characters.Count);
            foreach (var cha in response.Characters)//已解决！！是正确的，第二个角色进入的时候，待添加的角色只有第二个角色，故只有一个。这里的Characters是response的[待添加character列表]————在第二个角色进入游戏时，第一个角色的character不知为啥变得只有第二个角色一个了
            {
                if (User.Instance.CurrentCharacter == null || User.Instance.CurrentCharacter.Id == cha.Id)//第一个创建的角色会是主角吗?在地图中有其他角色的情况下
                {//当前角色切换地图
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId != response.mapId)//感觉【有隐患】，如果收到了其他角色进入其他地图的消息，不是就会跟着其他角色切换地图了嘛?——暂时不动，出问题再改
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }
        }


        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMap = map;
                SceneManager.Instance.LoadScene(map.Resource);
                
            }
            else
            {
                Debug.LogErrorFormat("EnterMap: Map {0} not existerd", mapId);
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:characterEntityId:{0}", response.characterId);
            if (response.characterId!=User.Instance.CurrentCharacter.Entity.Id)
            {
                CharacterManager.Instance.RemoveCharacter(response.characterId);//这个characterID是entityid
            }
            else
            {
                CharacterManager.Instance.Clear();
                //在这里应该再写 "离开地图时保存角色背包信息"的代码
            }
        }

   

        internal void SendMapEntitySync(EntityEvent entityEvent, NEntity entityData)//同步协议
        {
            Debug.LogFormat("MapEntityUpdateRequest: ID:{0} POS:{1} DIR:{2} SPD:{3}", entityData.Id, entityData.Position.String(), entityData.Direction.String(), entityData.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entityData.Id,
                Event = entityEvent,
                Entity = entityData
            };
            NetClient.Instance.SendMessage(message);
        }



        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse:Entitys:{0}", response.entitySyncs.Count);
            sb.AppendLine();
            foreach (var entity in response.entitySyncs)
            {
                Managers.EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat("     {0} evt:{1} entity:{2}", entity.Id, entity.Event, entity.Entity.String());
                sb.AppendLine();
                
            }
           // Debug.Log("{0}", sb.ToString());
        }

        internal void SendMapTeleport(int teleporterID)
        {
            Debug.LogFormat("MapTeleportRequest: teleporterID:{0}", teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }

    }
}
