using Entities;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour {

	public Text playerName;
	public Text playerLevel;
	//public Image icon;//头像功能日后添加
	public Character character;
	public Camera camera;



	void Start () {
        if (this.character != null)
        {
			UpdateInfo();

		}
		
	}
	void Update()
    {
		UpdateInfo();
		//this.transform.LookAt(this.camera.transform);
		this.transform.forward = camera.transform.forward;
		

    }

	void UpdateInfo () {
        if (this.character != null)
        {
			string name = this.character.Name;
			string level = "Lv."+this.character.Info.Level.ToString();
            if (name != this.playerName.text||level != this.playerLevel.text)
            {
				this.playerName.text = name;
				this.playerLevel.text = level;
            }

        }
	}
}
