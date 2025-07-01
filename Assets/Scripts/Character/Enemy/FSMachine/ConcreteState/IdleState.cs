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
        //�жϵ�ǰ
        //if (!isIdleAnimationPlaying)
        //{
        //    EnsureIdleAnimation();
        //}
    }

    public override void Enter()
    {
        //IDLE״̬������ʱ�л�����״̬��
        m_Mchine.Animator.Play("Idle");
    }

    public override void Exit()
    {
        
    }

    public override void Reason()
    {
        
        //�л�����
        //�����ֵ���ʱ��׷��
        //Debug.Log(m_Mchine.Owner.GetComponent<EnemyCombatControl>().LockPlayer());
        if (m_Mchine.Owner.GetComponent<EnemyCombatControl>().LockPlayer())
        {
            //�л�״̬
            m_Mchine.SetTransition(Transition.FindPlayer);
        }
    }

}
