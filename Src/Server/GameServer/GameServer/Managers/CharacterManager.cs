using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using Managers;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public CharacterManager()
        {

        }

        public void Dispose()
        {

        }

        public void Init()
        {

        }

        public void Clear()
        {
            this.Characters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            
            EntityManager.Instance.AddEntity(cha.MapID, character);
            this.Characters[character.entityId] = character;//已经改为entityid————这里的ID用不用改成EntityID？
            return character;
        }
        
        public void RemoveCharacter(int entityId)
        {
            if (Characters.ContainsKey(entityId) == false)
            {
                return;//这句防止回到角色界面之后断开连接，导致报错
            }
            var cha = this.Characters[entityId];//这个characterId就是用的entityid
            EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);//已解决！答:互通！Character就是Entity的子类！————entity格式和character格式是可以互通的吗？为什么cha在这里不会报错？
            this.Characters.Remove(entityId); 
        }


    }
}
