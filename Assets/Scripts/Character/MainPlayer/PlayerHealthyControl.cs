using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// 角色健康系统
/// </summary>
public class PlayerHealthyControl : CharacterHealthyBase
{
    
    [SerializeField] private PlayerAttr m_attr = null;

    private bool isDead = false;
    //初始化属性
    public void InitHealthyAttr(PlayerAttr attr)
    {
        m_attr = attr;
    }
    protected override void Awake()
    {
        base.Awake();
        m_attr = new PlayerAttr(100, 100, 1, 2, 2.5f, 100, 100, 1);

    }
    public PlayerAttr GetAttr()
    {
        if (m_attr == null)
        {
            m_attr = new PlayerAttr(100, 100, 10, 10, 2.5f, 100, 100, 1);
        }
        return m_attr;
    }
    private void Start()
    {
        InitAddEvent();
        GameBase.MainInstance.GetPlayerDataSys().OnAttributeChanged += OnAttributeChanged;

    }
    private void OnDestroy()
    {
        RemoveEvent();
    }

    public override void HitAction(float damage, string hitName, Transform atker, Transform self)
    {
        base.HitAction(damage, hitName, atker, self);
        if (isDead) return;
        if (transform != self) return;
        Debug.Log("触发玩家受伤");
        float trueDamage = damage - m_attr.Def;
        int hitHash = Animator.StringToHash(hitName);

        //播放受伤动画
        if (m_Animator.HasState(0, hitHash))
        {
            m_Animator.Play(hitName, 0, 0f);
            Debug.Log("触发事件11111");
        }
        else if (string.IsNullOrEmpty(hitName))
        {
            //什么都不做
        }
        else
        {
            m_Animator.Play("NormalHit", 0, 0f);
        }
        //播放音效


        //扣除血量
        m_attr.CurrentHp -= trueDamage;
        GameEventManager.MainInstance.CallEvent<DamagetakenArgs>(EventHash.DamageTaken, new DamagetakenArgs(trueDamage, atker.gameObject));
        //触发PureMVC的扣血消息
        PMFacade.MainInstance.SendNotification(PMConst.PHpUpdateCommand, -trueDamage);
        PMFacade.MainInstance.SendNotification(PMConst.PMaxHpUpdateCommand);
        if (CheckDead())
        {
            OnCharacterDead();
        }
    }
    /// <summary>
    /// 治疗触发
    /// </summary>
    /// <param name="healingCount">治疗的数值</param>
    /// <param name="healingName">治疗特效的名字</param>
    /// <param name="self">标定位移物体</param>
    protected override void HealingAction(float healingCount, string healingName, Transform self)
    {
        base.HealingAction(healingCount, healingName, self);
        //主人公治疗的行为
        if (isDead) return;
        if (transform != self) return;

        //播放角色受伤特效

        //播放音效

        //增加血量
        m_attr.CurrentHp += healingCount;
        //触发PureMvc的回血消息
        PMFacade.MainInstance.SendNotification(PMConst.PHpUpdateCommand, healingCount);
        PMFacade.MainInstance.SendNotification(PMConst.PMaxHpUpdateCommand);
    }

    private bool CheckDead()
    {
        if(m_attr.CurrentHp <= 0)  
        {
            isDead = true;
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
        //触发死亡
        GameBase.MainInstance.GameOver("");
    }

    #region 修改角色的属性

    // 属性变更回调
    private void OnAttributeChanged(PlayerAttribute attribute)
    {
        //当属性修改时候调用
        //用于更新UI
        switch (attribute)
        {
            case PlayerAttribute.MaxHP:
                break;
            case PlayerAttribute.MaxMP:
                break;
            case PlayerAttribute.ATK:
                break;
            case PlayerAttribute.DEF:
                break;
            case PlayerAttribute.SPEED:
                break;
            default:
                break;
        }
    }
    public void ModifyAttribute(PlayerAttribute attribute, float value, bool isPercentage) 
    { 

    }
    public void ModifyMaxHP(float value,bool isPercentage)
    {
        // 修改初始代码，传递实际增减值
        float oldhp = m_attr.MaxHp;
        float num = isPercentage ? oldhp * (1 + value) : oldhp + value;
        m_attr.MaxHp = (int)num;
        float cur = m_attr.CurrentHp * (m_attr.MaxHp / oldhp);
        m_attr.CurrentHp = (int)cur;
        Debug.Log(m_attr.CurrentHp+"))))))))))))" + m_attr.MaxMp);
        // 计算实际增减值并传递
        PMFacade.MainInstance.SendNotification(PMConst.PMaxHpUpdateCommand);

    }
    public void ModifyMaxMP(float value, bool isPercentage)
    {
        float oldmp = m_attr.MaxMp;
        m_attr.MaxMp = isPercentage ? m_attr.MaxMp * (1 + value) : m_attr.MaxMp + value;
        m_attr.CurrentMp = m_attr.CurrentMp * (m_attr.MaxHp / oldmp);

    }
    public void ModifyDef(float value, bool isPercentage)
    {
        float olddef = m_attr.Def;
        m_attr.Def = isPercentage ? m_attr.Def * (1 + value) : m_attr.Def + value;

    }
    public void ModifyAtk(float value, bool isPercentage)
    {
        float oldatk = m_attr.Atk;
        m_attr.Atk = isPercentage ? m_attr.Atk * (1 + value) : m_attr.Atk + value;

    }
    public void ModifySpeed(float value, bool isPercentage)
    {
        float oldspeed = m_attr.Speed;
        m_attr.Speed = isPercentage ? m_attr.Speed * (1 + value) : m_attr.Speed + value;

    }
    #endregion

}
