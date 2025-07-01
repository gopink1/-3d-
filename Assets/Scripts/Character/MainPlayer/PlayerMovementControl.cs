using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : CharacterMovementBase
{
    [SerializeField]private PlayerAttr m_attr;

    #region 角色移动和转向
    private Camera m_Camera;
    private float _rotateAngle;//转向角度

    private float currentVelocity ;

    [SerializeField] private float smoothTime;

    protected override void Start()
    {
        base.Start();
        m_attr = GetComponent<PlayerHealthyControl>().GetAttr();
    }

    private void CharacterRotateControl()
    {
        if (animator.GetBool(AnimationHash.HasInputHash))
        {
            _rotateAngle = Mathf.Atan2(GameInputManager.MainInstance.Movement.x,
           GameInputManager.MainInstance.Movement.y) * Mathf.Rad2Deg +
           m_Camera.transform.eulerAngles.y;  //角色输入的旋转
        }
        if (animator.GetBool(AnimationHash.HasInputHash) && (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion")||animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
            _rotateAngle,
            ref currentVelocity,
            smoothTime);
        }
    }

    /// <summary>
    /// 根据输入更新动画
    /// </summary>
    private void UpdateAnimation()
    {
        if (!isGround) return;

        animator.SetBool(AnimationHash.HasInputHash, GameInputManager.MainInstance.Movement != Vector2.zero);
        if (animator.GetBool(AnimationHash.HasInputHash))
        {
            animator.SetBool(AnimationHash.RunHash, GameInputManager.MainInstance.Run);
            animator.SetFloat(AnimationHash.MovementHash,
            animator.GetBool(AnimationHash.RunHash) ? 2f : GameInputManager.MainInstance.Movement.sqrMagnitude,
                    .25f,
                    Time.deltaTime);

            animator.SetFloat(AnimationHash.HorizontalHash,
                        0f,
                        .25f,
                        Time.deltaTime);
            animator.SetFloat(AnimationHash.VerticalHash,
                        0f,
                        .25f,
                        Time.deltaTime);
        }
        else
        {
            animator.SetFloat(AnimationHash.MovementHash, 0,
            .25f,
            Time.deltaTime);
            if (animator.GetFloat(AnimationHash.MovementHash) < 0.2f)
            {
                animator.SetBool(AnimationHash.RunHash, false);
            }
        }

    }

    protected override void OnAnimatorMove()
    {
        if (!isApplyRootMotion) return;
        animator.ApplyBuiltinRootMotion();
        UpdateCharacterMoveDirection(animator.deltaPosition * m_attr.Speed);
    }
    #endregion

    #region 角色闪避
    //角色闪避相关参数
    private float timer;
    [SerializeField] private float roolMoveSpeed;

    /// <summary>
    /// 角色闪避相关逻辑
    /// </summary>
    private void PlayerDodgeUpdate()
    {
        //重置冷却事件，如果未处于闪避
        ResetRoolTimer();
        //角色闪避的触发
        //当按下闪避后会触发
        if (!animator) return;
        if (!CanDodge()) return;
        //未锁定
        if (GameInputManager.MainInstance.Dodge)
        {
            animator.Play("Dodge");
            ////然后可以根据计时器也就可以根据动画事件设置回复根运动
            TimerManager.MainInstance.TryEnableOneGameTimer(0.75f, OnRoolEnd);
            RemoveRootMotion();
        }
    }
    private bool CanDodge()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Finality")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Assassinate")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Climb")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge")) return false;
        if(animator.IsInTransition(0)) return false;

        if (timer != 0f) return false;
        return true;
    }

    private void UpdateDodgeMoveDir()
    {
        //分为两种情况锁定和没有锁定
        //未锁定仅需简单的向前翻滚
        //锁定需要对输入方向和摄像机的朝向进行判断对其进行位移

        //移动的距离通过曲线来控制速度
        //调用UpdateCharacterMoveDirection函数进行方向上位移传入的内容可以控制速度
        if ((animator.GetFloat(AnimationHash.LockHash) == 0))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge"))
            {
                timer += Time.deltaTime;
                //Debug.Log(timer);
                //未锁定状态
                //roolMoveSpeed = animator.GetFloat(AnimationHash.RoolMoveSpeed);

                UpdateCharacterMoveDirection(transform.forward  * roolMoveSpeed * 2.5f * animator.GetFloat("DodgeOffset"));
            }
        }
        else if ((animator.GetFloat(AnimationHash.LockHash) == 1))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge") && !animator.IsInTransition(0))
            {
                //未锁定状态
                timer += Time.deltaTime;
                //根据输入进行动画播放判断
                //根据摄像机和输入判断位移方向
                Vector3 dir = new Vector3(animator.GetFloat(AnimationHash.HorizontalHash), 0, animator.GetFloat(AnimationHash.VerticalHash));//获取的是世界坐标的方向
                //需要把方向旋转
                Quaternion rotation = Quaternion.Euler(0, _rotateAngle, 0);
                dir = rotation * dir;
                UpdateCharacterMoveDirection(dir * 6f);
            }
        }


    }

    private void ResetRoolTimer()
    {
        if (timer == 0) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge"))
        {
            timer = 0f;
        }
    }

    /// <summary>
    /// 禁用根运动
    /// </summary>
    private void RemoveRootMotion()
    {
        isApplyRootMotion = false;
        animator.applyRootMotion = false;
    }
    /// <summary>
    /// 启用根运动
    /// </summary>
    private void OnRoolEnd()
    {
        isApplyRootMotion = true;
        animator.applyRootMotion = true;
        timer = 0f;
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        //m_Camera = Camera.main;
        m_Camera = GameObject.FindWithTag("PlayerCamera").transform.GetComponent<Camera>();
    }

    protected override void Update()
    {
        base.Update();
        PlayerDodgeUpdate();
        UpdateDodgeMoveDir();
    }
    private void LateUpdate()
    {
        UpdateAnimation();
        CharacterRotateControl();
        //UpdateCharacterFootSound();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }



    //private void UpdateCharacterFootSound()
    //{
    //    //当在地面上面并且在移动动画并且移动速度达到0.5时才会触发移动的声音
    //    if (isGround && animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && animator.GetFloat(AnimationHash.MovementHash) > .5f)
    //    {
    //        nextFootSound -= Time.deltaTime;
    //        if (nextFootSound < 0f)
    //        {
    //            PlayFootSound();
    //        }
    //    }
    //    else
    //    {
    //        nextFootSound = 0f;
    //    }
    //}

    //private void PlayFootSound()
    //{
    //    GamePoolManager.MainInstance.TryGetPoolItem("FootSound", transform.position, transform.rotation);
    //    nextFootSound = animator.GetFloat(AnimationHash.MovementHash)>1.1f ? fastFootSound : slowFootSound;
    //}


}
