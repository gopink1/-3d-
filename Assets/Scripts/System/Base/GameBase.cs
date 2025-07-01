using MyTools;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 当游戏开始时候开始调用
/// </summary>
public class GameBase : SingletonNonMono<GameBase>
{
    //游戏系统

    //角色系统
    private PlayerDataSystem m_playerDataSystem = null;
    //技能系统
    private PlayerSkillSystem m_skillSystem = null;
    //敌人管理系统
    private EnemyManagerSystem m_EnemyManagerSystem = null;
    //关卡系统
    private StageSystem m_StageSystem = null;
    //卡牌增益系统
    private CardBornSystem m_CardBornSystem = null;

    //时间系统

    //对象池
    private GameEnemyPoolSystem m_GamePoolSystem = null;
    //商城系统

    //背包（饰品）系统
    private BagSystem m_BagSystem = null;

    //游戏UI（游戏进行中的UI）

    //血条

    //金币

    //蓝条

    //PureMVC

    

    /// <summary>
    /// 初始化所有子系统
    /// </summary>
    public void Init()
    {
        Debug.Log(Application.persistentDataPath);


        //初始化对象池
        m_GamePoolSystem = new GameEnemyPoolSystem(this);

        m_CardBornSystem = new CardBornSystem(this);
        //添加对象池内容
        //添加敌人对象

        //初始化敌人管理系统
        m_EnemyManagerSystem = new EnemyManagerSystem(this);

        //初始化关卡系统
        m_StageSystem = new StageSystem(this);

        m_BagSystem = new BagSystem(this);

        //初始化玩家技能数据系统
        m_skillSystem = new PlayerSkillSystem(this);



        //隐藏UI


        //初始化玩家数据系统
        m_playerDataSystem = new PlayerDataSystem(this);

    }
    public void EnterPlaying()
    {
        //初始化PureMVC
        UIManager.MainInstance.OpenPanel(UIConst.PlayerStatePanel);
        UIManager.MainInstance.OpenPanel(UIConst.BossStatePanel);
        UIManager.MainInstance.OpenPanel(UIConst.PlayingPanel);
        
        PMFacade.MainInstance.StartUp();
        PMFacade.MainInstance.SendNotification(PMConst.PMaxHpUpdateCommand);
        PMFacade.MainInstance.SendNotification(PMConst.PAttrUpdateCommand,PlayerAttribute.DEF);
        PMFacade.MainInstance.SendNotification(PMConst.PAttrUpdateCommand, PlayerAttribute.ATK);
        PMFacade.MainInstance.SendNotification(PMConst.PAttrUpdateCommand, PlayerAttribute.SPEED);
        CacheNowStage();
    }


    public void UpdateSystem()
    {
        m_EnemyManagerSystem.Update();
        m_GamePoolSystem.Update();
        m_StageSystem.Update();
    }

    public void GameOver(string title)
    {
        FloatingTextPanel panel = UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        panel.FloatingText("游戏结束！！！");
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //出现游戏结束的UI
        DeadPanel panel1 =  UIManager.MainInstance.OpenPanel(UIConst.DeadPanel) as DeadPanel;
        if (!string.IsNullOrEmpty(title)) {
            panel1.SetText(title);
        }


    }
    public void ReStart()
    {
        //重新开始
        //重新加载Battle场景
        Time.timeScale = 1.0f;
        GameEventManager.MainInstance.CallEvent<string>(EventHash.ChangeSceneState, "StartState");
    }
    public void Exit()
    {

        Application.Quit();
    }
    public void Release()
    {

        m_BagSystem.Release();
        m_CardBornSystem.Release();
        m_EnemyManagerSystem.Release();
        m_playerDataSystem.Release();
        m_skillSystem.Release();
        m_GamePoolSystem.Release();
        m_StageSystem.Release();

        m_BagSystem = null;
        m_CardBornSystem = null;
        m_EnemyManagerSystem = null;
        m_playerDataSystem = null;
        m_skillSystem = null;
        m_GamePoolSystem = null;
        m_StageSystem = null;
        PMFacade.MainInstance.Release();

        UIManager.MainInstance.CloseAllPanel();
    }
    #region 敌人管理系统相关方法
    public GameObject GetMainPlayer()
    {
        //Debug.Log(m_EnemyManagerSystem.MainPlayer);
        if(m_EnemyManagerSystem.MainPlayer != null) 
        {
            return m_EnemyManagerSystem.MainPlayer;
        }
        return null;
    }



    #endregion

    #region 缓存池系统

