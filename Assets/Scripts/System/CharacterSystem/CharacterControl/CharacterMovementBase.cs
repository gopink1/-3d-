using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHash
{
    public const string HasInputHash = "HasInput";
    public const string RunHash = "Run";
    public const string MovementHash = "Movement";
    public const string LockHash = "Lock";
    public const string HorizontalHash = "Horizontal";
    public const string VerticalHash = "Vertical";
    public const string RoolMoveSpeed = "RoolMoveSpeed";
}
public class CharacterMovementBase : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController charactercontroller;

    protected Vector3 moveDirection;

    //地面检测
    protected bool isGround;//是否在地面
    [SerializeField, Header("位置偏移量")] protected float DetectionPositionOffset;//位置偏移量
    [SerializeField, Header("检测范围")] protected float DetectionRange;//检测的偏移量
    [SerializeField, Header("层级")] protected LayerMask WhatisGround;//确定地面的层级


    //重力
    private float characterGravity = -9.8f;//重力
    private float characterVerticalVelocity;//竖直方向上的速度
    private float fallOutDalteTime;//下落延迟的时间的判定，用于下楼梯时候检测落地的时间如果小于阈值则不会触发下落的动画
    private float fallOutTime = 0.15f;//下落延迟的时间
    private float maxVerticalVelocity = 54f;
    private Vector3 characterVerticalDirection;


    //是否启动重力
    private bool isEnableGravity;




    //是否启用跟运动
    protected bool isApplyRootMotion;

    protected virtual void Start()
    {
        fallOutDalteTime = fallOutTime;
        isEnableGravity = true;
        isApplyRootMotion = true;

    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        charactercontroller = GetComponent<CharacterController>();
    }
    protected virtual void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListening<float>(EventHash.ChangeCharacterVerticalVelocity, ChangeCharacterVerticalVelocity);
        GameEventManager.MainInstance.AddEventListening<bool>(EventHash.EnableGravity, EnableGravity);
    }
    protected virtual void Update()
    {
        SetGravity();
        UpdateVerticalVelocity();
    }
    protected virtual void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<float>(EventHash.ChangeCharacterVerticalVelocity, ChangeCharacterVerticalVelocity);
        GameEventManager.MainInstance.RemoveEvent<bool>(EventHash.EnableGravity, EnableGravity);
    }
    /// <summary>
    /// 启用RootMotion的角色移动处理
    /// </summary>
    protected virtual void OnAnimatorMove()
    {
        animator.ApplyBuiltinRootMotion();
        UpdateCharacterMoveDirection(animator.deltaPosition);
    }
    private bool CheckIsGround()
    {
        Vector3 DetectionPosition = new Vector3(transform.position.x,
            transform.position.y - DetectionPositionOffset, transform.position.z);//偏移后的检测中心点
        return Physics.CheckSphere(DetectionPosition, //位置
            DetectionRange, //范围
            WhatisGround, //层级
            QueryTriggerInteraction.Ignore);//忽略触发器
    }

    private void SetGravity()
    {
        isGround = CheckIsGround();
        if (isGround)
        {
            //在地面上
            fallOutDalteTime = fallOutTime;//下落检测的计时归零
            //Debug.Log(fallOutDalteTime);
            if (characterVerticalVelocity<0)
            {
                characterVerticalVelocity = -2f;//速度变为-2这样保证下次的跳跃的速度-2
                //Debug.Log(characterVerticalVelocity);
            }
        }
        else
        {
            if (fallOutDalteTime > 0)
            {
                fallOutDalteTime -= Time.deltaTime;
            }
            else
            {
                //当过了阈值还落未到地面，触发动画或者其他
            }
            //没有在地面
            if (characterVerticalVelocity < maxVerticalVelocity && isEnableGravity)//施加重力不断的给向下的速度
            {
                characterVerticalVelocity += characterGravity * Time.deltaTime;
            }
        }
    }

    private void UpdateVerticalVelocity()//更新垂直方向上速度
    {
        if (!charactercontroller.enabled) return;
        if (isEnableGravity)
        {
            characterVerticalDirection.Set(0, characterVerticalVelocity, 0);
            charactercontroller.Move(characterVerticalDirection  * Time.deltaTime);
        }
    }

    //坡道检测
    //检测是否在坡道上
    private Vector3 SlopResetDirection(Vector3 moveDirection)
    {
        if (!charactercontroller.enabled) return moveDirection;
        if (Physics.Raycast(transform.transform.position + (transform.up * .5f),//射线检测的初始值
            Vector3.down,//方向
            out var hit,//返回值
            charactercontroller.height * .85f,//最大距离
            WhatisGround,//检测物体
            QueryTriggerInteraction.Ignore)//忽略触发器
            )
        {
            if (Vector3.Dot(Vector3.up, hit.normal) != 0)//两者点乘部位零说明不垂直，说明在坡道
            {
                return Vector3.ProjectOnPlane(moveDirection, hit.normal);
            }
        }
        return moveDirection;
    }

    protected virtual void UpdateCharacterMoveDirection(Vector3 moveDir)
    {
        if (!charactercontroller.enabled) return;
        moveDirection = SlopResetDirection(moveDir);
        charactercontroller.Move(moveDirection * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Vector3 DetectionPosition = new Vector3(transform.position.x,
          transform.position.y - DetectionPositionOffset, transform.position.z);//偏移后的检测中心点
        Gizmos.DrawWireSphere(DetectionPosition, DetectionRange);
    }

    //事件注册
    private void ChangeCharacterVerticalVelocity(float v)
    {
        characterVerticalVelocity = v;
    }

    private void EnableGravity(bool enable)
    {
        isEnableGravity = enable;
    }


}
