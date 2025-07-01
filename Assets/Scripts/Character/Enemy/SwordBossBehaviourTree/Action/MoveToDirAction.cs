using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDirAction : Action
{
    //使角色移动至某个方向的行为
    //四个方向0123，代表前后左右

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
        //当没有攻击指令和Run指令时候才会进行该对峙状态
        if(enemyBossComboControl.AtkCommand || enemyMovementControl.IsApplyRunToTarget) return TaskStatus.Success;

        //判断当前的位置是否需要进行移动即在每个方向移动时候
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
                    //前
                    enemyMovementControl.SetAnimatorParameters(1f, 0f, true);
                    enemyMovementControl.MoveToDir(transform.forward);
                    //向前说明需要判断距离是否过近
                    if (enemyBossComboControl.InLongAtkRange())
                    {
                        enemyMovementControl.SetApplyMovement(false);
                        return TaskStatus.Success;
                    }


                    break;
                case 1:
                    //后
                    enemyMovementControl.SetAnimatorParameters(-1f, 0f, true);
                    enemyMovementControl.MoveToDir(-transform.forward);
                    //向前说明需要判断距离是否过远
                    if (!enemyBossComboControl.InAwardRange())
                    {
                        enemyMovementControl.SetApplyMovement(false);
                        return TaskStatus.Success;
                    }
                    

                    break;
                case 2:
                    //左
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
                    //右
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
