
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour
{
    public Text playerName;
    public Text playerLevel;
    // Use this for initialization
    void Start()
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




}
