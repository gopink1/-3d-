using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState
{
    //��ȡ����ҵ�Movement�ű�
    private EnemyMovementControl enemyMovementControl = null;
    private EnemyCombatControl enemyCombatControl = null;
    private Vector3 TargetPos = Vector3.zero;
    public MoveState(FSMachine mchine) : base(mchine)
    {

    }

    private void InitState()
    {
        enemyMovementControl =  m_Mchine.Owner.GetComponent<EnemyMovementControl>();
        enemyCombatControl =  m_Mchine.Owner.GetComponent<EnemyCombatControl>();
    }
    public override void Act()
    {
        TargetPos = enemyCombatControl.GetCurEnemy().position;
        //������ƶ�
        enemyMovementControl.LockViewToTarget(TargetPos);

        enemyMovementControl.MoveToFor();
    }

    public override void Enter()
    {
        InitState();

        enemyMovementControl.SetApplyMovement(true);//��ʼ�˶�����
        enemyMovementControl.SetAnimatorParameters(1f,0f,false);
    }

    public override void Exit()
    {
        enemyMovementControl.SetApplyMovement(false);//�����˶�����
        enemyMovementControl.SetAnimatorParameters(0f, 0f, false);
        TargetPos = Vector3.zero;
    }

    public override void Reason()
    {
        TargetPos = enemyCombatControl.GetCurEnemy().position;
        //�жϵ����Ƿ��ڷ�Χ�ڲ��ڷ�Χ�ھͻ����
        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AwardDistance)
        {
            m_Mchine.SetTransition(Transition.LostTarget);
        }

        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) < enemyCombatControl.AtkDiatance)
        {
            m_Mchine.SetTransition(Transition.Attack);
        }
    }
}
