using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
/// <summary>
/// 敌人构建的参数
/// </summary>
public class EnemyBuildParm
{
    //敌人的基础属性
    private int index;      //对应的敌人类型代号
    private string name;    //敌人名字
    Dictionary<string,CharacterComboSO> comboData;    //敌人所需要的技能<连招index，连招数据>

    public Dictionary<string, CharacterComboSO> ComboData
    {
        get => comboData;
    }

    //敌人的状态机初始化数据
    public EnemyBuildParm(int index, string name)
    {
        this.index=index;
        this.name=name;
        comboData = new Dictionary<string, CharacterComboSO>();
    }
    public void InitComboData(Dictionary<string, CharacterComboSO> comboData)
    {
        this.comboData = comboData;
    }
    public void AddComboIndex(string comboIndex)
    {
        // 只添加键，不添加值
        if (!comboData.ContainsKey(comboIndex))
        {
            comboData.Add(comboIndex, null);
        }
    }

    public int Index
    {
        get { return index; }
    }
    public string Name
    {
        get { return name; }
    }

}

/// <summary>
/// 连招数据
/// </summary>
public class ComboData
{
    string comboIndex;  //连招的代号
    string comboName;   //连招的名字
    string comboPath;   //连招的地址
    public ComboData(string comboIndex, string comboName, string comboPath)
    {
        this.comboIndex=comboIndex;
        this.comboName=comboName;
        this.comboPath=comboPath;
    }

    public string ComboIndex
    {
        get { return comboIndex; }
    }
    public string ComboName
    {
        get { return comboName; }
    }
    public string ComboPath
    {
        get { return comboPath; }
    }
}

/// <summary>
/// 初始化敌人状态机的参数
/// 包含者一个类型敌人创建状态机
/// 初始化状态机所需要的参数信息
/// </summary>
public class EnemyFSMParm
{
    //状态信息
    //Dictionary<状态，Dictionary<状态转换条件，转换转入的状态>>
    private Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> statesMassage;
    //构造函数构建信息
    //一般读取json文件
    public EnemyFSMParm(Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> list)
    {
        this.statesMassage=list;
    }
    //获取信息
    public Dictionary<Enemy_State, Dictionary<Transition,Enemy_State>> GetList()
    {
        return statesMassage;
    }
}

public class CharacterFactory : ICharacterFactory
{
    Dictionary<int, EnemyBuildParm> m_enemyType = new Dictionary<int, EnemyBuildParm>();

    Dictionary<int, EnemyFSMParm> m_enemyFSM = new Dictionary<int, EnemyFSMParm>();

    Dictionary<string,CharacterComboSO> m_ComboMap = new Dictionary<string, CharacterComboSO>();    //连招的表，键为招式的id，值读取到的招式本体
    public CharacterFactory()
    {
        InitComboData();
        InitEnemyType();
        InitEnemyFSM();
    }

    private void InitComboData()
    {
        //初始化连招数据
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<string,ComboData>> jsonParse =  factory.CreateParser<Dictionary<string, ComboData>>();
        Dictionary<string, ComboData> m_enemyComboData = jsonParse.ParseJsonContext(JsonCfgName.EnemyComboCfg);//敌人，敌人对应的

        //读取相应的数据后，需要进行在资源文件中进行load然后进行存储
        //加载工厂
        ResourcesAssetFactory factory1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;

        foreach(var cb in m_enemyComboData)
        {
            //从地址中加载so表
            CharacterComboSO so = factory1.LoadSO(cb.Value.ComboPath) as CharacterComboSO;
            if (!m_ComboMap.ContainsKey(cb.Key))
            {
                m_ComboMap.Add(cb.Key, so);
            }
            else
            {
                Debug.LogWarning("已经含有代号为"+cb.Key+"招式配置");
            }

        }

    }

