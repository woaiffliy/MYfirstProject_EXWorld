using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text title;


    public void SetTitle(string ti)
    {
        title.text = ti;
    }
}
