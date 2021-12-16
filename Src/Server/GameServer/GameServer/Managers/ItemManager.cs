using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character Owner;

        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        public ItemManager(Character owner)
        {
            this.Owner = owner;
            foreach (var item in owner.Data.Items)//这个data.item似乎是db表Tcharacter所对应的Items，不是Bag
            {
                this.Items.Add(item.ItemID, new Item(item));
            }

        }

        public bool UseItem(int id,int count = 1)
        {

            //是Tid没错————注：这个owner的data的id理当是TID，至于这里到底是不是TID有待验证
            Log.InfoFormat("Owner.Data.ID:[{0}] UseItem [ItemID:{1} Count:{2}]", this.Owner.Data.ID, id, count);
            Item item = null;
            if (this.Items.TryGetValue(id, out item))
            {
                if (item.ItemCount < count)
                {
                    return false;
                }

                item.Remove(count);//是一个效果，并且害保存在了db上，【但是并没有上传给db】——这个和item.Count -= count是一个效果吗？
                return true;
            }
            return false;
        }

        public bool HasItem(int itemId)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                return item.ItemCount > 0;
            }
            return false;
        }

        public Item GetItem(int itemId)
        {
            Item item = null;
            this.Items.TryGetValue(itemId, out item);
            Log.InfoFormat("Owner.Data.ID:[{0}] GetItem [ItemID:{1} Count:{2}]", this.Owner.Data.ID, item.ItemID, item.ItemCount);
            return item;
        }

        public bool AddItem(int itemId,int count)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = new TCharacterItem();
                dbItem.TCharacterID = this.Owner.Data.ID; //
                dbItem.Owner = this.Owner.Data;
                dbItem.ItemID = itemId;
                dbItem.ItemCount = count; // 创造一个 TcharacterItem新物品（注意这是一个物品的表）的数据并赋值给Tcharacter

                Owner.Data.Items.Add(dbItem); //手动 把新物品的数据的表 加到 当前owner的数据库中

                item = new Item(dbItem);
                this.Items.Add(itemId, item); //给当前物品管理器的Owner对应的List添加该物品数据
            }

            this.Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Add);//状态管理器：物品添加

            Log.InfoFormat("Owner.Data.ID:[{0}] Owner.Id:[{1}] AddItem:[ItemID:{2} Count:{3}]", Owner.Data.ID,Owner.Id, itemId,count);
            //DBService.Instance.Save();//等修改，以后一次添加大量数据再进行保存———数据库保存
            return true;
        }

        public bool RemoveItem(int itemId,int count)
        {
            Item item = null;
            if (!this.Items.TryGetValue(itemId,out item))
            {
                return false;//没有该物品，就不删除
            }
            if (item.ItemCount < count)
            {
                return false;//该物品数量不足于remove的count，就不删除
            }
            item.Remove(count);

            this.Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Delete);//状态管理器：物品删除

            Log.InfoFormat("Owner.Data.ID:[{0}] Owner.Id:[{1}]  RemoveItem:[ItemID:{2} Count:{3}] ", this.Owner.Data.ID, Owner.Id, itemId, count);
            //DBService.Instance.Save();
            return true;

        }

        public void GetItemInfos(List<NItemInfo> list) //把Manager中内存中的items数据加到 List网络数据中，便于传送数据
        {
            foreach (var item in Items)
            {
                list.Add(new NItemInfo() { Id = item.Value.ItemID, Count = item.Value.ItemCount });
            }
        }


    }
}
