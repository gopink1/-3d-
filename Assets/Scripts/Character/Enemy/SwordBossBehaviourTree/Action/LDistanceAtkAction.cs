using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDistanceAtkAction : Action
{
    //Զ���빥���ű�
    private EnemyBossComboControl enemyBossComboControl;
    public override void OnAwake()
    {
        base.OnAwake();
        enemyBossComboControl = GetComponent<EnemyBossComboControl>();
    }
    public override TaskStatus OnUpdate()
    {

        if (enemyBossComboControl.AtkCommand)
        {
            //�ж��Ƿ���Ҫ�����л�����
            if (enemyBossComboControl.CanChangeComb0)
            {
                enemyBossComboControl.SetComboAsLong();//����Ϊ�̾���
                enemyBossComboControl.SetRandomCombo();//����Ϊ���combo
            }
            //ִ�ж���
            enemyBossComboControl.BossComboInput();
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }

    }
}
