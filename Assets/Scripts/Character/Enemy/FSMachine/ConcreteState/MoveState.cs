using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState
{
    //获取到玩家的Movement脚本
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
        //向玩家移动
        enemyMovementControl.LockViewToTarget(TargetPos);

        enemyMovementControl.MoveToFor();
    }

    public override void Enter()
    {
        InitState();

        enemyMovementControl.SetApplyMovement(true);//开始运动动画
        enemyMovementControl.SetAnimatorParameters(1f,0f,false);
    }

    public override void Exit()
    {
        enemyMovementControl.SetApplyMovement(false);//结束运动动画
        enemyMovementControl.SetAnimatorParameters(0f, 0f, false);
        TargetPos = Vector3.zero;
    }

    public override void Reason()
    {
        TargetPos = enemyCombatControl.GetCurEnemy().position;
        //判断敌人是否在范围内不在范围内就会待机
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
