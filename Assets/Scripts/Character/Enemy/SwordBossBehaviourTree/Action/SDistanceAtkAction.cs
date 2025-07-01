using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;

public class SDistanceAtkAction : Action
{
    //近距离攻击脚本
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
            //判断是否需要进行切换连招
            if (enemyBossComboControl.CanChangeComb0)
            {
                enemyBossComboControl.SetComboAsShort();//设置为短距离
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
