using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : FSMState
{
    //死亡状态播放死亡动画
    private EnemyMovementControl enemyMovementControl = null;
    private EnemyCombatControl enemyCombatControl = null;
    private EnemyHealthyControl enemyHealthyControl = null;
    private Animator m_animator = null;

    public DeadState(FSMachine mchine) : base(mchine)
    {
    }

    public override void Act()
    {
        //死亡后每帧执行
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
        if(enemyHealthyControl == null)
        {
            enemyHealthyControl = m_Mchine.Owner.GetComponent<EnemyHealthyControl>();
        }
    }
    public override void Enter()
    {
        InitState();
        enemyHealthyControl.CheckDead();
        //播放死亡动画
        m_animator.Play("Die");
        //敌人死亡移除charactercontrol
        m_Mchine.Owner.gameObject.GetComponent<CharacterController>().enabled = false;

        //触发回收的函数

        //死亡特效
        //死亡音效
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Reason()
    {
        //死亡状态不需要切换
        //除非要复活，敬请期待
    }
}
