using BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// boss的combo系统，因为对于boss不需要使用状态机
/// 而是直接使用行为树控制
/// </summary>
public class EnemyBossComboControl : CharacterCombatBase
{
    //攻击检测相关
    [SerializeField] private Collider[] detectColliders;
    [SerializeField, Header("检测范围")] private float detecteRange;
    [SerializeField, Header("检测的层级")] private LayerMask detectLayer;
    [SerializeField, Header("警戒距离")] private float awardDistance;

    [SerializeField, Header("进入中距离攻击距离")] private float mdatkDistance;

    [SerializeField, Header("进入近距离攻击距离")] private float sdatkDistance;

    [SerializeField] private Dictionary<string, CharacterComboSO> m_ComboSO = new Dictionary<string, CharacterComboSO>();
    [SerializeField] private List<CharacterComboSO> m_ShortCombo = new List<CharacterComboSO>();
    [SerializeField] private List<CharacterComboSO> m_LongCombo = new List<CharacterComboSO>();
    private List<CharacterComboSO> m_currentCombos;
    private bool isLongCombo;

    private bool canChangeCombo;           //是否更换combo，用于标定当前连招是否已经全部执行，或者被打断等，需要切换combo
    public bool CanChangeComb0
    {
        get => canChangeCombo;
    }
    private bool atkCommand;
    public bool AtkCommand
    { 
        get => atkCommand;
        set => atkCommand = value;
    }

    protected override void Start()
    {
        base.Start();
        canChangeCombo = true;

    }
    #region 设置连招相关
    //用于把combo进行解析，即把comboso中的连招进行分类初始化
    public void InitComboSO(Dictionary<string, CharacterComboSO> sos)
    {

        //初始化so表
        m_ComboSO = sos;
        //把so表的招式进行分离
        foreach(string ci in m_ComboSO.Keys)
        {
            if (ci[3] == 'S')
            {
                m_ShortCombo.Add(m_ComboSO[ci]);
            }
            else if (ci[3] =='L')
            {
                m_LongCombo.Add(m_ComboSO[ci]);
            }
        }
    }

    /// <summary>
    /// 把当前连招设置为近距离连招
    /// </summary>
    public void SetComboAsShort()
    {
        if (m_currentCombos == m_ShortCombo) return;
        m_currentCombos = m_ShortCombo;
        isLongCombo = false;
    }
    /// <summary>
    /// 把当前连招设置为远距离
    /// </summary>
    public void SetComboAsLong()
    {
        if (m_currentCombos == m_LongCombo) return;
        m_currentCombos = m_LongCombo;
        isLongCombo = true;
    }
    /// <summary>
    /// 随机一个combo
    /// </summary>
    public void SetRandomCombo()
    {
        //获取随机数
        int randomnum = Random.Range(0, m_currentCombos.Count);
        for(int i = 0; i < m_currentCombos.Count; i++)
        {
            if(i == randomnum)
            {
                currentCombo = m_currentCombos[i];
                canChangeCombo = false;
            }
        }
    }

    
    #endregion

    #region 攻击触发相关

    #region 执行攻击行为
    private bool CanAtk()
    {
        if (!canAttack) return false;
        if (!LockPlayer()) return false;
        if (currentEnemy == null) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        return true;
    }
    public void BossComboInput()
    {
        if (!CanAtk()) return;

        if(currentCombo == null)
        {
            //为空说明未正确初始化
            Debug.LogError("currentCombo为空敌人"+gameObject+"的行为树combo触发错误");
        }
        ExecuteShortComboAction();
    }

    protected override void ExecuteShortComboAction()
    {
        currentComboCount += 1;
        hitIndex = 0;
        //需要先判断索引值是否为范围内
        if (comboIndex >= currentCombo.TryGetComboMaxCount())
        {
            comboIndex = 0;
            currentComboCount = 0;
            //Debug.Log("qqq");
        }
        maxColdTime = currentCombo.TryGetColdTime(comboIndex);//获取冷却时间
        animator.CrossFadeInFixedTime(currentCombo.TryGetOneComboAction(comboIndex), 0.155555f, 0, 0.0f);
        //UpdatePlayerRotate();
        TimerManager.MainInstance.TryEnableOneGameTimer(maxColdTime, UpdateComboInfo);
        canAttack = false;
    }
    protected override void UpdateComboInfo()
    {
        base.UpdateComboInfo();
        comboIndex++;
        maxColdTime = 0;
        canAttack = true;
        if (comboIndex >= currentCombo.TryGetComboMaxCount())
        {
            comboIndex = 0;
            canChangeCombo = true;
            atkCommand = false;
        }
    }
    #endregion

    #region 攻击判定相关

    protected override void TriggerAttack()
    {
        //触发攻击，判断玩家距离和角度，然后触发受伤事件
        if (currentEnemy == null) return;
        if (IsCanAttack(currentEnemy) && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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
    protected bool IsCanAttack(Transform trans)
    {
        return IsInAttackAngle(trans)&& IsInAttackRange(trans);
    }
    protected bool IsInAttackAngle(Transform trans)
    {

        var dot = Vector3.Dot(transform.forward, GetVector3(trans.position, transform.position));

        if (dot < currentCombo.TryGetComboAngleRange(comboIndex)) return false;//角度范围

        //获取到武器属性中的攻击距离
        return true;

    }
    protected bool IsInAttackRange(Transform trans)
    {
        if (GetDistance(trans.transform.position, transform.position) > currentCombo.TryGetComboAtkRange(comboIndex)) return false;//距离范围
        return true;
    }
    #endregion

    #endregion


    #region 获取敌人的方法
    public bool InAwardRange()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < awardDistance 
            && GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) > mdatkDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        }
        return false;
    }
    public bool InLongAtkRange()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < mdatkDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        } 
        return false;
    }
    public bool InShortAtkRange()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < sdatkDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position+ transform.up * 0.7f, sdatkDistance);
        Gizmos.DrawWireSphere(transform.position+ transform.up * 0.7f, mdatkDistance);
        Gizmos.DrawWireSphere(transform.position+ transform.up * 0.7f, awardDistance);
    }
    public bool LockPlayer()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < awardDistance)
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
            currentEnemy  = GameBase.MainInstance.GetMainPlayer().transform;
        }
        return currentEnemy;
    }
    #endregion
    protected override void UpdateCharacterRotate()
    {
        if (currentEnemy == null) return;
        //攻击匹配旋转
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsTag("Attack") && currentState.normalizedTime < 0.3f)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);
            // 平滑旋转到目标旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

            //要跟主角保持距离
        }
        //远距离需要位置
        if (currentState.IsTag("Attack")
            && isLongCombo
            && currentState.normalizedTime < currentCombo.TryGetComboMatchTime(comboIndex).y
            && currentState.normalizedTime > currentCombo.TryGetComboMatchTime(comboIndex).x)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);
            // 平滑旋转到目标旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
            Vector3 targetpos = currentEnemy.position + (-transform.forward * currentCombo.TryGetComboPositionOffset(comboIndex));
            //匹配位置信息
            //匹配到角色在前百分之三十的旋转方向上的旋转*面对方向再减去当前的连招里的offset
            //判断攻击敌人是否再范围内如果在范围内进行匹配

            animator.MatchTarget(targetpos,
                Quaternion.identity,
                AvatarTarget.Root,
                new MatchTargetWeightMask(Vector3.one, 0),
                currentCombo.TryGetComboMatchTime(comboIndex).x,
                currentCombo.TryGetComboMatchTime(comboIndex).y);
        }
    }

}
