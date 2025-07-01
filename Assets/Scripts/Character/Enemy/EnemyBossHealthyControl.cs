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
    /// ������Ϊ
    /// </summary>
    public override void HitAction(float damage, string hitName, Transform atker, Transform self)
    {
        base.HitAction(damage, hitName, atker, self);
        //����������Ϊ�ű�
        //�ж��ջ����Ƿ����Լ�
        //if (me.GetComponent<EnemyHealthyControl>().id != id) return;
        if (transform != self) return;
        //�۳�Ѫ��
        m_attr.ReduceHP(damage);
        Debug.Log("����boss����" + m_attr.GetCurrentHp());

        //����PureMVC����������
        PMFacade.MainInstance.SendNotification(PMConst.BossHpUpdateCommand,-damage);


        //�������˶����Ĵ���
        //�������˶���
        int hitHash = Animator.StringToHash(hitName);
        if (m_Animator.HasState(0, hitHash))
        {
            m_Animator.Play(hitName, 0, 0f);
        }
        else if (string.IsNullOrEmpty(hitName))
        {
            //ʲô������
        }
        else
        {
            m_Animator.Play("NormalHit", 0, 0f);
            //Debug.Log("�����¼�1");
        }
        //����������Ч


        //�����жϷ���״̬����
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
    /// ��ɫ��������
    /// </summary>
    private void OnCharacterDead()
    {
        //��ɫ����������������
        m_Animator.Play("Die");
        transform.GetComponent<BehaviorTree>().enabled = false;
        Destroy(gameObject);
        //����������������Ҫ���л���
        //����һ����ʱ�����жԸõ��˵Ļ���
    }
}
