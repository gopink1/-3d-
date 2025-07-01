using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillBar
{
    Q,
    E
}
public class PlayerCombatControl : CharacterCombatBase
{
    public AudioCue atkcue;
    private Transform m_camera;//摄像机
    private PlayerAttr m_attr;//玩家的属性
    private IWeapon m_mainWeapon;//主武器
    private IWeapon curAtkWeapon;//当前武器
    [SerializeField] private GameObject WeaponPos;//武器的位置

    //[SerializeField, Header("武器连招表")] private CharacterComboSO m_MainWeaponCombo;
    //[SerializeField, Header("武器连招表")] private CharacterComboSO m_RangedWeaponCombo;

    //攻击检测相关
    [SerializeField] private List<Collider> detectColliderslist;//范围内所有的敌人碰撞体
    [SerializeField, Header("检测范围")] private float detecteRange;
    [SerializeField, Header("检测的层级")] private LayerMask detectLayer;
    [SerializeField, Header("检测间隔")] private float detectionInterval;
    private float nextDetectionTime;

    //攻击范围
    [SerializeField, Header("攻击距离")] private float attackRange = 1.7f;
    [SerializeField, Header("攻击角度dot范围")] private float attackAngle = 0.6f;

    //视角锁定
    private bool isLockView = false;

    //技能系统相关属性
    private InputHandler inputHandler;//责任链模式的起点
    Dictionary<TriggerType, ISkillTriggerStrategy> allSkillStrategy;//所有技能策略


    private bool QCooling;//技能Q的是否冷却中
    private bool ECooling;//技能E是否冷却中
    Dictionary<SkillBar, SkillData> equipedSkill;//已经装备的技能
    public Dictionary<SkillBar,SkillData> EqiupedSkil
    {
        get { return equipedSkill; }
    }

    protected override void Awake()
    {
        base.Awake();
        if (Camera.main != null) m_camera = Camera.main.transform;
        //初始化技能系统
        //初始化责任链
        InitChain();
        //初始化所有策略
        InitStrategy();
        //初始化技能（测试）
        InitSkillData();
    }
    /// <summary>
    /// 初始化责任链
    /// </summary>
    private void InitChain()
    {
        inputHandler = new InstantTriggerHander(gameObject);
        ChargeTriggerHander h2 = new ChargeTriggerHander(gameObject);
        AimTriggerHander am2 = new AimTriggerHander(gameObject);
        inputHandler.SetNextHander(h2);
        h2.SetNextHander(am2);
    }

    private void InitStrategy()
    {
        allSkillStrategy = new Dictionary<TriggerType, ISkillTriggerStrategy>();
        allSkillStrategy.Add(TriggerType.MagicalBall,new MagicalBallTriggerStrategy(animator));
        allSkillStrategy.Add(TriggerType.BuffOwn, new UpOwnTriggerStrategy(animator));
        allSkillStrategy.Add(TriggerType.ChargeCombo, new ChargeActionStrategy(animator));
        allSkillStrategy.Add(TriggerType.MagicalCircle, new MagicalCircleTriggerStrategy(animator));
    }

