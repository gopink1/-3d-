using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人的移动控制脚本
/// </summary>
public class EnemyMovementControl : CharacterMovementBase
{
    //敌人的移动控制脚本
    //对于敌人处于需要移动的行为时候需要调用该脚本中的方法
    //1.敌人移动逻辑中动画机的控制
    //2.进入某些状态时需要面对角色
    [SerializeField] private float MoveSpeed = 2f;


    [SerializeField] private bool ApplyMovement;
    [SerializeField] private bool ApplyRunToTarget = false;
    [SerializeField] private float slerpSpeed = 1f;

    public bool IsApplyMovement
    {
        get => ApplyMovement;
    }
    public bool IsApplyRunToTarget
    {
        get => ApplyRunToTarget;
        set => ApplyRunToTarget = value;
    }
    //属性类
    private EnemyAttr m_attr = null;

    public void InitHealthyAttr(EnemyAttr attr)
    {
        m_attr = attr;
        MoveSpeed = m_attr.Speed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Update()
    {
        base.Update();

    }
    public void SetApplyMovement(bool apply)
    {
        ApplyMovement = apply;
    }
    /// <summary>
    /// 传入需要锁定的目标位置
    /// 朝向该目标
    /// </summary>
    /// <param name="position"></param>
    public void LockViewToTarget(Vector3 position)
    {
        if (ApplyMovement)
        {
            Vector3 lookDir = (position -transform.position).normalized;
            lookDir.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * slerpSpeed);
        }
    }


    /// <summary>
    /// 设置动画机参数
    /// 当敌人通过行为树进入不同行为如朝角色跑时需要改变动画机参数
    /// </summary>
    public void SetAnimatorParameters(float vertical,float horizontal,bool isLocked)
    {
        if (!ApplyMovement) return;
        if(isLocked)
        {
            //启用移动则改变相关参数
            animator.SetBool(AnimationHash.HasInputHash, ApplyMovement);
            animator.SetFloat(AnimationHash.LockHash, 1f);
            animator.SetFloat(AnimationHash.MovementHash, 1f);
            animator.SetFloat(AnimationHash.VerticalHash, vertical);
            animator.SetFloat(AnimationHash.HorizontalHash, horizontal);
        }
        else
        {
            //启用移动则改变相关参数
            animator.SetBool(AnimationHash.HasInputHash, ApplyMovement);
            animator.SetFloat(AnimationHash.LockHash, 0f);
            animator.SetFloat(AnimationHash.MovementHash, 1f);
            animator.SetFloat(AnimationHash.VerticalHash, vertical);
            animator.SetFloat(AnimationHash.HorizontalHash, horizontal);
        }

    }

    public void MoveToDir(Vector3 dir)
    {
        if(dir != null)
        {
            charactercontroller.Move(dir.normalized * Time.deltaTime * MoveSpeed);
        }
    }
    public void MoveToFor()
    {
        charactercontroller.Move(transform.forward * Time.deltaTime * MoveSpeed);
    }

    private void OnEnemyDead(Transform enemy)
    {
        if (enemy == transform)
        {
            //死亡后无法移动
            ApplyMovement = false;
        }
    }
}
