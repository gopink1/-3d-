using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAliveStateScore : IStageScore
{
    private float AliveTime;    //存货时间

    bool first = true;

    private float timer = 0.0f;
    private int lastIntTime = 0;

    private StageSystem m_stageSys;
    public PlayerAliveStateScore(float time, StageSystem stage)
    {
        AliveTime = time;
        m_stageSys = stage;
        InitScore();
    }
    public override void InitScore()
    {

        timer = 0.0f;
        lastIntTime = 0;
        first = true;


    }
    public override bool CheckScore()
    {
        GameEventManager.MainInstance.CallEvent(EventHash.UpdateStageText, "生存" + AliveTime + "S");
        if (m_stageSys.GetAliveTime > AliveTime)
        {
            return true;
        }
        //返回一个bool值
        //判断是否
        return false;
    }

    public override void Release()
    {
        
    }

}
