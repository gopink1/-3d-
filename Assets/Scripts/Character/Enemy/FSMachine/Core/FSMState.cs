using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlExtensions;

public abstract class FSMState
{
    protected FSMachine m_Mchine = null;

    /// <summary>
    /// ��ǰ״̬������ת��
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
            Debug.Log("�Ѿ�����" + t +"��������滻");
            allTrans[t] = s;
        }
    }

    /// <summary>
    /// �����ж�
    /// </summary>
    public abstract void Reason();
    /// <summary>
    /// ״̬��Ϊ
    /// </summary>
    public abstract void Act();
    /// <summary>
    /// ״̬�״ν���
    /// </summary>
    public abstract void Enter();
    public abstract void Exit(); 
}
