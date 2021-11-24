using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
public class UIMinimap : MonoBehaviour//MonoSingleton<UIMinimap>//发生了改动：因为此脚本的start函数比GameobjectManager脚本的创建角色gameobject函数执行的早，所以这里改为单例，将InitMap方法公用，并在GameobjectManager脚本末尾执行该单例
{

    public Collider MinimapBox;
    public Image arrow;
    public Image minimap;
    public Text mapName;

    private Transform playerTransform;



    // Use this for initialization
    void Start()
    {
        this.InitMap();
    }

    public void InitMap()
    {
        this.mapName.text = User.Instance.CurrentMap.Name;
        if (this.minimap.overrideSprite == null)
        {
            this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        }

        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;

            



        //现在的问题：CurrentCharacterObject在角色进入地图时丢失，原因：进入地图时删除了一次角色的gameobject，线索是EntityController的OnDestory被执行
        //明天任务：找到被执行的原因，是否有必要执行，如无必要，想办法避免角色在进入地图时被直接删除然后重新创建
        //线索：总觉得是跟Character表有关系，当Character表清空的时候，角色会被删除吗？2. minimap.cs和gameobjectmanager.cs这两个到底谁先执行谁后执行？会影响资源导入吗？
        //已解决，原因是userservice里有一段不需要的代码，具体什么时间老师删掉了，再看
        //关于start执行顺序的问题也已经解决， 通过修改脚本执行顺序解决。
    }


    // Update is called once per frame
    void Update()
    {
        if (playerTransform==null)
        {
            this.playerTransform = MinimapManager.Instance.PlayerTransform;
        }
        


        if (this.MinimapBox == null || playerTransform == null) return;//防止切换地图时 组件删除的先后顺序导致报空异常

        float realWidth = this.MinimapBox.bounds.size.x;
        float realHeight = this.MinimapBox.bounds.size.z;

        float relaX = this.playerTransform.position.x - this.MinimapBox.bounds.min.x;
        float relaY = this.playerTransform.position.z - this.MinimapBox.bounds.min.z;

        float pivotX = relaX / realWidth;//中心点这个不是很理解，再看看（第十三课1小时8分）
        float pivotY = relaY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }
}
