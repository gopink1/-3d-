using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : FSMState
{
    //����״̬������������
    private EnemyMovementControl enemyMovementControl = null;
    private EnemyCombatControl enemyCombatControl = null;
    private EnemyHealthyControl enemyHealthyControl = null;
    private Animator m_animator = null;

    public DeadState(FSMachine mchine) : base(mchine)
    {
    }

    public override void Act()
    {
        //������ÿִ֡��
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
        //������������
        m_animator.Play("Die");
        //���������Ƴ�charactercontrol
        m_Mchine.Owner.gameObject.GetComponent<CharacterController>().enabled = false;

        //�������յĺ���

        //������Ч
        //������Ч
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Reason()
    {
        //����״̬����Ҫ�л�
        //����Ҫ��������ڴ�
    }
}
