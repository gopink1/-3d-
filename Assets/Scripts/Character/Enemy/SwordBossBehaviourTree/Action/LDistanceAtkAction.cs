using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDistanceAtkAction : Action
{
    //远距离攻击脚本
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
            //判断是否需要进行切换连招
            if (enemyBossComboControl.CanChangeComb0)
            {
                enemyBossComboControl.SetComboAsLong();//设置为短距离
                enemyBossComboControl.SetRandomCombo();//设置为随机combo
            }
            //执行动作
            enemyBossComboControl.BossComboInput();
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }

    }
}
