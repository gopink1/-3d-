using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : FSMState
{
    private EnemyMovementControl enemyMovementControl = null;
    private EnemyCombatControl enemyCombatControl = null;
    private Animator m_animator = null;
    private Vector3 TargetPos = Vector3.zero;

    private string curHitName;
    public HitState(FSMachine mchine) : base(mchine)
    {
        curHitName = "";
    }

    private void InitState()
    {
        m_animator = m_Mchine.Owner.gameObject.GetComponent<Animator>();
        if (enemyMovementControl == null)
        {
            enemyMovementControl =  m_Mchine.Owner.GetComponent<EnemyMovementControl>();
        }
        if (enemyCombatControl == null)
        {
            enemyCombatControl =  m_Mchine.Owner.GetComponent<EnemyCombatControl>();
        }
    }
    public override void Act()
    {
        
    }

    public override void Enter()
    {
        InitState();
        //播放受伤动画
        int hitHash = Animator.StringToHash(curHitName);
        if (m_animator.HasState(0, hitHash))
        {
            m_animator.Play(curHitName, 0, 0f);
        }
        else
        {
            m_animator.Play("NormalHit", 0, 0f);
            //Debug.Log("触发事件1");
        }
        m_animator.Play("NormalHit", 0, 0f);
        //播放受伤音效

    }

    public override void Exit()
    {
        //throw new System.NotImplementedException();
    }

    public override void Reason()
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return;
        TargetPos = enemyCombatControl.GetCurEnemy().position;
        //判断，当前动画播放完毕后，根据距离进入状态
        //未处于警戒范围
        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AwardDistance)
        {
            m_Mchine.SetTransition(Transition.LostTarget);
        }
        //警戒范围
        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AtkDiatance &&
            Vector3.Distance(TargetPos, enemyMovementControl.transform.position) < enemyCombatControl.AwardDistance)
        {
            m_Mchine.SetTransition(Transition.FindPlayer);
        }
        //攻击范围
        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) < enemyCombatControl.AtkDiatance)
        {
            m_Mchine.SetTransition(Transition.Attack);
        }
        //死了

    }

    public void SetHitName(string hitName)
    {
        curHitName = hitName;
    }

}
