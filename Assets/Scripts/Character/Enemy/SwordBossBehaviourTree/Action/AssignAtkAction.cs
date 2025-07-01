using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignAtkAction : Action
{
    //
    EnemyBossComboControl enemyBossComboControl;
    EnemyMovementControl enemyMovementControl;

    //ָ�ɹ�������Ϊ
    private float timer;//��ʱ��
    private float atkDelta = 3f;//�����У��������

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
