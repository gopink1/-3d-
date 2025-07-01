using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyCombatControl : CharacterCombatBase
{
    private FSMachine m_Machine = null;
    //攻击检测相关
    [SerializeField] private Collider[] detectColliders;
    [SerializeField, Header("检测范围")] private float detecteRange;
    [SerializeField, Header("检测的层级")] private LayerMask detectLayer;
    [SerializeField, Header("警戒距离")] private float awardDistance;
    [SerializeField, Header("进入攻击距离")] private float atkDistance;

    [SerializeField, Header("具体攻击距离")] private float atkRange;
    [SerializeField, Header("攻击角度距离")] private float atkAngle;
    //角色出招表，敌人的攻击表，当角色状态机控制角色的行为的时候
    [SerializeField] private Dictionary<string, CharacterComboSO> m_ComboSO = new Dictionary<string, CharacterComboSO>();
    [SerializeField] private CharacterComboSO m_Combo;



    //private EnemyAttr m_attr = null;

    //public void InitHealthyAttr(EnemyAttr attr)
    //{
    //    m_attr = attr;
    //}
    public void InitComboSO(Dictionary<string, CharacterComboSO> sos)
    {
        m_ComboSO = sos;
    }
    public float AwardDistance
    {
        get { return awardDistance; }
    }
    public float AtkDiatance
    {
        get { return atkDistance; }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        m_Machine.SetState(Enemy_State.Idle);
        //InitMachine();
    }
    public void InitMachines(Dictionary<Enemy_State,FSMState> states,MassageQueue massageQueue)
    {
        //初始化状态机
        m_Machine = new FSMachine(gameObject, animator, massageQueue);

        //初始化DIC状态列表
        foreach (var i in states.Keys)
        {
            states[i].SetMachine(m_Machine);    //设置状态机到每个状态
            m_Machine.AddState(i, states[i]);
        }

    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        //查看消息队列中的消息


        if(m_Machine != null)
        {
            m_Machine.StateUpdate();
        }
    }
    public bool LockPlayer()
    {
        if(GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < AwardDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        }
        return false;
    }
    public Transform GetCurEnemy()
    {
        if (currentEnemy == null)
        {
            currentEnemy = currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
        } 
        return currentEnemy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.7f, detecteRange);
    }

    protected override void TriggerAttack()
    {
        //触发攻击，判断玩家距离和角度，然后触发受伤事件
        if (currentEnemy == null) return;
        if(IsCanAttack(currentEnemy) && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //触发攻击
            //目前说明敌人就在攻击范围内，触发敌人的生命系统
            GameEventManager.MainInstance.CallEvent(EventHash.OnCharacterHit,
            currentCombo.TryGetDmage(comboIndex) * atkMultiplier,
            currentCombo.TryGetOneHitComboAction(comboIndex, hitIndex),
            transform,
            currentEnemy.gameObject.transform
            );//触发攻击
            Debug.Log(comboIndex +"-----" + hitIndex);
            Debug.Log(currentCombo.TryGetOneHitComboAction(comboIndex, hitIndex));
        }
    }

    /// <summary>
    /// 执行连招行为
    /// 执行常规平A
    /// </summary>
    public void ExecuteNormalAtkAction()
    {
        //查询当前玩家
        if (!CanAtk()) return;
        //说明当前有敌人可以进攻
        //触发玩家的动作
        //随机触发连招表的招式
        //随机一个正数

        //随机一个数字
        int count = m_ComboSO.Count;
        Debug.Log("000000000000000000000000000000000"+count);
        int index = UnityEngine.Random.Range(0, count);
        ChangeComboSO(GetOneCombo(index));


        maxColdTime = currentCombo.TryGetColdTime(comboIndex);
        animator.CrossFadeInFixedTime(currentCombo.TryGetOneComboAction(comboIndex), 0.1555555f, 0, 0.0f);
        TimerManager.MainInstance.TryEnableOneGameTimer(maxColdTime, UpdateComboInfo);
        canAttack = false;
    }
    /// <summary>
    /// 随机获取一个连招
    /// </summary>
    public CharacterComboSO GetOneCombo(int index)
    {
        int ci = 0;
        foreach (var combo in m_ComboSO)
        {
            if(ci == index)
            {
                Debug.Log(combo.Value + "随机触发招式名字");
                return combo.Value;
            }
            ci++;
        }
        Debug.LogWarning("错误没有随机正确" +  ci + "超出了正确的范围");
        return null;
    }
    private bool CanAtk()
    {
        if (!canAttack) return false;
        if (!LockPlayer()) return false;
        if (currentEnemy == null) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        return true;
    }
    protected override void UpdateComboInfo()
    {
        base.UpdateComboInfo();
        comboIndex = 0;
        maxColdTime = 0;
        canAttack = true;
    }
    protected bool IsCanAttack(Transform trans)
    {
        return IsInAttackAngle(trans)&& IsInAttackRange(trans);
    }
    protected bool IsInAttackRange(Transform trans)
    {

        var dot = Vector3.Dot(transform.forward, GetVector3(trans.position, transform.position));

        if (dot < currentCombo.TryGetComboAngleRange(comboIndex)) return false;//角度范围

        //获取到武器属性中的攻击距离
        return true;

    }
    protected bool IsInAttackAngle(Transform trans)
    {
        if (GetDistance(trans.transform.position, transform.position) > currentCombo.TryGetComboAtkRange(comboIndex)) return false;//距离范围
        return true;
    }

    /// <summary>
    /// 设置状态机状态
    /// </summary>
    /// <param name="trans"></param>
    public void SetMachine(Transition trans)
    {
        m_Machine.SetTransition(trans);
    }
}
