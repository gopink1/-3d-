using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerAction : Action
{
    //�ƶ�����ҵ���Ϊ
    //
    //����ɫ���ھ��䷶Χ��
    //���Ҵ����޷�����״̬�������벻����cd�ȣ�
    //��ʱ����ƶ��ͻ����������������ƶ�

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
