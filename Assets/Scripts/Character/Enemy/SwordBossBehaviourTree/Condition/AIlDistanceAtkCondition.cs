using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIlDistanceAtkCondition : Conditional
{
    //ai近距离的攻击判断情况
    //判断当前敌人的位置和自身位置的情况
    private EnemyBossComboControl m_comboControl;
    public override void OnAwake()
    {
        base.OnAwake();
        m_comboControl = GetComponent<EnemyBossComboControl>();
    }

    public override TaskStatus OnUpdate()
    {
        //判断与敌人的位置距离的判断
        if (m_comboControl.InLongAtkRange())
        {
            //m_comboControl.AtkCommand = true;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
