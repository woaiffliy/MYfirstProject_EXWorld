using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;
namespace GameServer.Managers
{
    class MapManager : Singleton<MapManager>
    {
        Dictionary<int, Map> Maps = new Dictionary<int, Map>();

        


        public void Init()
        {
            foreach (var mapdefine in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(mapdefine);
                Log.InfoFormat("MapManager.Init > Map:{0}:{1}", map.Define.ID, map.Define.Name);//更新了地图名字之后输出地图名字应该是更新之后的，但还是更新之前的名字，需要调查一下看看是哪的问题。目前推测：1.有内存没清除，2.配置表没有复制给服务端（老师并没有进行过这个步骤）
                this.Maps[mapdefine.ID] = map;
            }
        }





        public Map this[int key]
        {
            get
            {
                return this.Maps[key];
            }
        }


        public void Update()//将来刷怪要用到，地图要刷新，是地图管理器的一个自主服务，其他管理器没有
        {
            foreach (var map in this.Maps.Values)
            {
                map.Update();
            }
        }
    }
}
