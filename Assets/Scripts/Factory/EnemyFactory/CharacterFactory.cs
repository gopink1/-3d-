using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
/// <summary>
/// ���˹����Ĳ���
/// </summary>
public class EnemyBuildParm
{
    //���˵Ļ�������
    private int index;      //��Ӧ�ĵ������ʹ���
    private string name;    //��������
    Dictionary<string,CharacterComboSO> comboData;    //��������Ҫ�ļ���<����index����������>

    public Dictionary<string, CharacterComboSO> ComboData
    {
        get => comboData;
    }

    //���˵�״̬����ʼ������
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
        // ֻ��Ӽ��������ֵ
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
/// ��������
/// </summary>
public class ComboData
{
    string comboIndex;  //���еĴ���
    string comboName;   //���е�����
    string comboPath;   //���еĵ�ַ
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
/// ��ʼ������״̬���Ĳ���
/// ������һ�����͵��˴���״̬��
/// ��ʼ��״̬������Ҫ�Ĳ�����Ϣ
/// </summary>
public class EnemyFSMParm
{
    //״̬��Ϣ
    //Dictionary<״̬��Dictionary<״̬ת��������ת��ת���״̬>>
    private Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> statesMassage;
    //���캯��������Ϣ
    //һ���ȡjson�ļ�
    public EnemyFSMParm(Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> list)
    {
        this.statesMassage=list;
    }
    //��ȡ��Ϣ
    public Dictionary<Enemy_State, Dictionary<Transition,Enemy_State>> GetList()
    {
        return statesMassage;
    }
}

public class CharacterFactory : ICharacterFactory
{
    Dictionary<int, EnemyBuildParm> m_enemyType = new Dictionary<int, EnemyBuildParm>();

    Dictionary<int, EnemyFSMParm> m_enemyFSM = new Dictionary<int, EnemyFSMParm>();

    Dictionary<string,CharacterComboSO> m_ComboMap = new Dictionary<string, CharacterComboSO>();    //���еı���Ϊ��ʽ��id��ֵ��ȡ������ʽ����
    public CharacterFactory()
    {
        InitComboData();
        InitEnemyType();
        InitEnemyFSM();
    }

    private void InitComboData()
    {
        //��ʼ����������
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<string,ComboData>> jsonParse =  factory.CreateParser<Dictionary<string, ComboData>>();
        Dictionary<string, ComboData> m_enemyComboData = jsonParse.ParseJsonContext(JsonCfgName.EnemyComboCfg);//���ˣ����˶�Ӧ��

        //��ȡ��Ӧ�����ݺ���Ҫ��������Դ�ļ��н���loadȻ����д洢
        //���ع���
        ResourcesAssetFactory factory1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;

        foreach(var cb in m_enemyComboData)
        {
            //�ӵ�ַ�м���so��
            CharacterComboSO so = factory1.LoadSO(cb.Value.ComboPath) as CharacterComboSO;
            if (!m_ComboMap.ContainsKey(cb.Key))
            {
                m_ComboMap.Add(cb.Key, so);
            }
            else
            {
                Debug.LogWarning("�Ѿ����д���Ϊ"+cb.Key+"��ʽ����");
            }

        }

    }

