using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStageHander
{
    protected IStageData m_stageData = null;      //关卡的内容
    protected IStageScore m_stageScore = null;    //通关的条件
    protected IStageHander m_nextHander = null;   //下一个关卡
    
    public IStageHander SetNextHander(IStageHander nextHander)
    {
        m_nextHander = nextHander;
        return m_nextHander;
    }
    public IStageHander GetNextHnader()
    {
        if(m_nextHander == null)
        {
            Debug.Log("m_nextHander为空");
        }
        return m_nextHander;
    }

    /// <summary>
    /// 初始化关卡
    /// 调用关卡内容的初始化方法
    /// </summary>
    public abstract void InitStage();
    /// <summary>
    /// 更新关卡
    /// </summary>
    public abstract void UpdateStage();
    /// <summary>
    /// 查询关卡
    /// 查看是否通过
    /// </summary>
    public abstract bool CheckStage();

    public abstract void Release();

    public abstract void ApplyStage( bool apply);
}