    private void InitSkillData()
    {
        equipedSkill = new Dictionary<SkillBar, SkillData>();
        //MagicalBallSkillData s0 = new MagicalBallSkillData(0, 20f, 10f, 20f, "fireball", InputType.Instant, TriggerType.MagicalBall,
        //"ComboSO/Player/FireCombo/FireBallCombo0",
        //"Icon/IceFire",
        //"SkillPrefab/IceFire",
        //"发射火球碰到敌人会爆炸",
        //10f);
        //MagicalCircleSkillData s1 = new MagicalCircleSkillData(1, Vector3.zero, 5f, 4f, "hp+10f/s", 10f, 0.95f, "HealingCircle", InputType.Instant, TriggerType.MagicalCircle,
        //"ComboSO/Player/HealingCircle/HealingCircleCombo0",
        //"Icon/HealingCircle",
        //"SkillPrefab/HealingCircle",
        //"引导治疗法阵",
        //10f);

        //BuffSkillData s2 = new BuffSkillData(2, 6f, "atk + 10f", 10f, "upAtkBuff", InputType.Instant, TriggerType.BuffOwn,
        //"ComboSO/Player/UpOwnCombo/UpOwnAttrCombo0",
        //"Icon/Buff",
        //"SkillPrefab/Buff", 
        //"加攻击力的buff，开启后加10点攻击力",
        //15f);

        //ChargeComboSkillData s3 = new ChargeComboSkillData(3, 2.5f, new Vector3(0, 0.9f, 1.5f), "ChargeHeavyHoriAtk", InputType.Charge, TriggerType.ChargeCombo,
        //"ComboSO/Player/ChargeHeavyHoriAtk/ChargeHeavyHoriAtkCombo0",
        //"Icon/Electro slash",
        //"SkillPrefab/Electro slash",
        //"蓄力攻击蓄力时间越长效果次数越多",
        //5f);
        //MagicalCircleSkillData s4 = new MagicalCircleSkillData(4, new Vector3(0, 0, 4f), 5f, 4f, "damage+20f", 20f, 0f, "RedEnergyCircle", InputType.Instant, TriggerType.MagicalCircle,
        //"ComboSO/Player/RedEnergyCircle/RedEnergyCircleCombo0",
        //"Icon/RedEnergyCircle",
        //"SkillPrefab/RedEnergyCircle",
        //"引导红色血石砸下", 
        //5f);
        equipedSkill.Add(SkillBar.Q, null);
        equipedSkill.Add(SkillBar.E, null);
    }
    #region 装备技能
    public void EquipSkillCard(SkillBar bar,SkillData data)
    {
        //替换现在的技能
        equipedSkill[bar] = data;
    }
    #endregion

    protected override void Start()
    {
        base.Start();
        //初始化武器
        WeaponFactory factory = GameBaseFactory.GetWeaponFactory() as WeaponFactory;
        m_mainWeapon = factory.CreateSword("S0_1");
        //m_rangedWeapon = factory.CreateIceFire("F_I") as RangedWeapon;
        m_attr = GetComponent<PlayerHealthyControl>().GetAttr();
    }
    protected override void OnEnable()
    {

    }
    protected override void OnDisable()
    {

    }
    protected override void Update()
    {
        base.Update();
        PlayerSkillInput();
        CharacterBaseComboInput();//玩家输入
        LockViewInput();
        //ClearCurrentEnemy();
    }

    private void PlayerSkillInput()
    {
        if (equipedSkill == null) return;
        if (!CanAttackInput()) return;
        if (Time.timeScale == 0) return;
        if (GameInputManager.MainInstance.SkillQ)
        {
            if (QCooling) return;
            //根据技能当前位置技能类型选择正确的输入方式
            InputHanderResult result = inputHandler.HandleInput(equipedSkill[SkillBar.Q],0);
            if(result != null)
            {
                //判断返回结果
                if (result.IsCharged)
                {
                    //如果正在蓄力，则进行不可攻击的设置

                }
                else
                {
                    TriggerSkill(result);
                }
            }
        }
        if (GameInputManager.MainInstance.SkillE)
        {
            if (ECooling) return;
            //根据技能当前位置技能类型选择正确的输入方式
            InputHanderResult result = inputHandler.HandleInput(equipedSkill[SkillBar.E],1);
            if (result != null)
            {
                //判断返回结果
                if (result.IsCharged)
                {
                    //如果正在蓄力，则进行不可攻击的设置

                }
                else
                {
                    TriggerSkill(result);
                }
            }
        }
    }
    //用于触发技能
    public void TriggerSkill(InputHanderResult result)
    {
        //根据结果进行触发技能效果
        //根据结果选择策略进行使用技能
        allSkillStrategy[result.SkillData.TriggerType].triggerSkill(result);
        //触发技能后设置为不可攻击状态
        if(result.InventoryIndex == 0)
        {
            QCooling = true;
        }
        else if (result.InventoryIndex == 1)
        {
            ECooling = true;
        }
        TimerManager.MainInstance.TryEnableOneGameTimer(result.SkillData.CoolTime,()=> {
            ResetSkillCoolTime(result.InventoryIndex, result.SkillData);
            });
    }
    private void ResetSkillCoolTime(int skillInventory,SkillData data)
    {
        switch (skillInventory)
        {
            case 0:
                //q
                QCooling = false;
                break;
            case 1:
                //e
                ECooling = false;
                break;
        }

    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        RangeDetectionTarget();//检测附近敌人
    }

