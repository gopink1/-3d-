using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 /// <summary>
 /// ���˵�״̬
 /// </summary>
public enum Enemy_State
{
    Idle,
    Attack,
    Move,
    Victory,
    Hit,
    Dead,

}
/// <summary>
/// ת������
/// </summary>
public enum Transition
{
    Init,      //Idle
    FindPlayer,//Idle TO Move
    LostTarget, //Move TO Idle
    Attack,     //Move/Idle TO Attack
    Hit,        //Move/Idle TO Hit
    Die,         //Hit TO Die
    Revive      //Die TO Idle
}
/// <summary>
/// ״̬����
/// </summary>
public class FSMachine
{
    private GameObject m_Owner = null;
    public GameObject Owner
    {
        get { return m_Owner; }
    }
    
    private Animator m_Animator;
    public Animator Animator
    {
        get { return m_Animator; }
    }

    private Dictionary<Enemy_State,FSMState> m_allState = null;
    private FSMState currentState;

    private MassageQueue m_Queue;

    public FSMachine(GameObject owner,Animator animator, MassageQueue queue)
    {
        m_Owner=owner;
        m_Animator = animator;
        m_allState = new Dictionary<Enemy_State, FSMState>();
        m_Queue=queue;
    }

    public void AddState(Enemy_State enumstate,FSMState state)
    {
        if (!m_allState.ContainsKey(enumstate))
        {
            m_allState.Add(enumstate, state);
        }
        else
        {
            Debug.Log("�Ѿ�����" +  enumstate+"��������滻");
            m_allState[enumstate] = state;
        }
    }

    public void SetState(Enemy_State state)
    {
        if (!m_allState.ContainsKey(state))
        {
            Debug.Log("û��Ŀ��״̬"+ state);
            return;
        }
        else
        {
            currentState = m_allState[state];
            currentState.Enter();
        }

    }
    /// <summary>
    /// ����ת��
    /// </summary>
    /// <param name="trans"></param>
    public void SetTransition(Transition trans)
    {
        //��ȡ����ǰ��״̬ͨ���״̬����
        Enemy_State targetState = currentState.Trans[trans];//��ȡĿ��״̬
        //�˳���ǰ״̬
        if(currentState != null)
        {
            currentState.Exit();
        }
        //��ȡĿ��״̬
        currentState =  m_allState[targetState];
        m_allState[targetState].Enter();
        Debug.Log("״̬�л���"+currentState.ToString());
    }

    public void StateUpdate()
    {
        Message message;
        while (m_Queue.Dequeue(out message))
        {
            if (message.currentHealth <= 0f && (currentState.GetType() != typeof(DeadState)))
            {
                SetTransition(Transition.Die);
                break;
            }
            //Ϊ����״̬�������˶���������
            if (m_allState.ContainsKey(Enemy_State.Hit))
            {
               (m_allState[Enemy_State.Hit] as HitState).SetHitName(message.hitName);
            }
            SetTransition(Transition.Hit);
        }
        currentState.Reason();
        currentState.Act();
    }

}