    public void InitOnePool(int prefabsID,int count)
    {
        //初始化一个对象池
        //使用工厂模式生产对象
        //然后添加池中
        CharacterFactory factory = GameBaseFactory.GetCharacterFactory() as CharacterFactory;
        List<GameObject> list = new List<GameObject>();
        for(int i = 0; i < count; i++)
        {
            GameObject obj = factory.CreateEnemy(prefabsID);
            list.Add(obj);
        }
        m_GamePoolSystem.AddOneEnemyPool(factory.GetEnemyName(prefabsID), factory.GetEnemyName(prefabsID), list);
    }

    /// <summary>
    /// 直接清除一种类型的缓存池
    /// 该为敌人缓存池
    /// </summary>
    public void ClearEnemyPool()
    {
        m_GamePoolSystem.Clear();
    }
    /// <summary>
    /// 回收一整个对象池的所有对象
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseActivePool(string poolName)
    {
        //传入池名字然后清空所有的池对象
        m_GamePoolSystem.ReleaseOneActivePool(poolName);
    }

    public void ReleaseOneItemToPool(GameObject obj)
    {
        m_GamePoolSystem.ReleaseActiveItem(obj);
    }

    public void ClearOnePool(string poolName)
    {
        m_GamePoolSystem.Clear(poolName);
    }

    #endregion

    #region 敌人生成

    /// <summary>
    /// 激活多个怪物
    /// </summary>
    /// <param name="enemyName">敌人名字</param>
    /// <param name="count">数量</param>
    public void ActiveEnemyRandom(string enemyName, int count)
    {
        for(int i = 0; i < count; i++)
        {
            //随机位置
            Vector3 position = new Vector3(Random.Range(0, 50), 1, Random.Range(0, 50));
            //指定对象池中对象位置并且激活
            m_GamePoolSystem.TryGetPoolItem(enemyName, position,Quaternion.identity);
            Debug.Log("激活对象" + enemyName);
        }
    }
    public void ActiveEnemy(int enemyIndex, Vector3 pos)
    {
        m_GamePoolSystem.TryGetPoolItem(enemyIndex, pos, Quaternion.identity);
        //Debug.Log("激活对象代号" + enemyIndex);
    }
    /// <summary>
    /// 激活单个怪物
    /// </summary>
    /// <param name="enemyName">敌人的名字</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转</param>
    public void ActiveOneEnemy(string enemyName, Vector3 position, Vector3 rotation)
    {
        m_GamePoolSystem.TryGetPoolItem("RedShit", position, Quaternion.identity);
    }
    #endregion

    #region 关卡系统
    public void CacheNowStage()
    {
        m_StageSystem.CacheNowStage();
    }
    public void ApplyStage(bool apply)
    {
        m_StageSystem.ApplyNowStage(apply);
    }
    #endregion

    #region 技能数据存储系统
    //已经加载到内存中的玩家技能数据存储
    public void GetPlayerSkillByID(int id)
    {
        m_skillSystem.GetSkillById(id);
    }

    #endregion

    #region 玩家当前数据系统

    public Dictionary<SkillBar, SkillData> GetPlayerEqiupedSkillData()
    {
        return m_playerDataSystem.GetEquipedSkill();
    }
    public PlayerDataSystem GetPlayerDataSys()
    {
        if(m_playerDataSystem != null)
        {
            return m_playerDataSystem;
        }
        Debug.Log("未初始化玩家数据系统");
        return null;
    }

    public void ChangePlayerAttr(PlayerAttribute type,float value)
    {
        m_playerDataSystem.ModifyAttrBuite(type, value);
    }

    #endregion

    #region 卡牌生成系统
    public void RnadomBornAttrText()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //打开面板
        UIManager.MainInstance.OpenPanel(UIConst.AbilityCardPanel);
        //生成卡片
        m_CardBornSystem.RandomBornAttrCard();
        m_CardBornSystem.RandomBornAttrCard();
        m_CardBornSystem.RandomBornAttrCard();
    }
    public void RandomBornSkillText()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UIManager.MainInstance.OpenPanel(UIConst.AbilityCardPanel);

        m_CardBornSystem.RandomBornSkillCard();
        m_CardBornSystem.RandomBornSkillCard();
        m_CardBornSystem.RandomBornSkillCard();
    }
    public void RandomBornItemText()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UIManager.MainInstance.OpenPanel(UIConst.AbilityCardPanel);

        m_CardBornSystem.RandomBornItemCard();
        m_CardBornSystem.RandomBornItemCard();
        m_CardBornSystem.RandomBornItemCard();
    }
    #endregion

    #region 背包系统，保存角色的背包

    public SkillCardData GetPlayerEqiupedSkillCardData(SkillBar bar)
    {
        return m_BagSystem.SkillCards[bar];
    }
    #endregion
}
