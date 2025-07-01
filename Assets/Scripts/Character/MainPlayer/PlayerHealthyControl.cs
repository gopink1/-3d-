using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// ��ɫ����ϵͳ
/// </summary>
public class PlayerHealthyControl : CharacterHealthyBase
{
    
    [SerializeField] private PlayerAttr m_attr = null;

    private bool isDead = false;
    //��ʼ������
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
        Debug.Log("�����������");
        float trueDamage = damage - m_attr.Def;
        int hitHash = Animator.StringToHash(hitName);

        //�������˶���
        if (m_Animator.HasState(0, hitHash))
        {
            m_Animator.Play(hitName, 0, 0f);
            Debug.Log("�����¼�11111");
        }
        else if (string.IsNullOrEmpty(hitName))
        {
            //ʲô������
        }
        else
        {
            m_Animator.Play("NormalHit", 0, 0f);
        }
        //������Ч


        //�۳�Ѫ��
        m_attr.CurrentHp -= trueDamage;
        GameEventManager.MainInstance.CallEvent<DamagetakenArgs>(EventHash.DamageTaken, new DamagetakenArgs(trueDamage, atker.gameObject));
        //����PureMVC�Ŀ�Ѫ��Ϣ
        PMFacade.MainInstance.SendNotification(PMConst.PHpUpdateCommand, -trueDamage);
        PMFacade.MainInstance.SendNotification(PMConst.PMaxHpUpdateCommand);
        if (CheckDead())
        {
            OnCharacterDead();
        }
    }
    /// <summary>
    /// ���ƴ���
    /// </summary>
    /// <param name="healingCount">���Ƶ���ֵ</param>
    /// <param name="healingName">������Ч������</param>
    /// <param name="self">�궨λ������</param>
    protected override void HealingAction(float healingCount, string healingName, Transform self)
    {
        base.HealingAction(healingCount, healingName, self);
        //���˹����Ƶ���Ϊ
        if (isDead) return;
        if (transform != self) return;

        //���Ž�ɫ������Ч

        //������Ч

        //����Ѫ��
        m_attr.CurrentHp += healingCount;
        //����PureMvc�Ļ�Ѫ��Ϣ
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
    /// ��ɫ��������
    /// </summary>
    private void OnCharacterDead()
    {
        //��ɫ����������������
        m_Animator.Play("Die");
        //��������
        GameBase.MainInstance.GameOver("");
    }

    #region �޸Ľ�ɫ������

    // ���Ա���ص�
    private void OnAttributeChanged(PlayerAttribute attribute)
    {
        //�������޸�ʱ�����
        //���ڸ���UI
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
        // �޸ĳ�ʼ���룬����ʵ������ֵ
        float oldhp = m_attr.MaxHp;
        float num = isPercentage ? oldhp * (1 + value) : oldhp + value;
        m_attr.MaxHp = (int)num;
        float cur = m_attr.CurrentHp * (m_attr.MaxHp / oldhp);
        m_attr.CurrentHp = (int)cur;
        Debug.Log(m_attr.CurrentHp+"))))))))))))" + m_attr.MaxMp);
        // ����ʵ������ֵ������
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