    /// <summary>
    /// 设置武器
    /// </summary>
    /// <param name="weapon"></param>
    public void SetMainWeapon(IWeapon weapon)
    {
        //传入一个武器
        m_mainWeapon = weapon;
        ////m_MainWeaponCombo = weapon.GetWeaponAtkSO();//设置动作表

        //设置animator
        //获取animator的名字
        string animatorcontroler = weapon.GetAnimatorName();
        //从资源工厂中加载出来animator进行替换
        ResourcesAssetFactory factory =  GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        Debug.Log(animatorcontroler);
        //animator.runtimeAnimatorController = factory.LoadAnimator("Character/Animator/" + animatorcontroler);

        if (WeaponPos.transform.childCount != 0)
        {
            Destroy(WeaponPos.transform.GetChild(0).gameObject);
        }
        Instantiate(m_mainWeapon.GetModel(),WeaponPos.transform);
    }
    /// <summary>
    /// 玩家需要获取多个敌人
    /// 所以重写一下
    /// </summary>
    public override void CharacterBaseComboInput()
    {
        if (m_mainWeapon == null) return;
        if (!CanAttackInput()) return;
        if (Time.timeScale == 0) return;
        if (GameInputManager.MainInstance.Latk)
        {
            if (currentCombo == null || currentCombo != m_mainWeapon.GetWeaponAtkSO())
            {
                currentCombo = m_mainWeapon.GetWeaponAtkSO();
                curAtkWeapon = m_mainWeapon;
                ResetCombo();
            }
            ExecuteShortComboAction();
        }
    }
    /// <summary>
    /// 执行连招部分、
    /// 执行动作表中的动作
    /// </summary>
    protected override void ExecuteShortComboAction()
    {
        //当初发攻击后是当前comboCount
        currentComboCount += 1;
        hitIndex = 0;
        //需要先判断索引值是否为范围内
        if (comboIndex >= currentCombo.TryGetComboMaxCount())
        {
            comboIndex = 0;
            currentComboCount = 0;
        }
        maxColdTime = currentCombo.TryGetColdTime(comboIndex);//获取冷却时间
        animator.CrossFadeInFixedTime(currentCombo.TryGetOneComboAction(comboIndex), 0.155555f, 0, 0.0f);

        //触发音效
        curAtkWeapon.Attack();

        //UpdatePlayerRotate();
        TimerManager.MainInstance.TryEnableOneGameTimer(maxColdTime, UpdateComboInfo);
        canAttack = false;
    }

