using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Services;
using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using UnityEngine;

namespace Managers
{
    class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        public void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id, item);

                Debug.LogFormat("ItemManager:Init[{0}]", item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);

        }



        public ItemDefine GetItem(int itemId)//得到一个物品，1 应该在背包里显示，2 应该提交消息到服务器，在玩家的db里添加道具
        {
            return null;
        }

        private bool OnItemNotify(NStatus status)//因为StatusNotifyHandler是bool型，具体用处未知————为啥是bool型
        {
            if (status.Action == StatusAction.Add)
            {
                this.AddItem(status.Id, status.Value);
            }
            if (status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
            return true;
        }


        void AddItem(int id, int value)
        {
            Item item = null;
            if (this.Items.TryGetValue(id, out item))
            {
                item.Count += value;
            }
            else
            {
                item = new Item(id, value);
                this.Items.Add(id, item);
            }

            BagManager.Instance.AddItem(id, value);

        }
        void RemoveItem(int id, int value)
        {
            if (!this.Items.ContainsKey(id))
            {
                return;
            }
            Item item = this.Items[id];
            if (item.Count < value)
            {
                return;
            }
            item.Count -= value;

            BagManager.Instance.RemoveItem(id, value);


        }



        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }


    }
}
