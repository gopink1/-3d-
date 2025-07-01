using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignAtkAction : Action
{
    //
    EnemyBossComboControl enemyBossComboControl;
    EnemyMovementControl enemyMovementControl;

    //指派攻击的行为
    private float timer;//计时器
    private float atkDelta = 3f;//对峙中，攻击间隔

    public override void OnAwake()
    {
        base.OnAwake();
        enemyBossComboControl = GetComponent<EnemyBossComboControl>();
        enemyMovementControl = GetComponent<EnemyMovementControl>();
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;
        if(timer > atkDelta )
        {
            timer = 0;
            enemyMovementControl.SetApplyMovement(false);
            enemyBossComboControl.AtkCommand = true;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }


}