    /// <summary>
    /// 初始化敌人的基础信息
    /// </summary>
    private void InitEnemyType()
    {
        //添加RedShit的构建信息
        //初始化配置信息
        JsonParseFactory factory =  GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<int, EnemyBuildParm>> context =  factory.CreateParser<Dictionary<int, EnemyBuildParm>>();
        m_enemyType = context.ParseJsonContext(JsonCfgName.CharacterBuilderCfg);

        //添加敌人类型配置中的ComboData数据
        foreach (var enemyEntry in m_enemyType)
        {
            int enemyIndex = enemyEntry.Key;
            EnemyBuildParm enemyBuildParm = enemyEntry.Value;
            var comboIndices = enemyBuildParm.ComboData.Keys.ToArray(); // 转换为数组

            for (int i = 0; i < comboIndices.Length; i++)
            {
                string comboIndex = comboIndices[i];
                if (m_ComboMap.TryGetValue(comboIndex, out var comboData))
                {
                    // 直接添加，避免重复检查
                    enemyBuildParm.ComboData[comboIndex] = comboData;
                }
                else
                {
                    Debug.LogWarning($"未能找到连招索引 {comboIndex} 对应的ComboData数据，敌人索引: {enemyIndex}");
                }
            }
        }

    }
    /// <summary>
    /// 初始化敌人的状态信息
    /// </summary>
    private void InitEnemyFSM()
    {
        #region 构建cdf
        ////RedShit的信息构建
        ////----------------------->
        ////新建RedShit的状态表信息
        //Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> States = new Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>>();
        ////初始化Idle状态的转换信息
        //Dictionary<Transition, Enemy_State> Idletrans = new Dictionary<Transition, Enemy_State>();
        ////添加转换信息
        //Idletrans.Add(Transition.FindPlayer, Enemy_State.Move);
        //Idletrans.Add(Transition.Attack, Enemy_State.Attack);
        //Idletrans.Add(Transition.Hit, Enemy_State.Hit);
        //Idletrans.Add(Transition.Die, Enemy_State.Dead);
        ////添加状态
        //States.Add(Enemy_State.Idle, Idletrans);

        ////初始化Move状态的转换信息
        //Dictionary<Transition, Enemy_State> Movetrans = new Dictionary<Transition, Enemy_State>();
        ////初始化Move状态的转换信息
        //Movetrans.Add(Transition.LostTarget, Enemy_State.Idle);
        //Movetrans.Add(Transition.Attack, Enemy_State.Attack);
        //Movetrans.Add(Transition.Hit, Enemy_State.Hit);
        //Movetrans.Add(Transition.Die, Enemy_State.Dead);
        ////添加状态
        //States.Add(Enemy_State.Move, Movetrans);

        ////初始化Attack状态的转换信息
        //Dictionary<Transition, Enemy_State> Attacktrans = new Dictionary<Transition, Enemy_State>();
        ////初始化Attack状态的转换信息
        //Attacktrans.Add(Transition.LostTarget, Enemy_State.Idle);
        //Attacktrans.Add(Transition.FindPlayer, Enemy_State.Move);
        //Attacktrans.Add(Transition.Hit, Enemy_State.Hit);
        //Attacktrans.Add(Transition.Die, Enemy_State.Dead);
        ////添加状态
        //States.Add(Enemy_State.Attack, Attacktrans);

        ////初始化Hit状态
        //Dictionary<Transition, Enemy_State> Hittrans = new Dictionary<Transition, Enemy_State>();
        //Hittrans.Add(Transition.LostTarget, Enemy_State.Idle);
        //Hittrans.Add(Transition.FindPlayer, Enemy_State.Move);
        //Hittrans.Add(Transition.Attack, Enemy_State.Attack);
        //Hittrans.Add(Transition.Die, Enemy_State.Dead);
        //States.Add(Enemy_State.Hit, Hittrans);

        ////初始化Die状态
        //Dictionary<Transition, Enemy_State> Deadtrans = new Dictionary<Transition, Enemy_State>();
        //States.Add(Enemy_State.Dead, Deadtrans);
        ////<-----------------------
        ////More EnemyType------


        ////添加到config
        //EnemyFSMParm attr = new EnemyFSMParm(States);
        //m_enemyFSM.Add(0, attr);
        #endregion
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<int, EnemyFSMParm>> context = factory.CreateParser<Dictionary<int, EnemyFSMParm>>();
        m_enemyFSM = context.ParseJsonContext(JsonCfgName.CharacterFsmParamCfg);


    }
    //创建敌人对象
    public override GameObject CreateEnemy(int enemyIndex)
    {
        //读取模型
        ResourcesAssetFactory factory =  GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject prefab = factory.LoadModel(ResourcesPath.EnemyPath + m_enemyType[enemyIndex].Name);//加载方式
        GameObject obj =  GameObject.Instantiate(prefab);
        obj.SetActive(false);
        //设置状态机消息队列//用于传输消息例如受伤消息传递给
        MassageQueue massageQueue = new MassageQueue();
        //设置属性
        AttrFactory attrFactory = GameBaseFactory.GetAttrFactory() as AttrFactory;
        EnemyAttr enemyAttr = attrFactory.GetEnemyAttr(enemyIndex);
        obj.GetComponent<EnemyHealthyControl>().InitHealthy(enemyAttr, massageQueue);

        //初始化事件系统
        obj.GetComponent<EnemyHealthyControl>().InitAddEvent();

        //初始化敌人状态机
        EnemyCombatControl combatControl = obj.GetComponent<EnemyCombatControl>();
        combatControl.InitMachines(NewFsmStates(enemyIndex),massageQueue);
        //初始化连招
        //初始化敌人出招表
        //从m_enemyType中读取到所有的配置
        combatControl.InitComboSO(m_enemyType[enemyIndex].ComboData);

        //加入敌人管理器

        return obj;
    }

