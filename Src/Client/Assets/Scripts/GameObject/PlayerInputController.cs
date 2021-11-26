using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Entities;
using Services;

public class PlayerInputController : MonoBehaviour
{

    public Rigidbody rb;
    CharacterState state;
    public Character character;

    public float rotateSpeed = 2.0f;

    public float turnAngle = 10;

    public int speed;

    public EntityController entityController;

    //public bool onAir = false;


    Vector3 lastPos;
    //float lastSync = 0;





    void Start()
    {

        state = CharacterState.Idle;
        if (this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;

            //测试代码
        }





    }


    void FixedUpdate()
    {
        if (character == null)
        {
            return;
        }

        float v = Input.GetAxis("Vertical");
        if (v > 0.01)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);

            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
            //移动速度优化算法（不知道为什么这样写，以后再看）
        }
        else if (v < -0.01)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            if (state != CharacterState.Idle)
            {
                state = CharacterState.Idle;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
            this.rb.velocity = Vector3.zero;
        }

        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }

        float h = Input.GetAxis("Horizontal");
        if (h<-0.1||h>0.1)
        {//旋转速度优化算法（不知道为什么这样写，以后再看）
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y>this.turnAngle&&rot.eulerAngles.y<(360-this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }
        }





    }

    void OnDestroy()
    {



    }


    private void LateUpdate()
    {
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        this.lastPos = this.rb.transform.position;
        if (character == null)
        {
            return;
        }
        if ((GameObjectTool.WorldToLogic(this.rb.transform.position)-this.character.position).magnitude>10)//这是老师用来做位置同步用的
            //还有一个问题，这个if条件里的数字原本是50，但是50的时候要走很远距离才能触发从而报告坐标，而50在老师的项目中并不会走很远，怀疑有地方有问题，暂时按下不表
            //已解决，是因为场景中多放了一个角色perfab——因为Character为空，为空的原因是因为start的一串代码，可能之后的课有讲，先把课听完
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;

    }

    void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
        {
            entityController.OnEntityEvent(entityEvent);
        }
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData);
    }
}
