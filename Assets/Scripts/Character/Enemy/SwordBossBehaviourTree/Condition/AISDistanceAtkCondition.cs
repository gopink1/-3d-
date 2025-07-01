using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AISDistanceAtkCondition : Conditional
{
    //ai������Ĺ����ж����
    //�жϵ�ǰ���˵�λ�ú�����λ�õ����
    private EnemyBossComboControl m_comboControl;
    public override void OnAwake()
    {
        base.OnAwake();
        m_comboControl = GetComponent<EnemyBossComboControl>();
    }

    public override TaskStatus OnUpdate()
    {
        //�ж�����˵�λ�þ�����ж�
        if (m_comboControl.InShortAtkRange())
        {
            //m_comboControl.AtkCommand = true;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }



}
