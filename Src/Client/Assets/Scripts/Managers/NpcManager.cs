using Common.Data;
using System;
using System.Collections.Generic;


namespace Managers
{
    public class NpcManager : Singleton<NpcManager>
    {

        public delegate bool NpcActionHandler(NpcDefine npc);//这里delegate bool 事件布尔型是双重属性？（需要看一下事件与委托的课程）

        Dictionary<NpcDefine.NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcDefine.NpcFunction, NpcActionHandler>();

        public void RegisterNpcEvent(NpcDefine.NpcFunction function, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
        }


        public NpcDefine GetNpcDefine(int npcId)
        {
            NpcDefine npc = null;
            DataManager.Instance.NPCs.TryGetValue(npcId, out npc);
            return npc;
            //return DataManager.Instance.NPCs[npcId];//舍弃这句是因为怕报错
        }

        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.NPCs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.NPCs[npcId];
                return Interactive(npc);
            }
            return false;
        }

        public bool Interactive(NpcDefine npc)
        {
            if (npc.Type == NpcDefine.NpcType.Task)
            {
                return DoTaskInteractive(npc);
            }
            else if (npc.Type == NpcDefine.NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }



        private bool DoTaskInteractive(NpcDefine npc)//任务NPC互动内容
        {
            MessageBox.Show("点击了NPC：" + npc.Name, "NPC对话");
            return true;
        }

        private bool DoFunctionInteractive(NpcDefine npc)//功能NPC互动内容
        {
            if (npc.Type != NpcDefine.NpcType.Functional)//有，如果npc同时有两种类型，就需要这个——有存在的必要吗？个人觉得之前好像已经剔除了
            {
                return false;
            }

            if (!eventMap.ContainsKey(npc.Function))
            {
                return false;
            }

            return eventMap[npc.Function](npc);//这个事件类型是bool型？为什么要求返回bool而他不报错？
        }
    }
}

