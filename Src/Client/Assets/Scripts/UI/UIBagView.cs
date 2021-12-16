using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBagView : MonoBehaviour
{
    public GameObject[] bagPages;
    public UITabButton[] bagButtons;

    public int index = -1;
    // Use this for initialization
    IEnumerator Start()
    {
        for (int i = 0; i < bagButtons.Length; i++)//刚开始的时候就给每个按钮赋值，挂上相应的Index和对应的bagview
        {
            bagButtons[i].bagView = this;
            bagButtons[i].bagIndex = i;//自己会挂index，不用手动写
        }

        yield return new WaitForEndOfFrame();
        SelectTab(0);//率先选中page0
    }

    public void SelectTab(int index)
    {
        if (this.index != index)
        {
            for (int i = 0; i < bagButtons.Length; i++)
            {
                bagPages[i].SetActive(i == index);//scroll view是否调出
                bagButtons[i].Select(i == index);//button是否更换图片
            }
        }
    }





    // Update is called once per frame
    void Update()
    {

    }

}
