using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCircleControl : MagicalCircleControl
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layers;



    protected float effectCd;
    private void Awake()
    {
        
    }
    protected override void Update()
    {
        if (effectCd == 0) effectCd = effectRate;
        base.Update();
        //治疗法阵技能
        //要得到每秒回复多少生命

    }
    public override void UpdateWithTimer()
    {
        base.UpdateWithTimer();
        //每帧更新函数用于每帧回复生命的判断和触发
        if(timer >  effectRate )
        {
            effectRate += effectCd;
            //触发回血
            //判断玩家是否在范围内
            Collider[] colliders =  Physics.OverlapSphere(center, radius, layers);
            //判断
            // 遍历所有碰撞到的对象
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider hitCollider = colliders[i];

                if (hitCollider.CompareTag("Player"))
                {
                    // 如果是玩家就触发治疗事件
                    GameEventManager.MainInstance.CallEvent(
                        EventHash.OnCharacterHealing,
                        effectCount,
                        "",
                        hitCollider.transform
                    );
                }
            }
        }
    }

    ////范围物理检测的返回绘制
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(c1, radius);

    //}
}