    /// <summary>
    /// ��ʼ�����˵Ļ�����Ϣ
    /// </summary>
    private void InitEnemyType()
    {
        //���RedShit�Ĺ�����Ϣ
        //��ʼ��������Ϣ
        JsonParseFactory factory =  GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<int, EnemyBuildParm>> context =  factory.CreateParser<Dictionary<int, EnemyBuildParm>>();
        m_enemyType = context.ParseJsonContext(JsonCfgName.CharacterBuilderCfg);

        //��ӵ������������е�ComboData����
        foreach (var enemyEntry in m_enemyType)
        {
            int enemyIndex = enemyEntry.Key;
            EnemyBuildParm enemyBuildParm = enemyEntry.Value;
            var comboIndices = enemyBuildParm.ComboData.Keys.ToArray(); // ת��Ϊ����

            for (int i = 0; i < comboIndices.Length; i++)
            {
                string comboIndex = comboIndices[i];
                if (m_ComboMap.TryGetValue(comboIndex, out var comboData))
                {
                    // ֱ����ӣ������ظ����
                    enemyBuildParm.ComboData[comboIndex] = comboData;
                }
                else
                {
                    Debug.LogWarning($"δ���ҵ��������� {comboIndex} ��Ӧ��ComboData���ݣ���������: {enemyIndex}");
                }
            }
        }

    }
    /// <summary>
    /// ��ʼ�����˵�״̬��Ϣ
    /// </summary>
    private void InitEnemyFSM()
    {
        #region ����cdf
        ////RedShit����Ϣ����
        ////----------------------->
        ////�½�RedShit��״̬����Ϣ
        //Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> States = new Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>>();
        ////��ʼ��Idle״̬��ת����Ϣ
        //Dictionary<Transition, Enemy_State> Idletrans = new Dictionary<Transition, Enemy_State>();
        ////���ת����Ϣ
        //Idletrans.Add(Transition.FindPlayer, Enemy_State.Move);
        //Idletrans.Add(Transition.Attack, Enemy_State.Attack);
        //Idletrans.Add(Transition.Hit, Enemy_State.Hit);
        //Idletrans.Add(Transition.Die, Enemy_State.Dead);
        ////���״̬
        //States.Add(Enemy_State.Idle, Idletrans);

        ////��ʼ��Move״̬��ת����Ϣ
        //Dictionary<Transition, Enemy_State> Movetrans = new Dictionary<Transition, Enemy_State>();
        ////��ʼ��Move״̬��ת����Ϣ
        //Movetrans.Add(Transition.LostTarget, Enemy_State.Idle);
        //Movetrans.Add(Transition.Attack, Enemy_State.Attack);
        //Movetrans.Add(Transition.Hit, Enemy_State.Hit);
        //Movetrans.Add(Transition.Die, Enemy_State.Dead);
        ////���״̬
        //States.Add(Enemy_State.Move, Movetrans);

        ////��ʼ��Attack״̬��ת����Ϣ
        //Dictionary<Transition, Enemy_State> Attacktrans = new Dictionary<Transition, Enemy_State>();
        ////��ʼ��Attack״̬��ת����Ϣ
        //Attacktrans.Add(Transition.LostTarget, Enemy_State.Idle);
        //Attacktrans.Add(Transition.FindPlayer, Enemy_State.Move);
        //Attacktrans.Add(Transition.Hit, Enemy_State.Hit);
        //Attacktrans.Add(Transition.Die, Enemy_State.Dead);
        ////���״̬
        //States.Add(Enemy_State.Attack, Attacktrans);

        ////��ʼ��Hit״̬
        //Dictionary<Transition, Enemy_State> Hittrans = new Dictionary<Transition, Enemy_State>();
        //Hittrans.Add(Transition.LostTarget, Enemy_State.Idle);
        //Hittrans.Add(Transition.FindPlayer, Enemy_State.Move);
        //Hittrans.Add(Transition.Attack, Enemy_State.Attack);
        //Hittrans.Add(Transition.Die, Enemy_State.Dead);
        //States.Add(Enemy_State.Hit, Hittrans);

        ////��ʼ��Die״̬
        //Dictionary<Transition, Enemy_State> Deadtrans = new Dictionary<Transition, Enemy_State>();
        //States.Add(Enemy_State.Dead, Deadtrans);
        ////<-----------------------
        ////More EnemyType------


        ////��ӵ�config
        //EnemyFSMParm attr = new EnemyFSMParm(States);
        //m_enemyFSM.Add(0, attr);
        #endregion
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<int, EnemyFSMParm>> context = factory.CreateParser<Dictionary<int, EnemyFSMParm>>();
        m_enemyFSM = context.ParseJsonContext(JsonCfgName.CharacterFsmParamCfg);


    }
    //�������˶���
    public override GameObject CreateEnemy(int enemyIndex)
    {
        //��ȡģ��
        ResourcesAssetFactory factory =  GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject prefab = factory.LoadModel(ResourcesPath.EnemyPath + m_enemyType[enemyIndex].Name);//���ط�ʽ
        GameObject obj =  GameObject.Instantiate(prefab);
        obj.SetActive(false);
        //����״̬����Ϣ����//���ڴ�����Ϣ����������Ϣ���ݸ�
        MassageQueue massageQueue = new MassageQueue();
        //��������
        AttrFactory attrFactory = GameBaseFactory.GetAttrFactory() as AttrFactory;
        EnemyAttr enemyAttr = attrFactory.GetEnemyAttr(enemyIndex);
        obj.GetComponent<EnemyHealthyControl>().InitHealthy(enemyAttr, massageQueue);

        //��ʼ���¼�ϵͳ
        obj.GetComponent<EnemyHealthyControl>().InitAddEvent();

        //��ʼ������״̬��
        EnemyCombatControl combatControl = obj.GetComponent<EnemyCombatControl>();
        combatControl.InitMachines(NewFsmStates(enemyIndex),massageQueue);
        //��ʼ������
        //��ʼ�����˳��б�
        //��m_enemyType�ж�ȡ�����е�����
        combatControl.InitComboSO(m_enemyType[enemyIndex].ComboData);

        //������˹�����

        return obj;
    }

    //����boss����

    public override GameObject CreateBoss(int enemyIndex)
    {
        //��ȡģ��
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject prefab = factory.LoadModel(ResourcesPath.EnemyPath + m_enemyType[enemyIndex].Name);//���ط�ʽ
        GameObject obj = GameObject.Instantiate(prefab);
        obj.SetActive(false);

        //��ʼ������
        AttrFactory attrFactory = GameBaseFactory.GetAttrFactory() as AttrFactory;
        EnemyAttr enemyAttr = attrFactory.GetEnemyAttr(enemyIndex);
        obj.GetComponent<EnemyBossHealthyControl>().InitHealthy(enemyAttr);
        //��ʼ�������¼�ϵͳ
        obj.GetComponent<EnemyBossHealthyControl>().InitAddEvent();
        //��ʼ������
        //��ʼ�����˳��б�
        //��m_enemyType�ж�ȡ�����е�����
        EnemyBossComboControl combatControl = obj.GetComponent<EnemyBossComboControl>();
        combatControl.InitComboSO(m_enemyType[enemyIndex].ComboData);

        return obj;
    }

    /// <summary>
    /// ������Ϣ�½�һ�����˵�״̬
    /// </summary>
    /// <param name="enemyIndex">���˵Ĵ���</param>
    /// <returns></returns>
    private Dictionary<Enemy_State,FSMState> NewFsmStates(int enemyIndex)
    {
        //���ݴ����״̬����Ϣ�����ж�״̬�ĳ�ʼ��
        Dictionary<Enemy_State, FSMState> redShitStates = new Dictionary<Enemy_State, FSMState>();

        if (m_enemyFSM.TryGetValue(enemyIndex, out var fsmAttr))
        {
            Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>> fsmStates = fsmAttr.GetList();
            //����������Ϣ�½�һ��״̬
            foreach(var key in fsmStates.Keys)
            {
                FSMState newstate;

                newstate =  NewState(key);
                if(newstate != null)
                {
                    //���ת��
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
