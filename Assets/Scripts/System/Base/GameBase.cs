using MyTools;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Ϸ��ʼʱ��ʼ����
/// </summary>
public class GameBase : SingletonNonMono<GameBase>
{
    //��Ϸϵͳ

    //��ɫϵͳ
    private PlayerDataSystem m_playerDataSystem = null;
    //����ϵͳ
    private PlayerSkillSystem m_skillSystem = null;
    //���˹���ϵͳ
    private EnemyManagerSystem m_EnemyManagerSystem = null;
    //�ؿ�ϵͳ
    private StageSystem m_StageSystem = null;
    //��������ϵͳ
    private CardBornSystem m_CardBornSystem = null;

    //ʱ��ϵͳ

    //�����
    private GameEnemyPoolSystem m_GamePoolSystem = null;
    //�̳�ϵͳ

    //��������Ʒ��ϵͳ
    private BagSystem m_BagSystem = null;

    //��ϷUI����Ϸ�����е�UI��

    //Ѫ��

    //���

    //����

    //PureMVC

    

    /// <summary>
    /// ��ʼ��������ϵͳ
    /// </summary>
    public void Init()
    {
        Debug.Log(Application.persistentDataPath);


        //��ʼ�������
        m_GamePoolSystem = new GameEnemyPoolSystem(this);

        m_CardBornSystem = new CardBornSystem(this);
        //��Ӷ��������
        //��ӵ��˶���

        //��ʼ�����˹���ϵͳ
        m_EnemyManagerSystem = new EnemyManagerSystem(this);

        //��ʼ���ؿ�ϵͳ
        m_StageSystem = new StageSystem(this);

        m_BagSystem = new BagSystem(this);

        //��ʼ����Ҽ�������ϵͳ
        m_skillSystem = new PlayerSkillSystem(this);



        //����UI


        //��ʼ���������ϵͳ
        m_playerDataSystem = new PlayerDataSystem(this);

    }
    public void EnterPlaying()
    {
        //��ʼ��PureMVC
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
        panel.FloatingText("��Ϸ����������");
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //������Ϸ������UI
        DeadPanel panel1 =  UIManager.MainInstance.OpenPanel(UIConst.DeadPanel) as DeadPanel;
        if (!string.IsNullOrEmpty(title)) {
            panel1.SetText(title);
        }


    }
    public void ReStart()
    {
        //���¿�ʼ
        //���¼���Battle����
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
    #region ���˹���ϵͳ��ط���
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

    #region �����ϵͳ

    public void InitOnePool(int prefabsID,int count)
    {
        //��ʼ��һ�������
        //ʹ�ù���ģʽ��������
        //Ȼ����ӳ���
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
    /// ֱ�����һ�����͵Ļ����
    /// ��Ϊ���˻����
    /// </summary>
    public void ClearEnemyPool()
    {
        m_GamePoolSystem.Clear();
    }
    /// <summary>
    /// ����һ��������ص����ж���
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseActivePool(string poolName)
    {
        //���������Ȼ��������еĳض���
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

    #region ��������

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="enemyName">��������</param>
    /// <param name="count">����</param>
    public void ActiveEnemyRandom(string enemyName, int count)
    {
        for(int i = 0; i < count; i++)
        {
            //���λ��
            Vector3 position = new Vector3(Random.Range(0, 50), 1, Random.Range(0, 50));
            //ָ��������ж���λ�ò��Ҽ���
            m_GamePoolSystem.TryGetPoolItem(enemyName, position,Quaternion.identity);
            Debug.Log("�������" + enemyName);
        }
    }
    public void ActiveEnemy(int enemyIndex, Vector3 pos)
    {
        m_GamePoolSystem.TryGetPoolItem(enemyIndex, pos, Quaternion.identity);
        //Debug.Log("����������" + enemyIndex);
    }
    /// <summary>
    /// ���������
    /// </summary>
    /// <param name="enemyName">���˵�����</param>
    /// <param name="position">λ��</param>
    /// <param name="rotation">��ת</param>
    public void ActiveOneEnemy(string enemyName, Vector3 position, Vector3 rotation)
    {
        m_GamePoolSystem.TryGetPoolItem("RedShit", position, Quaternion.identity);
    }
    #endregion

    #region �ؿ�ϵͳ
    public void CacheNowStage()
    {
        m_StageSystem.CacheNowStage();
    }
    public void ApplyStage(bool apply)
    {
        m_StageSystem.ApplyNowStage(apply);
    }
    #endregion

    #region �������ݴ洢ϵͳ
    //�Ѿ����ص��ڴ��е���Ҽ������ݴ洢
    public void GetPlayerSkillByID(int id)
    {
        m_skillSystem.GetSkillById(id);
    }

    #endregion

    #region ��ҵ�ǰ����ϵͳ

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
        Debug.Log("δ��ʼ���������ϵͳ");
        return null;
    }

    public void ChangePlayerAttr(PlayerAttribute type,float value)
    {
        m_playerDataSystem.ModifyAttrBuite(type, value);
    }

    #endregion

    #region ��������ϵͳ
    public void RnadomBornAttrText()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //�����
        UIManager.MainInstance.OpenPanel(UIConst.AbilityCardPanel);
        //���ɿ�Ƭ
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

    #region ����ϵͳ�������ɫ�ı���

    public SkillCardData GetPlayerEqiupedSkillCardData(SkillBar bar)
    {
        return m_BagSystem.SkillCards[bar];
    }
    #endregion
}
