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
        //攻击状态，判断当前的攻击cd进行是否攻击判断
        //根据当前动画状态进行判断
        enemyCombatControl.ExecuteNormalAtkAction();
    }

    public override void Enter()
    {
        InitState();//初始化状态
        //初始化位置信息
        //攻击状态进入后触发攻击
        //触发EnemyCombatControl的ExecuteNormalAtkAction
        enemyCombatControl.ExecuteNormalAtkAction();
    }

    public override void Exit()
    {
        
    }

    public override void Reason()
    {
        TargetPos = enemyCombatControl.GetCurEnemy().position;
        //判断离开
        //当处于攻击动画时候便不会进行转换

        //如果跟玩家距离，超过攻击范围就会进入MoveState
        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AtkDiatance)
        {
            m_Mchine.SetTransition(Transition.FindPlayer);
        }
        //如果超过警戒范围就会进入IDle状态

        if (Vector3.Distance(TargetPos, enemyMovementControl.transform.position) > enemyCombatControl.AwardDistance)
        {
            m_Mchine.SetTransition(Transition.LostTarget);
        }
        //如果还在攻击范围内Action中继续继续

    }
}
