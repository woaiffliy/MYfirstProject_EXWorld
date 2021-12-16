using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;

namespace GameServer.Services
{
    class DBService : Singleton<DBService>
    {
        ExtremeWorldEntities entities;

        public ExtremeWorldEntities Entities
        {
            get { return this.entities; }
        }

        public void Init()
        {
            entities = new ExtremeWorldEntities();
        }


        //float time = 10;
        public void Save()
        {
            //保存需要有间隔时间，间隔时间越长，服务器压力越小，但是回档的间隔就越长，容错低。看你自己对于服务器性能和容错的选择了
            entities.SaveChanges();//不过目前还没有做保存时间调整——异步保存，边保存边继续游戏的逻辑，不耽误事，
        }


    }
}
