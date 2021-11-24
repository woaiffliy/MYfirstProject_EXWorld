using Common;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class EntityManager : Singleton<EntityManager>
    {
        private int idx = 0;
        public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        internal void AddEntity(int mapId, Entity entity)
        {

            //这个方法没有调用，导致MapEntities没东西----------------------------------------------------------
            AllEntities.Add(entity);
            //把实体加入实体管理器并生成唯一实体ID
            entity.EntityData.Id = ++this.idx;
            

            List<Entity> entities = null;
            if (!MapEntities.TryGetValue(mapId, out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities;//这里等于说entities已经和这个字典MapEntities建立关联了，
            }
            entities.Add(entity);//为什么不是MapEntities[mapId].Add(entity)呢？————已解决！！因为entities已经和这个字典MapEntities建立关联了

        }

        internal void RemoveEntity(int mapId, Entity entity)
        {
            this.AllEntities.Remove(entity);
            MapEntities[mapId].Remove(entity);
        }
    }
}
