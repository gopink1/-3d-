using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossHealthyControl : CharacterHealthyBase
{
    [SerializeField] private EnemyAttr m_attr = null;
    [SerializeField] private int m_ID;
    public EnemyAttr GetAttr() { return m_attr; }
    private bool isDead;
    public void InitHealthy(EnemyAttr attr)
    {
        m_attr = attr;

        gameObject.GetComponent<EnemyMovementControl>().InitHealthyAttr(attr);
    }
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {

    }
    private void OnEnable()
    {
        InitAddEvent();
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
    /// <summary>
    /// 受伤行为
    /// </summary>
    public override void HitAction(float damage, string hitName, Transform atker, Transform self)
    {
        base.HitAction(damage, hitName, atker, self);
        //敌人受伤行为脚本
        //判断收击者是否是自己
        //if (me.GetComponent<EnemyHealthyControl>().id != id) return;
        if (transform != self) return;
        //扣除血量
        m_attr.ReduceHP(damage);
        Debug.Log("触发boss受伤" + m_attr.GetCurrentHp());

        //触发PureMVC进行面板更新
        PMFacade.MainInstance.SendNotification(PMConst.BossHpUpdateCommand,-damage);


        //播放受伤动画的处理
        //播放受伤动画
        int hitHash = Animator.StringToHash(hitName);
        if (m_Animator.HasState(0, hitHash))
        {
            m_Animator.Play(hitName, 0, 0f);
        }
        else if (string.IsNullOrEmpty(hitName))
        {
            //什么都不做
        }
        else
        {
            m_Animator.Play("NormalHit", 0, 0f);
            //Debug.Log("触发事件1");
        }
        //播放受伤音效


        //死亡判断放在状态机中
        if (CheckDead())
        {
            OnCharacterDead();
        }
    }
    private bool CheckDead()
    {
        if (m_attr.GetCurrentHp() < 0)
        {
            isDead = true;
            GameEventManager.MainInstance.CallEvent<int>(EventHash.BossKill, m_ID);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 角色死亡触发
    /// </summary>
    private void OnCharacterDead()
    {
        //角色死亡播放死亡动画
        m_Animator.Play("Die");
        transform.GetComponent<BehaviorTree>().enabled = false;
        Destroy(gameObject);
        //播放完死亡动画需要进行回收
        //触发一个计时器进行对该敌人的回收
    }
}
