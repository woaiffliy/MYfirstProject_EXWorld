using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class UIBag:UIWindow
    {
        public Text money;//金钱系统之后再做

        public Transform[] pages;

        public GameObject bagItem;//代表一个格子

        List<Image> slots;//给格子的图片集


        bool isStarted = false;
        Dictionary<int,GameObject> ItemGameObjects = new Dictionary<int, GameObject>();

        void OnEnable()
        {
            if (isStarted)
            {
                BagReset();
            }
        }

        void Start()
        {
            if (slots == null)
            {
                slots = new List<Image>();
                for (int page = 0; page < this.pages.Length; page++)
                {
                    slots.AddRange(this.pages[page].GetComponentsInChildren<Image>(true));
                }
            }
            isStarted = true;
            StartCoroutine(InitBags());
        }

        IEnumerator InitBags()//为什么用协程？以后再说
        {
            BagReset();

            yield return null;
        }
        
        public void SetTitle(string title)
        {
            
        }
        public void OnReset()
        {
            BagManager.Instance.Reset();//
            //还要再加刷新背包UI逻辑
            BagReset();
        }
        public void BagReset()
        {
            for (int i = 0; i < BagManager.Instance.Items.Length; i++)
            {
                var item = BagManager.Instance.Items[i];
                if (item.ItemId > 0)
                {
                    GameObject go = null;
                    if (!ItemGameObjects.TryGetValue(i,out go))//这里会赋值吗？
                    {
                        go = Instantiate(bagItem, slots[i].transform);
                        ItemGameObjects.Add(i, go);
                    }
                    
                    var ui = go.GetComponent<UIBagIconItem>();
                    var def = ItemManager.Instance.Items[item.ItemId].Define;
                    ui.SetMainIcon(def.Icon, item.Count.ToString());
                }
            }
            for (int i = BagManager.Instance.Items.Length; i < slots.Count; i++)//锁住的格子都为灰色
            {
                slots[i].color = Color.gray;
            }

            this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
        }
    }
}
