using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services//必须改好，否则引用会出问题
{
    class StatusService : Singleton<StatusService>, IDisposable
    {
        //设计模式参照NPCManager
        public delegate bool StatusNotifyHandler(NStatus status);//为什么要定义一个bool型?

        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();

        public void Init()
        {


        }
        
        public void RegisterStatusNotify(StatusType function, StatusNotifyHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
        }

        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);

        }



        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }

        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach (NStatus status in notify.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            Debug.LogFormat("StatusNotify:[{0}][{1}]{2}:{3}", status.Type, status.Action, status.Id, status.Value);

            if (status.Type == StatusType.Money)//如果是钱，直接在User里改
            {
                if (status.Action == StatusAction.Add)
                {
                    User.Instance.AddGold(status.Value);
                }
                else if (status.Action == StatusAction.Delete)
                {
                    User.Instance.AddGold(-status.Value);
                }
            }
            //如果不是钱，看看是哪个通知到的系统注册了handler
            StatusNotifyHandler handler;
            if (eventMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}
