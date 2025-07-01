using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class AttackState : FSMState
{
    private EnemyMovementControl enemyMovementControl = null;
    private EnemyCombatControl enemyCombatControl = null;
    private Vector3 TargetPos = Vector3.zero;
    public AttackState(FSMachine mchine) : base(mchine)
    {
    }

    private void InitState()
    {
        if(enemyMovementControl == null)
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
        //����״̬���жϵ�ǰ�Ĺ���cd�����Ƿ񹥻��ж�
        //���ݵ�ǰ����״̬�����ж�
        enemyCombatControl.ExecuteNormalAtkAction();
    }

    public override void Enter()
    {
        InitState();//��ʼ��״̬
        //��ʼ��λ����Ϣ
        //����״̬����󴥷�����
        //����EnemyCombatControl��ExecuteNormalAtkAction
        enemyCombatControl.ExecuteNormalAtkAction();
    }

    public override void Exit()
    {
        
    }

    public override void Reason()
    {
        TargetPos = enemyCombatControl.GetCurEnemy().position;
        //�ж��뿪
        //�����ڹ�������ʱ��㲻�����ת��

        //�������Ҿ��룬����������Χ�ͻ����MoveState
        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AtkDiatance)
        {
            m_Mchine.SetTransition(Transition.FindPlayer);
        }
        //����������䷶Χ�ͻ����IDle״̬

        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AwardDistance)
        {
            m_Mchine.SetTransition(Transition.LostTarget);
        }
        //������ڹ�����Χ��Action�м�������

    }
}
