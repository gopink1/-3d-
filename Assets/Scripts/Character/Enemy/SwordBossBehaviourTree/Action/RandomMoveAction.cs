using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;

public class RandomMoveAction : Action
{
    //随机左右移动的行为


    private EnemyMovementControl enemyMovementControl;
    private EnemyBossComboControl enemyBossComboControl;

    private int randomDir;              //随机对峙状态的左右移动方向
    private int randomTime;             //随机左右移动的持续事件
    private float timer;                //计时器
    private bool israndomDir = false;   //是否转换左右移动的标志
/*    private float atkCD = 3f;    */            //攻击的cd
    public override void OnAwake()
    {
        base.OnAwake();
        enemyMovementControl = GetComponent<EnemyMovementControl>();
        enemyBossComboControl = GetComponent<EnemyBossComboControl>();
    }
    public override TaskStatus OnUpdate()
    {
        if(enemyMovementControl.IsApplyRunToTarget || enemyBossComboControl.AtkCommand) return TaskStatus.Success;


        //如果未处于攻击指令和跑向玩家的指令
        //进行当前节点行为，随机移动
        if(israndomDir)
        {
            randomDir = Random.Range(0, 2);
            enemyMovementControl.SetApplyMovement(true);
            israndomDir = false;
        }
        randomTime = Random.Range(1, 2);
        //根据随机数决定向左还是向右跑动,移动的事件
        enemyMovementControl.LockViewToTarget(enemyBossComboControl.GetCurEnemy().position);
        //Debug.Log("敌人位置" + enemyBossComboControl.GetCurEnemy().position);
        if(randomDir == 0)
        {
            //向左
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
            //向右
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
