
using Assets.Scripts.UI;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public Text playerName;
    public Text playerLevel;
    // Use this for initialization
    protected override void OnStart()
    {
        this.UpdateAvatar();
    }

    // Update is called once per frame
    void UpdateAvatar()
    {
        this.playerName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.playerLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }

    public void OnClickUITest()
    {
        UITest test =  UIManager.Instance.Show<UITest>();
        test.SetTitle("这是一个测试UI");
        test.OnClose += Test_OnCLlose;//这句涉及调用事件，还需要再看一下
    }

    private void Test_OnCLlose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的：" + result, "对话框响应结果", MessageBoxType.Information);
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
        //test.SetTitle("这是背包UI");
        //test.OnClose += Test_OnCLlose;//这句涉及调用事件，还需要再看一下
    }
}
