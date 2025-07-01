using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

public class GMcmd
{

    static SoundEmitterPool pool = new SoundEmitterPool();
    [MenuItem("Text/WeaponText/ChangeToGreen")]
    public static void ChangeWeapon()
    {
        GameObject obj =  GameObject.Find("Player");
        obj.transform.GetComponent<PlayerCombatControl>().SetMainWeapon(GameBaseFactory.GetWeaponFactory().CreateSword("S0_1"));
    }
    [MenuItem("Text/WeaponText/ChangeToBattleaxe")]
    public static void ChangeWeaponaxe()
    {
        GameObject obj = GameObject.Find("Player");
        obj.transform.GetComponent<PlayerCombatControl>().SetMainWeapon(GameBaseFactory.GetWeaponFactory().CreateBattleaxe("BA0_1"));
    }
    [MenuItem("Text/EnemyText/NewOneEnemy")]
    public static void NewEnemy()
    {
        GameBase.MainInstance.InitOnePool(0,10);

    }
    [MenuItem("Text/EnemyText/ActiveOneEnemy")]
    public static void ActiveEnemy()
    {
        Debug.Log("�������Gm");
        //�������
        GameBase.MainInstance.ActiveEnemyRandom("RedShit", 5);
    }
    [MenuItem("Text/EnemyText/CreatSwordBoss")]
    public static void ActiveSwordBoss()
    {
        Debug.Log("�������Gm");
        //�������
        CharacterFactory factory = GameBaseFactory.GetCharacterFactory() as CharacterFactory;
        GameObject enemy =  factory.CreateBoss(1);
        enemy.gameObject.transform.position = Vector3.zero;
        enemy.SetActive(true);
    }
    [MenuItem("Text/StageSysText/CacheStage")]
    public static void CacheStage()
    {
        //�������
        GameBase.MainInstance.CacheNowStage();
    }
    [MenuItem("Text/StageSysText/ApplyStage")]
    public static void ApplyStage()
    {
        //�������
        GameBase.MainInstance.ApplyStage(true);
    }
    [MenuItem("Text/JsonText/CharacterBuilderParmtext")]
    public static void TextJsonParaseFactory()
    {
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();

        //���Ե�������
        JsonParseContext<Dictionary<int, EnemyBuildParm>> jsonParseContext =  factory.CreateParser<Dictionary<int, EnemyBuildParm>>();
        Dictionary<int, EnemyBuildParm> dic = jsonParseContext.ParseJsonContext(JsonCfgName.CharacterBuilderCfg);

        //���Ե���״̬��
        JsonParseContext<Dictionary<int, EnemyFSMParm>> jsonParseContext2 = factory.CreateParser<Dictionary<int, EnemyFSMParm>>();
        Dictionary<int, EnemyFSMParm> dic2 = jsonParseContext2.ParseJsonContext(JsonCfgName.CharacterFsmParamCfg);
        for (int i = 0; i < dic.Count; i++)
        {
            Debug.Log(dic2.Count);
        }
    }

    [MenuItem("Text/AudioSystem/TextPoolInit")]
    public static void TextAudioPoolInit()
    {
        pool.PrewarmPool(10);
    }
    [MenuItem("Text/AudioSystem/TextPoolCreate")]
    public static void TextAudioPoolCreate()
    {
        pool.Request();
    }

    [MenuItem("Text/PureMVC/TextBossHpBar")]
    public static void TextBossHpBar()
    {
        //����boss��Ѫ���Ƿ��ܳɹ�����
        GameObject boss =  GameObject.FindWithTag("Boss");
        if (boss == null)
        {
            Debug.LogError("����bossΪ����Ҫ�ȴ���boss");
            return;
        }
        PMFacade.MainInstance.SendNotification(PMConst.BossNameUpdateCommand, boss.GetComponent<EnemyBossHealthyControl>());
    }

    [MenuItem("Text/Player/AttrText")]
    public static void TextPlayerAttr()
    {
        //����boss��Ѫ���Ƿ��ܳɹ�����
        PlayerAttr attr = GameBase.MainInstance.GetMainPlayer().GetComponent<PlayerHealthyControl>().GetAttr();
        Debug.Log(attr.Speed);
    }

    [MenuItem("Text/CardSystem/AttrCardBorn")]
    public static void TextAttrCardBorn()
    {
        //����boss��Ѫ���Ƿ��ܳɹ�����
        GameBase.MainInstance.RnadomBornAttrText();
    }
    [MenuItem("Text/CardSystem/SkillCardBorn")]
    public static void TextSkillCardBorn()
    {
        //����boss��Ѫ���Ƿ��ܳɹ�����
        GameBase.MainInstance.RandomBornSkillText();
    }
    
    [MenuItem("Text/CardSystem/ItemCardBorn")]
    public static void TextItemCardBorn()
    {
        //����boss��Ѫ���Ƿ��ܳɹ�����
        GameBase.MainInstance.RandomBornItemText();
    }

    [MenuItem("Text/UIText/TextFloatingText")]
    public static void TextFloatingText()
    {
        //����boss��Ѫ���Ƿ��ܳɹ�����
        FloatingTextPanel panel =  UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        panel.FloatingText("��ս�ɹ�������");
    }
    [MenuItem("Text/End")]
    public static void TextEnd()
    {
        GameBase.MainInstance.ReStart();
    }
    [MenuItem("Text/TimeLine")]
    public static void TimeLine()
    {
        Debug.Log(Time.timeScale);
    }
}
