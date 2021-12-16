using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using System;
using Models;

public class NpcController : MonoBehaviour
{
    public int NpcId;

    SkinnedMeshRenderer render;
    Color orignColor;
    Animator anim;
    NpcDefine npc;

    private bool inInteractive = false;
    // Use this for initialization
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        npc = NpcManager.Instance.GetNpcDefine(NpcId);
        render = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        orignColor = render.sharedMaterial.color;
        this.StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));

            }
            this.Relax();//时不时放松
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void Relax()
    {
        anim.SetTrigger("RELAX");
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("TALK");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }



    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }
    private void OnMouseEnter()
    {
        Highlight(true);
    }
    private void OnMouseExit()
    {
        Highlight(false);
    }


    void Highlight(bool highlight)//选中高亮
    {
        if (highlight)
        {
            if (render.sharedMaterial.color != Color.white)
            {
                render.sharedMaterial.color = Color.white;
            }
        }
        else
        {
            if (render.sharedMaterial.color != orignColor)
            {
                render.sharedMaterial.color = orignColor;
            }
        }

    }



}
