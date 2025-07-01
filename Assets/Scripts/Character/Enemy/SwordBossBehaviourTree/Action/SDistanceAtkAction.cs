using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;

public class SDistanceAtkAction : Action
{
    //�����빥���ű�
    private float comboCd = 6f;
    private float timer = 0f;
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
                enemyBossComboControl.SetComboAsShort();//����Ϊ�̾���
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
