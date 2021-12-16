using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SkillBridge.Message;

namespace Entities
{
    public class Entity//这个整体结构还需再看，看不是很懂
    {
        public int entityId;


        public Vector3Int position;
        public Vector3Int direction;
        public int speed;


        private NEntity entityData;
        public NEntity EntityData
        {
            get {
                //UpdateEntityData();//这里
                return entityData;
            }
            set {
                entityData = value;
                this.SetEntityData(value);
            }
        }

        public Entity(NEntity entity)
        {
            this.entityId = entity.Id;
            this.entityData = entity;
            this.SetEntityData(entity);
        }

        public virtual void OnUpdate(float delta)
        {
            if (this.speed != 0)
            {
                Vector3 dir = this.direction;
                this.position += Vector3Int.RoundToInt(dir * speed * delta / 100f);
            }
            entityData.Position.FromVector3Int(this.position);//这里的东西 在移动同步第三课中整理成UpdateEntityData()方法，并放在前面了。
            entityData.Direction.FromVector3Int(this.direction);//原因大概是那样子同步更有效率，涉及到OnUpdate的帧率和EntityData的调用频率（大概是，目前还不太能理解）问题
            entityData.Speed = this.speed;
        }

        public void SetEntityData(NEntity entity)
        {
            this.position = this.position.FromNVector3(entity.Position);
            this.direction = this.direction.FromNVector3(entity.Direction);
            this.speed = entity.Speed;
        }
        //private void UpdateEntityData()
        //{
        //    entityData.Position.FromVector3Int(this.position);
        //    entityData.Direction.FromVector3Int(this.direction);
        //    entityData.Speed = this.speed;
        //}
    }
}
