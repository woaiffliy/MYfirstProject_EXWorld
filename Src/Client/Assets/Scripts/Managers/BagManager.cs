using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class BagManager : Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;

        NBagInfo Info;

        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(Info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        public void Reset()//背包自动整理
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;

                }
                i++;
            }
        }

        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }

        public void AddItem(int itemId, int count)
        {
            ushort addCount = (ushort)count;

            for (int i = 0; i < Items.Length; i++) //Items长度为Unlocked
            {//先遍历整个背包，看看有相同物品的能加多少加多少，
                if (this.Items[i].ItemId == itemId)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - this.Items[i].Count);
                    if (canAdd >= addCount)//如果物品最大堆叠数量-现有数量>欲添加数量
                    {
                        this.Items[i].Count += addCount;//直接加上去
                        addCount = 0;
                        //break;//换成return ?
                        return;
                    }
                    else
                    {
                        this.Items[i].Count += canAdd;//直接先加满
                        addCount -= canAdd;//把超出堆叠数量的数量保存下来
                    }
                }

            }
            if (addCount > 0)//如果超出堆叠数量仍大于0
            {//剩下的再找无物品的格子（但是剩下的也有可能超过StackLimit，所以逻辑还需另作）
                for (int i = 0; i < Items.Length; i++)
                {
                    if (this.Items[i].ItemId == 0)//找一个无物品的格子
                    {
                        this.Items[i].ItemId = (ushort)itemId;
                        this.Items[i].Count = addCount;//加上去
                        addCount = 0;
                        break;//不加break的后果就是，如果买了最大堆叠数量是1 的物品，则空闲背包全会被该物品占领（虽然DB和服务器那里不会加就是了）
                              //return;
                    }
                }
            }


            //物品栏已满，无法购买 通知还没做
        }

        public void RemoveItem(int itemId, int count)
        {
            //售出、丢弃物品、以后再做
        }

    }
}
