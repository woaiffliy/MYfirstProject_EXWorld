using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;
using Managers;

namespace Services
{

    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();//entityID存的

        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;
        public void Init()
        {

        }
        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach (var key in keys)//key是entityid
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
            EntityManager.Instance.Clear();//自己加的，目的是为了clear掉EntityManager里的数据11.18

        }

        public void AddCharacter(NCharacterInfo cha)//character.info.Id 确实是TcharacterID
        {
            Debug.LogFormat("AddCharacter: TcharacterID: {0} EntityID: {1} Name: {2} Map:{3} Entity:{4}", cha.Id,cha.Entity.Id, cha.Name, cha.mapId, cha.Entity.String());
            Character character = new Character(cha);
            this.Characters[cha.Entity.Id] = character;//给改成entityid。11.23
            EntityManager.Instance.AddEntity(character);
            if (OnCharacterEnter != null)
            {
                OnCharacterEnter(character);//并创建角色的gameobject到地图中
            }
        }


        public void Dispose()
        {

        }

        internal void RemoveCharacter(int entityID)
        {
            Debug.LogFormat("RemoveCharacter:EntityID:{0}", entityID);//这里传回来的就是entityID


            if (this.Characters.ContainsKey(entityID))
            {
                EntityManager.Instance.RemoveEntity(this.Characters[entityID].Info.Entity);
                if (OnCharacterLeave!=null)
                {
                    OnCharacterLeave(this.Characters[entityID]);//这个事件还需再看
                }
                this.Characters.Remove(entityID);

            }


        }
    }
}


