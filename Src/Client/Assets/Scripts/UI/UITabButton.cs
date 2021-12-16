using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITabButton : MonoBehaviour
{
    public Sprite selectedImage;
    private Sprite normalImage;
    public UIBagView bagView;
    public int bagIndex;

    private Image tabImage;
    // Use this for initialization
    void Start()
    {
        tabImage = this.GetComponent<Image>();
        normalImage = tabImage.sprite;

        this.GetComponent<Button>().onClick.AddListener(OnClick);//给button的点击事件额外添加监听事件
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void Select(bool isSelected)//给button更换图片
    {
        tabImage.overrideSprite = isSelected ? selectedImage : normalImage; //三元式
    }

    void OnClick()
    {
        this.bagView.SelectTab(this.bagIndex);
    }

}
