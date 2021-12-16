using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Models
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    struct BagItem
    {
        public ushort ItemId;
        public ushort Count;

        public static BagItem zero = new BagItem { ItemId = 0, Count = 0 };

        public BagItem(int itemId, int count)
        {
            this.ItemId = (ushort)itemId;
            this.Count = (ushort)count;
        }


        public static bool operator ==(BagItem lhs, BagItem rhs)//以下方法是啥暂时不知，据说与背包物品交换位置有关，以后再说
        {
            return lhs.ItemId == rhs.ItemId && lhs.Count == rhs.Count;  
        }

        public static bool operator !=(BagItem lhs, BagItem rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)//应该是物品1与物品2格子交换，结果为物品1占据物品2格子
        {
            if (other is BagItem)
            {
                return Equals((BagItem)other);
            }
            return false;
        }

        public bool Equals(BagItem other)//应该是物品与无物品格子交换，结果为该物品占据该格子
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return ItemId.GetHashCode() ^ (Count.GetHashCode() << 2);//这啥意思？
        }


    }
}
