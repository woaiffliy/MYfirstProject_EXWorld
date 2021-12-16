using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;

        //public Item(NItemInfo item)//每次创建变量 从 服务器取值，故变量为 NItemInfo（是在协议里命名的）  若是db取值，则为 TCharacterItem（是数据库的名字）
        //{
        //    this.Id = item.Id;
        //    this.Count = item.Count;
        //    this.Define = DataManager.Instance.Items[item.Id];
        //}//改造后：——————>>>构造并重载（具体什么写法还得再看看）（这似乎是简写）

        public Item(NItemInfo item):
            this(item.Id, item.Count)
        {
        }

        public Item(int id, int count)
        {
            this.Id = id;
            this.Count = count;
            this.Define = DataManager.Instance.Items[this.Id];
        }



        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}", this.Id, this.Count);
        }

    }
}
