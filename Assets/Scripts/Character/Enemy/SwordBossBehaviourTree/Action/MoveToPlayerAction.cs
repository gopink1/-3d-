using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerAction : Action
{
    //移动向玩家的行为
    //
    //当角色处于警戒范围内
    //而且处于无法攻击状态（即距离不够，cd等）
    //此时这个移动就会随机触发进行随机移动

    private EnemyMovementControl enemyMovementControl;
    private EnemyBossComboControl enemyBossComboControl;
    public override void OnAwake()
    {
        base.OnAwake();
        enemyMovementControl = GetComponent<EnemyMovementControl>();
        enemyBossComboControl = GetComponent<EnemyBossComboControl>();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemyMovementControl.IsApplyMovement)
        {
            Vector3 targetpos = enemyBossComboControl.GetCurEnemy().position;
            enemyMovementControl.LockViewToTarget(targetpos);
            enemyMovementControl.SetAnimatorParameters(1f, 0f, true);
            enemyMovementControl.MoveToDir(transform.forward);
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}
