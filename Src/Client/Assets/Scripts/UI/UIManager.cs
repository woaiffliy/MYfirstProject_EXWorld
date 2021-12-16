using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    class UIElement
    {
        public string Resources;
        public bool Cache;
        public GameObject Instance;


    }

    private Dictionary<Type, UIElement> UIResouces = new Dictionary<Type, UIElement>();

    public UIManager()
    {
        this.UIResouces.Add(typeof(UITest), new UIElement() { Resources = "UI/UITest", Cache = true });
        this.UIResouces.Add(typeof(UIBag), new UIElement() { Resources = "UI/UIBag", Cache = true });
        this.UIResouces.Add(typeof(UIShop), new UIElement() { Resources = "UI/UIShop", Cache = true });
    }

    ~UIManager()//这是干啥的？
    {

    }

    public T Show<T>()
    {
        //SoundManager.Instance.PlaySound("open");//音乐管理器暂时未启用

        Type type = typeof(T);
        if (this.UIResouces.ContainsKey(type))
        {
            UIElement info = this.UIResouces[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);

            }
            else
            {
                UnityEngine.Object perfab = Resources.Load(info.Resources);
                if (perfab == null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(perfab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }


    public void Close(Type type)
    {
        //SoundManager.Instance.PlaySound("close");//音乐管理器暂时未启用

        if (this.UIResouces.ContainsKey(type))
        {
            UIElement info = UIResouces[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }


    }


}
