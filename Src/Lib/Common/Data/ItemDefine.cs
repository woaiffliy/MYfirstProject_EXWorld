using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class ItemDefine
    {
        //public enum ItemType//这个定义放协议里了，因为要用网络传输这个数据
        //{
        //    NORMAL = 0,
        //    METARIAL = 1,
        //    EQUIPMENT = 2,
        //}
        public enum ItemFunction
        {
            None = 0,
            RecoverHP = 1,
            RecoverMP = 2,
            AddMoney = 3,
            AddItem = 4,
            Teleport = 5,
            AddBuff = 6,
            AddExp = 7,
        }



        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public string Category { get; set; }
        public bool CanUse { get; set; }
        public float UseCD { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public ItemFunction Function { get; set; }
        public int Param { get; set; }
        public List<int> Params { get; set; }


        public int StackLimit { get; set; }
        public string Icon { get; set; }
    }
}
