using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UILogin : MonoBehaviour {

	public InputField username;
	public InputField password;
	public Button buttonLogin;


	// Use this for initialization
	void Start () {
		UserService.Instance.OnLogin = this.OnLogin;
	}
	void Update () {
		
	}
	
	

	public void OnClickLogin()
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
		//if (string.IsNullOrEmpty(this.passwordConfirm.text))
		//{
		//	MessageBox.Show("请再次确认密码！");
		//	return;
		//}
		//if (this.password.text != this.passwordConfirm.text)
		//{
		//	MessageBox.Show("两次输入的密码不一致！");
		//	return;
		//}

		UserService.Instance.SendLogin(this.username.text, this.password.text);

	}

	void OnLogin(Result result, string msg)
	{
        if (result == Result.Success)
        {
			//登录成功
			SceneManager.Instance.LoadScene("CharSelect");
        }
        else
        {
			MessageBox.Show(msg,"错误",MessageBoxType.Error);

		}
		//MessageBox.Show(string.Format("登录结果:{0} msg:{1}", result, msg));
	}

	// Update is called once per frame

}
