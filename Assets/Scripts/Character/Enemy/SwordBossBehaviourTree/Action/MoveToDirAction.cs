using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDirAction : Action
{
    //ʹ��ɫ�ƶ���ĳ���������Ϊ
    //�ĸ�����0123������ǰ������

    [SerializeField]private int dir;
    private EnemyMovementControl enemyMovementControl;
    private EnemyBossComboControl enemyBossComboControl;

    private float atkCoolTime = 10f;
    private float timer = 0f;


    public override void OnAwake()
    {
        base.OnAwake();
        enemyMovementControl = GetComponent<EnemyMovementControl>();
        enemyBossComboControl = GetComponent<EnemyBossComboControl>();
    }

    public override TaskStatus OnUpdate()
    {
        //��û�й���ָ���Runָ��ʱ��Ż���иö���״̬
        if(enemyBossComboControl.AtkCommand || enemyMovementControl.IsApplyRunToTarget) return TaskStatus.Success;

        //�жϵ�ǰ��λ���Ƿ���Ҫ�����ƶ�����ÿ�������ƶ�ʱ��
        timer += Time.deltaTime;
        if(timer > atkCoolTime)
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                enemyMovementControl.IsApplyRunToTarget = true;
            }
            else
            {
                enemyBossComboControl.AtkCommand = true;
            }
            timer = 0f;
        }


        enemyMovementControl.SetApplyMovement(true);
        if (enemyMovementControl.IsApplyMovement)
        {
            enemyMovementControl.LockViewToTarget(enemyBossComboControl.GetCurEnemy().position);
            switch (dir)
            {
                case 0:
                    //ǰ
                    enemyMovementControl.SetAnimatorParameters(1f, 0f, true);
                    enemyMovementControl.MoveToDir(transform.forward);
                    //��ǰ˵����Ҫ�жϾ����Ƿ����
                    if (enemyBossComboControl.InLongAtkRange())
                    {
                        enemyMovementControl.SetApplyMovement(false);
                        return TaskStatus.Success;
                    }


                    break;
                case 1:
                    //��
                    enemyMovementControl.SetAnimatorParameters(-1f, 0f, true);
                    enemyMovementControl.MoveToDir(-transform.forward);
                    //��ǰ˵����Ҫ�жϾ����Ƿ��Զ
                    if (!enemyBossComboControl.InAwardRange())
                    {
                        enemyMovementControl.SetApplyMovement(false);
                        return TaskStatus.Success;
                    }
                    

                    break;
                case 2:
                    //��
                    enemyMovementControl.SetAnimatorParameters(0f, -1f, true);
                    enemyMovementControl.MoveToDir(-transform.right);
                    //
                    if (enemyBossComboControl.InShortAtkRange())
                    {
                        enemyMovementControl.SetApplyMovement(false);
                        return TaskStatus.Success;
                    }

                    break;
                case 3:
                    //��
                    enemyMovementControl.SetAnimatorParameters(0f, 1f, true);
                    enemyMovementControl.MoveToDir(transform.right);
                    //
                    if (enemyBossComboControl.InShortAtkRange())
                    {
                        enemyMovementControl.SetApplyMovement(false);
                        return TaskStatus.Success;
                    }

                    break;
                default:
                    break;
            }
            return TaskStatus.Running;
        }
        return TaskStatus.Success;

    }
}
