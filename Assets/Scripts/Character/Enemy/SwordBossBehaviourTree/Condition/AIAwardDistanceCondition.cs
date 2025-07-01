using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwardDistanceCondition : Conditional
{
    //ai警戒距离的判断情况
    //判断当前敌人的位置和自身位置的情况
    private EnemyBossComboControl m_comboControl;
    private EnemyMovementControl m_movementControl;
    public override void OnAwake()
    {
        base.OnAwake();
        m_comboControl = GetComponent<EnemyBossComboControl>();
        m_movementControl = GetComponent<EnemyMovementControl>();
    }

    public override TaskStatus OnUpdate()
    {
        //判断与敌人的位置距离的判断
        if (m_comboControl.InAwardRange())
        {
            //m_movementControl.SetApplyMovement(true);
            return TaskStatus.Success;
        }
        //m_movementControl.SetApplyMovement(false);
        return TaskStatus.Failure;
    }
}
