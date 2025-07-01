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
        Debug.Log("激活敌人Gm");
        //激活敌人
        GameBase.MainInstance.ActiveEnemyRandom("RedShit", 5);
    }
    [MenuItem("Text/EnemyText/CreatSwordBoss")]
    public static void ActiveSwordBoss()
    {
        Debug.Log("激活敌人Gm");
        //激活敌人
        CharacterFactory factory = GameBaseFactory.GetCharacterFactory() as CharacterFactory;
        GameObject enemy =  factory.CreateBoss(1);
        enemy.gameObject.transform.position = Vector3.zero;
        enemy.SetActive(true);
    }
    [MenuItem("Text/StageSysText/CacheStage")]
    public static void CacheStage()
    {
        //激活敌人
        GameBase.MainInstance.CacheNowStage();
    }
    [MenuItem("Text/StageSysText/ApplyStage")]
    public static void ApplyStage()
    {
        //激活敌人
        GameBase.MainInstance.ApplyStage(true);
    }
    [MenuItem("Text/JsonText/CharacterBuilderParmtext")]
    public static void TextJsonParaseFactory()
    {
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();

        //测试敌人属性
        JsonParseContext<Dictionary<int, EnemyBuildParm>> jsonParseContext =  factory.CreateParser<Dictionary<int, EnemyBuildParm>>();
        Dictionary<int, EnemyBuildParm> dic = jsonParseContext.ParseJsonContext(JsonCfgName.CharacterBuilderCfg);

        //测试敌人状态机
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
        //测试boss的血条是否能成功设置
        GameObject boss =  GameObject.FindWithTag("Boss");
        if (boss == null)
        {
            Debug.LogError("敌人boss为空需要先创建boss");
            return;
        }
        PMFacade.MainInstance.SendNotification(PMConst.BossNameUpdateCommand, boss.GetComponent<EnemyBossHealthyControl>());
    }

    [MenuItem("Text/Player/AttrText")]
    public static void TextPlayerAttr()
    {
        //测试boss的血条是否能成功设置
        PlayerAttr attr = GameBase.MainInstance.GetMainPlayer().GetComponent<PlayerHealthyControl>().GetAttr();
        Debug.Log(attr.Speed);
    }

    [MenuItem("Text/CardSystem/AttrCardBorn")]
    public static void TextAttrCardBorn()
    {
        //测试boss的血条是否能成功设置
        GameBase.MainInstance.RnadomBornAttrText();
    }
    [MenuItem("Text/CardSystem/SkillCardBorn")]
    public static void TextSkillCardBorn()
    {
        //测试boss的血条是否能成功设置
        GameBase.MainInstance.RandomBornSkillText();
    }
    
    [MenuItem("Text/CardSystem/ItemCardBorn")]
    public static void TextItemCardBorn()
    {
        //测试boss的血条是否能成功设置
        GameBase.MainInstance.RandomBornItemText();
    }

    [MenuItem("Text/UIText/TextFloatingText")]
    public static void TextFloatingText()
    {
        //测试boss的血条是否能成功设置
        FloatingTextPanel panel =  UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        panel.FloatingText("挑战成功！！！");
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
