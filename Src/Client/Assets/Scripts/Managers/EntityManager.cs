using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }
    class EntityManager:Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            this.notifiers[entityId] = notify;//这是一个装满事件接口的字典，一种设计模式，多看多学
        }
        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }
        public void RemoveEntity(NEntity entity)
        {
            this.entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }//如果全部清空，这两个D用不用clear一下

        internal void Clear()// 自己加的,11.18日
        {
            entities.Clear();
            notifiers.Clear();
        }

        internal void OnEntitySync(NEntitySync entitySync)
        {
            Entity entity = null;
            entities.TryGetValue(entitySync.Id, out entity);
            if (entity!=null)
            {
                if (entitySync.Entity != null)
                {
                    entity.EntityData = entitySync.Entity;
                }
                if (notifiers.ContainsKey(entitySync.Id))
                {
                    notifiers[entitySync.Id].OnEntityChanged(entity);
                    notifiers[entitySync.Id].OnEntityEvent(entitySync.Event);
                }
            }

        }
    }
}
