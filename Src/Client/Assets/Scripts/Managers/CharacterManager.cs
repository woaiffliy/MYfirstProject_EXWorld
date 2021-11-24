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
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();//这个表改为TcharacterID维护，不能用entityID

        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;
        public void Init()
        {

        }
        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach (var key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
            EntityManager.Instance.Clear();//自己加的，目的是为了clear掉EntityManager里的数据11.18

        }

        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter:TCID{0}:{1} Map:{2} Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.Entity.String());
            Character character = new Character(cha);
            this.Characters[cha.Entity.Id] = character;//给改成entityid。11.23//这里是用TcharacterId来保存————有人进入地图，就加角色到角色列表中 //老师是用entityid，之后有问题再改。
            EntityManager.Instance.AddEntity(character);
            if (OnCharacterEnter != null)
            {
                OnCharacterEnter(character);//并创建角色的gameobject到地图中
            }
        }


        public void Dispose()
        {

        }

        internal void RemoveCharacter(int characterId)
        {
            Debug.LogFormat("RemoveCharacter:TcharacterID:{0}", characterId);//这里传回来的就是TcharacterID


            if (this.Characters.ContainsKey(characterId))
            {
                EntityManager.Instance.RemoveEntity(this.Characters[characterId].Info.Entity);
                if (OnCharacterLeave!=null)
                {
                    OnCharacterLeave(this.Characters[characterId]);//这个事件还需再看
                }
                this.Characters.Remove(characterId);

            }


        }
    }
}


