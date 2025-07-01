using PureMVC.Patterns.Facade;


public static class PMConst
{
    //command指令
    public const string PHpUpdateCommand = "PHpUpdateCommand";    //更新玩家hp
    public const string PMaxHpUpdateCommand = "PMaxHpUpdateCommand";    //更新玩家hp
    public const string EHpUpdateCommand = "EHpUpdateCommand";    //更新敌人hp
    public const string BossNameUpdateCommand = "BossNameUpdateCommand";//更新boss
    public const string BossHpUpdateCommand = "BossHpUpdateCommand";//更新boss的hp

    public const string PAttrUpdateCommand = "PAttrUpdateCommand";//更新左侧玩家属性

    //Mediate消息
    public const string UpdatePHpToView = "UpdatePHpToView";              //更新玩家的hp到面板上
    public const string UpdatePHpTextToView = "UpdatePHpTextToView";      //更新玩家的hp到面板上
    public const string UpdateEHpToView = "UpdateEHpToView";
    public const string UpdateBossHpToView = "UpdateBossHpToView";
    public const string UpdateBossNameToView = "UpdateBossNameToView";
    public const string UpdatePAttrToText = "UpdatePAttrToText";

    //代理名字
    public const string PlayerStateP = "PlayerStateP";      //更新玩家的hp到面板上
    public const string EnemyHpP = "EnemyHpP";              //更新敌人的hp到面板上
    public const string BossHpP = "BossHpP";              //更新boss的hp到面板上
    public const string PlayerAttrP = "PlayerAttrP";              //更新boss的hp到面板上

    //中介者名字
    public const string PlayerStateM = "PlayerStateM";      //更新玩家的hp到面板上
    public const string EnemyHpM = "EnemyHpM";              //更新敌人的hp到面板上
    public const string BossHpM = "BossHpM";                //更新boos的hp到面板上
    public const string PlayerAttrM = "PlayerAttrM";                //更新boos的hp到面板上

} 
public class PMFacade : Facade
{
    // 私有构造函数，防止外部实例化
    private PMFacade()
    {
        //Initialize();
    }

    private void Initialize()
    {
        // 注册数据代理
        RegisterProxy(new PlayerStateP(GameBase.MainInstance.GetMainPlayer().GetComponent<PlayerHealthyControl>()));
        RegisterProxy(new EnemyHpP());
        RegisterProxy(new BossHpP());
        
        //游戏中的UI数据代理


        // 注册命令
        //人物收击的command
        RegisterCommand(PMConst.PHpUpdateCommand, () =>
        {
            return new PHpUpdateCommand();
        });
        RegisterCommand(PMConst.PMaxHpUpdateCommand, () =>
        {
            return new PMaxHpUpdateCommand();
        });
        RegisterCommand(PMConst.EHpUpdateCommand, () =>
        {
            return new EHpUpdateCommand();
        });
        RegisterCommand(PMConst.BossNameUpdateCommand, () =>
        {
            return new BossNameUpdateCommand();
        });
        RegisterCommand(PMConst.BossHpUpdateCommand, () =>
        {
            return new BossHpUpdateCommand();
        });
        //RegisterCommand(PMConst.PAttrUpdateCommand, () =>
        //{
        //    return new PAttrUpdateCommand();
        //});

        //更新游戏中的UI

        // 注册中介器
        RegisterMediator(new PlayerStateM(UIManager.MainInstance.GetPanel(UIConst.PlayerStatePanel)));
        RegisterMediator(new EnemyHpM());
        RegisterMediator(new BossHpM(UIManager.MainInstance.GetPanel(UIConst.BossStatePanel)));
        //RegisterMediator(new PlayerAttrM(UIManager.MainInstance.GetPanel(UIConst.PlayingPanel)));
        //注册游戏中的面板


    }



    public void StartUp()
    {
        //开始逻辑
        Initialize();
    }


    public static PMFacade MainInstance
    {
        get
        {
            if (instance==null)
            {
                instance = new PMFacade();
            }
            return instance as PMFacade;
        }
    }
    
    public void Release()
    {
        // 注册数据代理
        RemoveProxy(PMConst.PlayerStateP);
        RemoveProxy(PMConst.EnemyHpP);
        RemoveProxy(PMConst.BossHpP);
        
        //游戏中的UI数据代理


        // 注册命令
        //人物收击的command

        RemoveCommand(PMConst.PHpUpdateCommand); 
        RemoveCommand(PMConst.EHpUpdateCommand); 
        RemoveCommand(PMConst.BossHpUpdateCommand); 
        RemoveCommand(PMConst.PMaxHpUpdateCommand);
        RemoveCommand(PMConst.BossNameUpdateCommand);
        //RemoveCommand(PMConst.PAttrUpdateCommand);
        //更新游戏中的UI

        RemoveMediator(PMConst.PlayerStateM);
        RemoveMediator(PMConst.EnemyHpM);
        RemoveMediator(PMConst.BossHpM);
        //RemoveMediator(PMConst.PlayerAttrM);
        //注册游戏中的面板
        // 清空内部缓存

        // 重置单例（可选，根据框架设计）
        instance = null;
    }

}
