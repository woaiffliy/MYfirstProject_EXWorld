﻿using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
    public int ID;
    Mesh mesh = null;


    // Use this for initialization
    void Start()
    {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerController = other.GetComponent<PlayerInputController>();
        //Debug.Log("碰撞了");
        if (playerController != null && playerController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td == null)
            {
                Debug.LogErrorFormat("TeleporterObject: Character[{0} Enter Teleporter[{1}],But TeleporterDefine not existed", playerController.character.Info.Id, this.ID);
                return;
            }
            Debug.LogFormat("TeleporterObject: Character[{0} Enter Teleporter[{1}:{2}]", playerController.character.Info.Name, td.ID, td.Name);
            if (td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleport(this.ID);
                }
                else
                {
                    Debug.LogErrorFormat("Teleporter ID:{0} LinkID:{1} error", td.ID, td.LinkTo);
                }
            }

        }
    }
#if UNITY_EDITOR//编辑模式显示方向箭头
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh, this.transform.position + Vector3.up * this.transform.localScale.y * 10f, this.transform.rotation, this.transform.localScale);
       
        
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 2f, EventType.Repaint);
    }
#endif



}
