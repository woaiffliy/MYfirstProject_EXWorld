using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Item
    {
        TCharacterItem dbItem;

        public int ItemID;
        public int ItemCount;

        public Item(TCharacterItem item)
        {
            this.dbItem = item;
            this.ItemID = (short)item.ItemID;
            this.ItemCount = (short)item.ItemCount;
        }

        public void Add(int count)
        {
            this.ItemCount += count;
            dbItem.ItemCount = this.ItemCount;
        }

        public void Remove(int count)
        {
            this.ItemCount -= count;
            dbItem.ItemCount = this.ItemCount;//DB保存不应该写在这，应该写在DBservice里————好像并没有写db的保存？
        }

        public bool Use(int count = 1)//每次默认使用一个道具
        {

            //使用物品功能暂时没写
            return false;
        }


        public override string ToString()
        {
            return string.Format("ID:{0} , Count:{1}", this.ItemID, this.ItemCount);
        }

    }
}
