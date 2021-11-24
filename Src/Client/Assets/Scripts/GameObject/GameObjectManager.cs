using Entities;
using Models;
using Services;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();//维护列表，创建角色//这个字典用的是 entityid,gameobject
    //bool ingame = false;
    // Use this for initialization
    protected override void OnStart()//要想GameObjectManager在切换地图时不被删除，必须做成单例类，要做成单例类必须要把start函数重载
    {
        //if (ingame)
        //{
        //    Destroy(this.gameObject);
        //}
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
        //ingame = true;
    }

    private void OnDestory()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);//有人进入地图，就加角色
    }
    void OnCharacterLeave(Character cha)
    {
        if (!Characters.ContainsKey(cha.entityId))
            return;
        if (Characters[cha.entityId] != null)
        {
            Destroy(Characters[cha.entityId]);
            this.Characters.Remove(cha.entityId);
        }
    }


    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;//一直在等待执行吗？等到characters里一有值就执行？
        }
       //这里具体是怎么运行的？去掉会有问题吗？
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.entityId)||Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if (obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
            Characters[character.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);

            InitGameObject(character, Characters[character.entityId]);

        }

        //UIMinimap.Instance.InitMap();//修改点，此位置不合适，位置需要变动！！在需要的时候！
        //已解决，通过setting 修改脚本运行顺序解决

    }

    private static void InitGameObject(Character character, GameObject go)
    {
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
        }

        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
            {
                User.Instance.CurrentCharacterObject = go;

                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;//不是玩家，则把playerinputcontroller脚本隐藏掉
            }
        }
    }

}
