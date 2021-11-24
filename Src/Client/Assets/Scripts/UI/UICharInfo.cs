using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
public class UICharInfo : MonoBehaviour {

	public NCharacterInfo info;
	public Text charClass;
	public Text charName;
	public Image[] charClassImages;
	
	public Image selectBg;
	// 点击角色后加上一个背景图片，代表选中该角色


	public void IsSelected(bool isselect)
    {
		selectBg.gameObject.SetActive(isselect);
    }



	// Use this for initialization
	void Start () {
        if (info!=null)
        {
			this.charClass.text = info.Class.ToString();
			this.charName.text = info.Name.ToString();
            for (int i = 0; i < 3; i++)
            {
				charClassImages[i].gameObject.SetActive((CharacterClass)(i+1) == info.Class);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
