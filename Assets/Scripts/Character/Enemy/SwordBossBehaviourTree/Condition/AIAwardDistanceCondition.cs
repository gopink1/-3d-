using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwardDistanceCondition : Conditional
{
    //ai���������ж����
    //�жϵ�ǰ���˵�λ�ú�����λ�õ����
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
        //�ж�����˵�λ�þ�����ж�
        if (m_comboControl.InAwardRange())
        {
            //m_movementControl.SetApplyMovement(true);
            return TaskStatus.Success;
        }
        //m_movementControl.SetApplyMovement(false);
        return TaskStatus.Failure;
    }
}
