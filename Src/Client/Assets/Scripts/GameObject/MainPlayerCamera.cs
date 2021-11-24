
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>//单例和unity的结合体
{
    public GameObject player;
    public Transform viewPoint;

    public new Camera camera;
    //单例删除Start函数，执行global条件防止切换场景被删除

    private void LateUpdate()
    {
        if (player == null)
        {
            player = User.Instance.CurrentCharacterObject;
        }
        if (player == null)
        {
            return;
        }
        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;
    }
}
