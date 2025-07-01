using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
    public IdleState(FSMachine mchine) : base(mchine)
    {
    }

    public override void Act()
    {
        //判断当前
        //if (!isIdleAnimationPlaying)
        //{
        //    EnsureIdleAnimation();
        //}
    }

    public override void Enter()
    {
        //IDLE状态，进入时切换动画状态机
        m_Mchine.Animator.Play("Idle");
    }

    public override void Exit()
    {
        
    }

    public override void Reason()
    {
        
        //切换条件
        //当发现敌人时候追上
        //Debug.Log(m_Mchine.Owner.GetComponent<EnemyCombatControl>().LockPlayer());
        if (m_Mchine.Owner.GetComponent<EnemyCombatControl>().LockPlayer())
        {
            //切换状态
            m_Mchine.SetTransition(Transition.FindPlayer);
        }
    }

}
