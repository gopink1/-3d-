using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombatBase : MonoBehaviour
{
    // 抽象类
    // player和敌人 的连招系统的基类

    protected Animator animator;

    protected CharacterComboSO currentCombo;

    protected Transform currentEnemy;


    protected bool canAttack;//是否可攻击
    protected int hitIndex;//伤害次数的
    protected float maxColdTime;//最大冷却时间
    protected int comboIndex;//连段的index

    protected float atkMultiplier = 1f;
    public float AtkMultiplier
    {
        get { return atkMultiplier; }
        set { atkMultiplier = value; }
    }

    protected int currentComboCount;//当前连击的段数，用于判断连段触发

    //protected bool isAction;//是否正在执行动作中


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        canAttack = true;
        maxColdTime = 0;
        comboIndex = 0;
        hitIndex = 0;
    }
    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Update()
    {
        RestAttackIndex();
    }
    protected virtual void LateUpdate()
    {
        UpdateCharacterRotate();//角色攻击自动视角锁定敌人
    }

    /// <summary>
    /// 判断能否攻击
    /// </summary>
    /// <returns> 返回一个bool值 用于判断角色当前的状态能都攻击</returns>
    #region 是否可以攻击检测

    protected virtual bool CanAttackInput()
    {
        if (!canAttack) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Parry")) return false;
        
        return true;
    }
    #endregion

    #region 角色输入
    public virtual void CharacterBaseComboInput() { }
    #endregion
    #region 改变连招表

    protected void ChangeComboSO(CharacterComboSO so)
    {
        if (currentCombo != so)
        {
            currentCombo = so;
            ResetCombo();
        }
    }
    #endregion


    #region 重置连招
    protected void ResetCombo()
    {
        maxColdTime = 0;
        comboIndex = 0;
        hitIndex = 0;
    }
    #endregion
    #region 重置hitindex
    /// <summary>
    /// 重置攻击代号
    /// 攻击单个动作可能是多段伤害
    /// 每次触发攻击需要进行对受伤代号的重置
    /// </summary>
    protected void ResetHitIndex()
    {
        hitIndex++;
        if (hitIndex == currentCombo.TryGetHitAndParryMaxCount(comboIndex))
        {
            hitIndex = 0;
        }
    }
    #endregion

    #region 执行动作
    protected virtual void ExecuteShortComboAction()
    {
    }

    protected virtual void UpdateComboInfo()
    {
    }
    #endregion

    #region 每次攻击后检测当前状态是不是移动

    protected void RestAttackIndex()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && !animator.IsInTransition(0))
        {
            ResetCombo();
            currentComboCount = 0;
        }
    }
    #endregion


    protected void ATK()
    {
        TriggerAttack();
        ResetHitIndex();
        //GamePoolManager.MainInstance.TryGetPoolItem("ATKSound", transform.position, transform.rotation);
    }
    protected virtual void TriggerAttack()
    {

    }
    #region 获取距离和向量的函数

    protected Vector3 GetVector3(Vector3 v1, Vector3 v2)
    {
        return (v1-v2).normalized;
    }
    protected float GetDistance(Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1, v2);
    }

    #endregion

    /// <summary>
    /// 更新角色的动画匹配
    /// </summary>
    protected virtual void UpdateMatchAnimation() { }
    #region 更新角色的朝向
    /// <summary>
    /// 角色处于攻击动画时候自动的朝向敌人
    /// </summary>
    protected virtual void UpdateCharacterRotate()
    {
        if (currentEnemy == null) return;
        if (GetDistance(transform.position, currentEnemy.position) > 2f) return;
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsTag("Attack") && currentState.normalizedTime < 0.3f)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);
            // 平滑旋转到目标旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    #endregion

}