    //创建boss对象

    public override GameObject CreateBoss(int enemyIndex)
    {
        //读取模型
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject prefab = factory.LoadModel(ResourcesPath.EnemyPath + m_enemyType[enemyIndex].Name);//加载方式
        GameObject obj = GameObject.Instantiate(prefab);
        obj.SetActive(false);

        //初始化属性
        AttrFactory attrFactory = GameBaseFactory.GetAttrFactory() as AttrFactory;
        EnemyAttr enemyAttr = attrFactory.GetEnemyAttr(enemyIndex);
        obj.GetComponent<EnemyBossHealthyControl>().InitHealthy(enemyAttr);
        //初始化受伤事件系统
        obj.GetComponent<EnemyBossHealthyControl>().InitAddEvent();
        //初始化连招
        //初始化敌人出招表
        //从m_enemyType中读取到所有的配置
        EnemyBossComboControl combatControl = obj.GetComponent<EnemyBossComboControl>();
        combatControl.InitComboSO(m_enemyType[enemyIndex].ComboData);

        return obj;
    }

    /// <summary>
    /// 根据信息新建一个敌人的状态
    /// </summary>
    /// <param name="enemyIndex">敌人的代号</param>
    /// <returns></returns>
    private Dictionary<Enemy_State,FSMState> NewFsmStates(int enemyIndex)
    {
        //根据传入的状态机信息，进行对状态的初始化
        Dictionary<Enemy_State, FSMState> redShitStates = new Dictionary<Enemy_State, FSMState>();

        if (m_enemyFSM.TryGetValue(enemyIndex, out var fsmAttr))
        {
            Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> fsmStates = fsmAttr.GetList();
            //根据配置信息新建一个状态
            foreach(var key in fsmStates.Keys)
            {
                FSMState newstate;

                newstate =  NewState(key);
                if(newstate != null)
                {
                    //添加转换
                    foreach (var obj in fsmStates[key].Keys)
                    {
                        newstate.AddTransition(obj, fsmStates[key][obj]);
                    }
                    redShitStates.Add(key, newstate);
                }
            }
        }
        else
        {
            Debug.LogError($"No FSM configuration found for enemy with index {enemyIndex}");
        }
        return redShitStates;
    }
    private FSMState NewState(Enemy_State enemy_State)
    {
        FSMState newState = null;
        switch (enemy_State)
        {
            case Enemy_State.Idle:
                newState = new IdleState(null);
                break;
            case Enemy_State.Attack:
                newState = new AttackState(null);
                break;
            case Enemy_State.Move:
                newState = new MoveState(null);
                break;
            case Enemy_State.Victory:
                
                break;
            case Enemy_State.Hit:
                newState = new HitState(null);
                break;
            case Enemy_State.Dead:
                newState = new DeadState(null);
                break;
            default:
                break;
        }
        return newState;
    }
    public string GetEnemyName(int index)
    {
        return m_enemyType[index].Name;
    }

}
