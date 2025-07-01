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
        //添加到Puremvc框架
        //proxy
        EnemyHpP p = PMFacade.MainInstance.RetrieveProxy(PMConst.EnemyHpP) as EnemyHpP;
        p.AddEnemyProxy(gameObject.GetInstanceID(), m_attr);
        //mdediator
        EnemyHpM m = PMFacade.MainInstance.RetrieveMediator(PMConst.EnemyHpM) as EnemyHpM;
        //获取uipanel
        PanelBase panel = transform.Find("HpBar/HpCanvas").GetComponent<EnemyHpBarPanel>();
        m.AddPanel(gameObject.GetInstanceID(), panel);
    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        //取消绑定
        //proxy
        //EnemyHpP p = PMFacade.MainInstance.RetrieveProxy(PMConst.EnemyHpP) as EnemyHpP;
        //p.RemoveEnemyProxy(gameObject.GetInstanceID());
        ////mdediator
        //EnemyHpM m = PMFacade.MainInstance.RetrieveMediator(PMConst.EnemyHpM) as EnemyHpM;
        ////获取uipanel
        //m.RemovePanel(gameObject.GetInstanceID());
        RemoveEvent();
    }
    /// <summary>
    /// 受伤行为
    /// </summary>
    public override void HitAction(float damage,string hitName,Transform atker,Transform self)
    {
        base.HitAction(damage,hitName,atker,self);
        //敌人受伤行为脚本
        //判断收击者是否是自己
        //if (me.GetComponent<EnemyHealthyControl>().id != id) return;
        if (transform != self) return;
        //扣除血量
        m_attr.ReduceHP(damage);
        Debug.Log(this.transform.gameObject.GetInstanceID());
        PMFacade.MainInstance.SendNotification(PMConst.EHpUpdateCommand, new object[] { gameObject.GetInstanceID(), -damage });


        // 创建消息并加入队列
        Message message = new Message(damage, hitName, m_attr.GetCurrentHp());
        m_queue.Enqueue(message);
        //死亡判断放在状态机中
        //if(CheckDead())
        //{
        //    OnCharacterDead();
        //}
    }
    /// <summary>
    /// 回收至对象池
    /// </summary>
    public void Release()
    {
        //进行其他操作
        //例如消失动画
        //然后消失回收入池中
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
    /// 角色死亡触发
    /// </summary>
    private void OnCharacterDead()
    {
        //角色死亡播放死亡动画
        
        //播放完死亡动画需要进行回收
        //触发一个计时器进行对该敌人的回收
    }
}
