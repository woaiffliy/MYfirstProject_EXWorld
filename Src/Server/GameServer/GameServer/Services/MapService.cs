using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class MapService:Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);//角色同步协议
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
            
        }



        public void Init()
        {
            MapManager.Instance.Init();
        }


        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapEntitySync: CharacterID:{0} Name:{1} Entity.ID:{2} Evt:{3} Entity:{4}", character.Id, character.Info.Name, request.entitySync.Id, request.entitySync.Event, request.entitySync.Entity.String());
            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }

        internal void SendEntityUpdate(NetConnection<NetSession> conn, NEntitySync entitySync)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entitySync);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);

        }

        void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapTeleport: CharacterID:{0}:{1} TeleporterID:{2}", character.Id, character.Data.Name, request.teleporterId);

            if (!DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
            {
                Log.WarningFormat("Source TeleporterID [{0}] not existed", request.teleporterId);
                return;
            }
            TeleporterDefine source = DataManager.Instance.Teleporters[request.teleporterId];
            if (source.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(source.LinkTo))
            {
                Log.WarningFormat("Source TeleporterID [{0}] LinkTO ID [{1}] not existed", request.teleporterId, source.LinkTo);
            }

            TeleporterDefine target = DataManager.Instance.Teleporters[source.LinkTo];

            MapManager.Instance[source.MapID].CharacterLeave(character);
            character.Position = target.Position;
            character.Direction = target.Direction;
            MapManager.Instance[target.MapID].CharacterEnter(sender,character);
        }


    }
}
