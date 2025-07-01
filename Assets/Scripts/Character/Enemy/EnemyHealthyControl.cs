using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthyControl : CharacterHealthyBase
{
    [SerializeField] private EnemyAttr m_attr = null;
    private bool isDead;
    [SerializeField] private int m_ID;
    private MassageQueue m_queue;
    public void InitHealthy(EnemyAttr attr,MassageQueue massageQueue)
    {
        m_attr = attr;
        //gameObject.GetComponent<EnemyCombatControl>().InitHealthyAttr(attr);
        gameObject.GetComponent<EnemyMovementControl>().InitHealthyAttr(attr);
        m_queue = massageQueue;
    }
    public EnemyAttr GetAttr() { return m_attr; }

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        //��ӵ�Puremvc���
        //proxy
        EnemyHpP p = PMFacade.MainInstance.RetrieveProxy(PMConst.EnemyHpP) as EnemyHpP;
        p.AddEnemyProxy(gameObject.GetInstanceID(), m_attr);
        //mdediator
        EnemyHpM m = PMFacade.MainInstance.RetrieveMediator(PMConst.EnemyHpM) as EnemyHpM;
        //��ȡuipanel
        PanelBase panel = transform.Find("HpBar/HpCanvas").GetComponent<EnemyHpBarPanel>();
        m.AddPanel(gameObject.GetInstanceID(), panel);
    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        //ȡ����
        //proxy
        //EnemyHpP p = PMFacade.MainInstance.RetrieveProxy(PMConst.EnemyHpP) as EnemyHpP;
        //p.RemoveEnemyProxy(gameObject.GetInstanceID());
        ////mdediator
        //EnemyHpM m = PMFacade.MainInstance.RetrieveMediator(PMConst.EnemyHpM) as EnemyHpM;
        ////��ȡuipanel
        //m.RemovePanel(gameObject.GetInstanceID());
        RemoveEvent();
    }
    /// <summary>
    /// ������Ϊ
    /// </summary>
    public override void HitAction(float damage,string hitName,Transform atker,Transform self)
    {
        base.HitAction(damage,hitName,atker,self);
        //����������Ϊ�ű�
        //�ж��ջ����Ƿ����Լ�
        //if (me.GetComponent<EnemyHealthyControl>().id != id) return;
        if (transform != self) return;
        //�۳�Ѫ��
        m_attr.ReduceHP(damage);
        Debug.Log(this.transform.gameObject.GetInstanceID());
        PMFacade.MainInstance.SendNotification(PMConst.EHpUpdateCommand, new object[] { gameObject.GetInstanceID(), -damage });


        // ������Ϣ���������
        Message message = new Message(damage, hitName, m_attr.GetCurrentHp());
        m_queue.Enqueue(message);
        //�����жϷ���״̬����
        //if(CheckDead())
        //{
        //    OnCharacterDead();
        //}
    }
    /// <summary>
    /// �����������
    /// </summary>
    public void Release()
    {
        //������������
        //������ʧ����
        //Ȼ����ʧ���������
        GameBase.MainInstance.ReleaseOneItemToPool(this.gameObject);
    }


    public bool CheckDead()
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
        
        //����������������Ҫ���л���
        //����һ����ʱ�����жԸõ��˵Ļ���
    }
}