    protected override void UpdateComboInfo()
    {
        comboIndex++;
        maxColdTime = 0;
        canAttack = true;
        if (comboIndex == currentCombo.TryGetComboMaxCount())
        {
            currentComboCount = 0;
        }
    }
    protected override void TriggerAttack()
    {
        //判断当前是否有敌人需要触发攻击
        if (currentEnemy == null) return;
        if (detectColliderslist.Count == 0) return;

        //遍历所有敌人，查找符合条件的敌人
        for(int i = 0; i < detectColliderslist.Count; i++)
        {
            if (detectColliderslist[i] == null) continue;
            if (IsCanAttack(detectColliderslist[i].gameObject.transform) && 
                animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                //触发攻击
                //目前说明敌人就在攻击范围内，触发敌人的生命系统
                GameEventManager.MainInstance.CallEvent(EventHash.OnCharacterHit,
                currentCombo.TryGetDmage(comboIndex) + m_attr.Atk * atkMultiplier,
                currentCombo.TryGetOneHitComboAction(comboIndex, hitIndex),
                transform,
                detectColliderslist[i].gameObject.transform
                );//触发攻击
                //Debug.Log(detectColliderslist[i].gameObject.transform.GetInstanceID()+ "1111111111111111111");
            }
        }
    }
    protected bool IsCanAttack(Transform trans)
    {
        return IsInAttackAngle(trans)&& IsInAttackRange(trans);
    }
    protected bool IsInAttackRange(Transform trans)
    {

        var dot = Vector3.Dot(transform.forward, GetVector3(trans.position, transform.position));

        if (dot < attackAngle) return false;//角度范围

        //获取到武器属性中的攻击距离
        return true;

    }
    protected bool IsInAttackAngle(Transform trans)
    {
        if (GetDistance(trans.transform.position, transform.position) > attackRange) return false;//距离范围
        return true;
    }

    #region 检测
    /// <summary>
    /// 范围检测
    /// 球星范围检测，检测currentEnemy
    /// </summary>
    private void RangeDetectionTarget()
    {
        //范围检测每帧执行
        if (Time.time < nextDetectionTime)
        {
            return;
        }
        //清空之前的
        Collider[] detectColliders;
        detectColliderslist.Clear();
        //检测圈中碰撞体
        detectColliders =  Physics.OverlapSphere(transform.position + transform.up * 0.7f, detecteRange, detectLayer, QueryTriggerInteraction.Ignore);
        if (detectColliders != null)
        {
            float dis = Mathf.Infinity;
            Transform nowEnemy = null;
            foreach (Collider collider in detectColliders)
            {
                float i = GetDistance(collider.transform.position, transform.position);
                if (i < dis)
                {
                    dis = i;
                    nowEnemy = collider.transform;
                }
                detectColliderslist.Add(collider);
            }
            currentEnemy = nowEnemy;
        }
        nextDetectionTime = Time.time + detectionInterval;
    }
    private void OnDrawGizmos()
    {
        //画出检测范围检测
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.7f, detecteRange);

        //画出攻击范围
        //计算攻击范围的终点
        //通过cos值计算角度，然后角度转为弧度值，弧度值
        //通过四元数的静态方法AngleAxis获取四元数,传入弧度和旋转轴
        //然后向量相乘得到旋转目标的向量
        //右边界四元数
        Quaternion rightQua = Quaternion.AngleAxis(Mathf.Acos(attackAngle) * Mathf.Rad2Deg, transform.up);
        //左边界四元数
        Quaternion leftQua = Quaternion.AngleAxis(Mathf.Acos(attackAngle) * Mathf.Rad2Deg, -transform.up);
        Gizmos.DrawLine(transform.position + transform.up * 0.7f, transform.position + transform.up * 0.7f + transform.forward * 1.5f);
        Gizmos.DrawLine(transform.position + transform.up * 0.7f, transform.position + transform.up * 0.7f + (rightQua * transform.forward) * 1.5f);
        Gizmos.DrawLine(transform.position + transform.up * 0.7f, transform.position + transform.up * 0.7f + (leftQua * transform.forward) * 1.5f);

    }
    #endregion

    #region 锁定敌人视角

    private void LockViewInput()
    {
        if(GameInputManager.MainInstance.LockView)
        {
            if (currentEnemy == null) return;
            //检测到锁定按键的输入获取物理检测的敌人
            //根据距离最近的敌人进行锁定
            isLockView = !isLockView;
            GameEventManager.MainInstance.CallEvent<Transform, bool>(EventHash.OnCameraViewLock, currentEnemy, isLockView);

        }
    }
    #endregion

}

