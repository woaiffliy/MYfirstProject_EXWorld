using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using SkillBridge.Message;
using ProtoBuf;
using Services;
using Managers;

public class loadingManager : MonoBehaviour {


	public GameObject UITips;
	public GameObject UILoading;
	public GameObject UILogin;
	public GameObject UIRegister;

	public Slider progressBar;
	//public Text progressText;
	public Text progressNumber;



	// Use this for initialization 
	IEnumerator Start () {
		log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
		UnityLogger.Init();
		Common.Log.Init("Unity");
		Common.Log.Info("LoadingManager start");


		UITips.SetActive(true);
		UILoading.SetActive(false);
		UILogin.SetActive(false);
		UIRegister.SetActive(false);
		yield return new WaitForSeconds(2f);
		UILoading.SetActive(true);
		//yield return new WaitForSeconds(1f);
		UITips.SetActive(false);

		yield return new WaitForSeconds(0.5f);// yield return
		DataManager.Instance.Load();//载入数据
        UserService.Instance.Init();
		MapService.Instance.Init();
		NpcFunctionTestManager.Instance.Init();
		for (float i = 50; i < 100; )
        {
			i += Random.Range(0.1f, 1.5f);
            if (i>100) i = 100;
			progressBar.value = i/100;
			progressNumber.text = i.ToString("#0") + "%";
			yield return new WaitForEndOfFrame();
        }

		UILoading.SetActive(false);
		UILogin.SetActive(true);
		//yield return null;

	}
	

}
