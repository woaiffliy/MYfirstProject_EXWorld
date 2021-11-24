//using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;
using Models;

public class UICharacterSelect : MonoBehaviour {


	public GameObject PanelCharCreate;
	public GameObject PanelCharSelect;


	public InputField inputCharName;
	CharacterClass charClass;



	public GameObject[] classLables;
	public Text describe;
    public List<GameObject> uiChars = new List<GameObject>();

    private int selectCharacterIdx = -1;

	public UICharacterView characterView;
    public GameObject charInfo;
    public Transform content;

    void Start()
    {
        OnSelectClass(2);//这个代码如果取消注释，43行代码不运行
        //DataManager.Instance.Load();//已解决——//本来应该是在登录时加载，现在临时写这里 DataManager预加载，之后要注释掉，不注释执行不了下面的代码
        //下边的代码不运行
        InitCharSelectPanel(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelectClass(int charClass)
    {
		this.charClass = (CharacterClass)(charClass+1);
		characterView.CurrentCharacter = charClass;
		
		for (int i = 0; i < 3; i++)
		{
			classLables[i].SetActive(i == charClass);
			describe.text = DataManager.Instance.Characters[charClass + 1].Description;
		}

		


	}


	public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.inputCharName.text))
        {
			MessageBox.Show("角色姓名不能为空！");
			return;
        }
		UserService.Instance.SendCharCreate(this.inputCharName.text, this.charClass);

    }
	 void OnCharacterCreate(Result result,string message)
    {
        if (result == Result.Success)
        {
            InitCharSelectPanel(true);
        }
        else
        {
			MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }

    public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {
            //MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
            UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }
    void InitCharSelectPanel(bool init)
    {
        PanelCharCreate.SetActive(false);
        PanelCharSelect.SetActive(true);
        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();//删除旧列表中所有角色资料
            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject obj = Instantiate(charInfo, content);
                UICharInfo cInfo = obj.GetComponent<UICharInfo>();
                cInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = obj.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                    
                });

                uiChars.Add(obj);
                obj.SetActive(true);

            }
        }
    }
    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        User.Instance.CurrentCharacter = cha;
        characterView.CurrentCharacter = (int)cha.Class-1;
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo cInfo = this.uiChars[i].GetComponent<UICharInfo>();
            cInfo.IsSelected(idx == i);
        }

    }

}

