using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;

public class RandomMoveAction : Action
{
    //��������ƶ�����Ϊ


    private EnemyMovementControl enemyMovementControl;
    private EnemyBossComboControl enemyBossComboControl;

    private int randomDir;              //�������״̬�������ƶ�����
    private int randomTime;             //��������ƶ��ĳ����¼�
    private float timer;                //��ʱ��
    private bool israndomDir = false;   //�Ƿ�ת�������ƶ��ı�־
/*    private float atkCD = 3f;    */            //������cd
    public override void OnAwake()
    {
        base.OnAwake();
        enemyMovementControl = GetComponent<EnemyMovementControl>();
        enemyBossComboControl = GetComponent<EnemyBossComboControl>();
    }
    public override TaskStatus OnUpdate()
    {
        if(enemyMovementControl.IsApplyRunToTarget || enemyBossComboControl.AtkCommand) return TaskStatus.Success;


        //���δ���ڹ���ָ���������ҵ�ָ��
        //���е�ǰ�ڵ���Ϊ������ƶ�
        if(israndomDir)
        {
            randomDir = Random.Range(0, 2);
            enemyMovementControl.SetApplyMovement(true);
            israndomDir = false;
        }
        randomTime = Random.Range(1, 2);
        //������������������������ܶ�,�ƶ����¼�
        enemyMovementControl.LockViewToTarget(enemyBossComboControl.GetCurEnemy().position);
        //Debug.Log("����λ��" + enemyBossComboControl.GetCurEnemy().position);
        if(randomDir == 0)
        {
            //����
            enemyMovementControl.SetAnimatorParameters(0f, -1f, true);
            enemyMovementControl.MoveToDir(-transform.right);
            timer += Time.deltaTime;
            //if (timer > atkCD)
            //{
            //    timer = 0;
            //    israndomDir = true;
            //    enemyMovementControl.SetApplyMovement(false);
            //    return TaskStatus.Success;
            //}
            if (timer > randomTime)
            {
                timer = 0;
                israndomDir = true;
                enemyMovementControl.SetApplyMovement(false);
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
        else
        {
            //����
            enemyMovementControl.SetAnimatorParameters(0f, 1f, true);
            enemyMovementControl.MoveToDir(transform.right);
            timer += Time.deltaTime;
            //if(timer > atkCD)
            //{
            //    timer = 0;
            //    israndomDir = true;
            //                    enemyBossComboControl.AtkCommand = true;
            //    return TaskStatus.Success;
            //}
            if (timer > randomTime)
            {
                timer = 0;
                israndomDir = true;
                enemyBossComboControl.AtkCommand = true;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }



}
