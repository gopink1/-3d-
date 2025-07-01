using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlExtensions;

public abstract class FSMState
{
    protected FSMachine m_Mchine = null;

    /// <summary>
    /// 当前状态的所有转换
    /// </summary>
    protected Dictionary<Transition, Enemy_State> allTrans = new Dictionary<Transition, Enemy_State>();
    public Dictionary<Transition, Enemy_State> Trans
    {
        get { return allTrans; }
    }

    public FSMState(FSMachine mchine)
    {
        m_Mchine=mchine;
    }
    public void SetMachine(FSMachine mchine)
    {
        m_Mchine = mchine;
    }
    public void AddTransition(Transition t,Enemy_State s)
    {
        if(!allTrans.ContainsKey(t))
        {
            allTrans.Add(t, s);
        }
        else
        {
            Debug.Log("已经含有" + t +"对其进行替换");
            allTrans[t] = s;
        }
    }

    /// <summary>
    /// 条件判断
    /// </summary>
    public abstract void Reason();
    /// <summary>
    /// 状态行为
    /// </summary>
    public abstract void Act();
    /// <summary>
    /// 状态首次进入
    /// </summary>
    public abstract void Enter();
    public abstract void Exit(); 
}
