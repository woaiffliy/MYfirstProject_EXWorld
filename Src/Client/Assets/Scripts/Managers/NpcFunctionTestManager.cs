using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class NpcFunctionTestManager:Singleton<NpcFunctionTestManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcDefine.NpcFunction.InvokeInstance, OnNpcInvokeInstance);
            //NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcDefine.NpcFunction.InvokeShop, OnNpcInvokeShop);
        }

        //private bool OnNpcInvokeShop(NpcDefine npc)
        //{
        //    Debug.LogFormat("NpcFunctionTestManager.OnNpcInvokeShop: NPC:[{0} :{1}] Func:{2}", npc.ID, npc.Name, npc.Function);
        //    UITest test = UIManager.Instance.Show<UITest>();
        //    test.SetTitle(npc.Name);
        //    return true;
        //}

        private bool OnNpcInvokeInstance(NpcDefine npc)
        {
            Debug.LogFormat("NpcFunctionTestManager.OnNpcInvokeInstance: NPC:[{0} :{1}] Func:{2}", npc.ID, npc.Name, npc.Function);
            MessageBox.Show("点击了NPC: " + npc.Name, "NPC对话");
            return true;
        }
    }
}
