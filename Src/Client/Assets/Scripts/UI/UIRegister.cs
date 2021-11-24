﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour {

	public InputField username;
	public InputField password;
	public InputField passwordConfirm;
	public Button buttonRegister;

	// Use this for initialization
	void Start () {
		UserService.Instance.OnRegister = this.OnRegister;
	}



	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickRegister()
    {
        
		if (string.IsNullOrEmpty(this.username.text))
		{
			MessageBox.Show("请输入账号！");
			return;
		}
		if (string.IsNullOrEmpty(this.password.text))
		{
			MessageBox.Show("请输入密码！");
			return;
		}
		if (string.IsNullOrEmpty(this.passwordConfirm.text))
		{
			MessageBox.Show("请再次确认密码！");
			return;
		}
		if (this.password.text!=this.passwordConfirm.text)
		{
			MessageBox.Show("两次输入的密码不一致！");
			return;
		}

		UserService.Instance.SendRegister(this.username.text, this.password.text);


	}
	void OnRegister(Result result, string msg)
	{
		MessageBox.Show(string.Format("注册结果:{0} msg:{1}", result, msg));

	}




}
