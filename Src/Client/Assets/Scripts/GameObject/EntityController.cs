using Entities;
using SkillBridge.Message;
using UnityEngine;
using Managers;
public class EntityController : MonoBehaviour,IEntityNotify
{


    public Animator anim;
    public Rigidbody rb;
   // private AnimatorStateInfo currentBaseState;

    public Entity entity;

    public Vector3 direction;
    public Vector3 position;
    public Vector3 lastPosition;
    Quaternion rotation;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;
    public bool isPlayer = false;








    // Use this for initialization
    void Start()
    {
        if (entity != null)
        {
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);//这里还得再看看
            this.UpdateTransform();
        }
        if (!this.isPlayer)
        {
            rb.useGravity = false;
        }

    }

    // Update is called once per frame
    void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;

    }

    void OnDestroy()
    {
        if (entity != null)
        {
            Debug.LogFormat("{0} OnDestroy: entityID:{1} POS:{2} DIR:{3} SPD:{4}", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
        }
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
    }
    void FixedUpdate()
    {
        if (this.entity == null)
        {
            return;
        }
        this.entity.OnUpdate(Time.fixedDeltaTime);
        if (!this.isPlayer)//一直刷新其他玩家动向
        {
            this.UpdateTransform();
        }
    }



    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch (entityEvent)//动画状态机
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                //Debug.LogFormat("idle了");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                //Debug.LogFormat("move了");
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                //Debug.LogFormat("move了");
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                //Debug.LogFormat("jump了");
                break;
        }
    }
    
    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
        Destroy(this.gameObject);
    }

    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged : ID:{0} POS:{1} DIR:{2} SPD:{3} ", entity.entityId,entity.position, entity.direction, entity.speed);
    }


}
