﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{

    public delegate void CloseHandler(UIWindow sender, WindowResult result);

    public event CloseHandler OnClose;//closehandler 这个函数需要了解，事件委托也需要了解

    public virtual System.Type Type { get { return this.GetType(); } }//这个还不是很理解

    public enum WindowResult
    {
        None = 0,
        Yes,
        No
    }

    public void Close(WindowResult result = WindowResult.None)
    {
        UIManager.Instance.Close(this.Type);
        if (this.OnClose!=null)
        {
            this.OnClose(this,result);
        }
        this.OnClose = null;
    
    }

    public virtual void OnCloseClick()
    {
        this.Close();
    }

    public virtual void OnYesClick()
    {
        this.Close(WindowResult.Yes);
    }

    //void OnMouseDown()
    //{
    //    Debug.LogFormat(this.name + "Clicked");
    //}

}

