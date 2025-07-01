using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalCircleControl : MonoBehaviour
{
    protected Vector3 center;
    protected float aliveTime;
    protected float timer;
    protected float effectCount;
    protected float effectRate;




    public virtual void Init(Vector3 bornPos,float aliveTime,float effectCount,float effectRate)
    {
        center = bornPos;
        transform.position = center;
        this.aliveTime = aliveTime;
        this.effectCount = effectCount;
        this.effectRate = effectRate;
    }


    protected virtual void Update()
    {
        timer += Time.deltaTime;
        UpdateWithTimer();
        if(timer > aliveTime)
        {
            timer = 0;
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 每帧跟新一般用于timer的时间判断
    /// </summary>
    public virtual void UpdateWithTimer()
    {
        
    }

    



}
