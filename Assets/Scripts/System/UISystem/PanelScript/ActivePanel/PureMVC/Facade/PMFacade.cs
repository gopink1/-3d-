using PureMVC.Patterns.Facade;


public static class PMConst
{
    //commandָ��
    public const string PHpUpdateCommand = "PHpUpdateCommand";    //�������hp
    public const string PMaxHpUpdateCommand = "PMaxHpUpdateCommand";    //�������hp
    public const string EHpUpdateCommand = "EHpUpdateCommand";    //���µ���hp
    public const string BossNameUpdateCommand = "BossNameUpdateCommand";//����boss
    public const string BossHpUpdateCommand = "BossHpUpdateCommand";//����boss��hp

    public const string PAttrUpdateCommand = "PAttrUpdateCommand";//��������������

    //Mediate��Ϣ
    public const string UpdatePHpToView = "UpdatePHpToView";              //������ҵ�hp�������
    public const string UpdatePHpTextToView = "UpdatePHpTextToView";      //������ҵ�hp�������
    public const string UpdateEHpToView = "UpdateEHpToView";
    public const string UpdateBossHpToView = "UpdateBossHpToView";
    public const string UpdateBossNameToView = "UpdateBossNameToView";
    public const string UpdatePAttrToText = "UpdatePAttrToText";

    //��������
    public const string PlayerStateP = "PlayerStateP";      //������ҵ�hp�������
    public const string EnemyHpP = "EnemyHpP";              //���µ��˵�hp�������
    public const string BossHpP = "BossHpP";              //����boss��hp�������
    public const string PlayerAttrP = "PlayerAttrP";              //����boss��hp�������

    //�н�������
    public const string PlayerStateM = "PlayerStateM";      //������ҵ�hp�������
    public const string EnemyHpM = "EnemyHpM";              //���µ��˵�hp�������
    public const string BossHpM = "BossHpM";                //����boos��hp�������
    public const string PlayerAttrM = "PlayerAttrM";                //����boos��hp�������

} 
public class PMFacade : Facade
{
    // ˽�й��캯������ֹ�ⲿʵ����
    private PMFacade()
    {
        //Initialize();
    }

    private void Initialize()
    {
        // ע�����ݴ���
        RegisterProxy(new PlayerStateP(GameBase.MainInstance.GetMainPlayer().GetComponent<PlayerHealthyControl>()));
        RegisterProxy(new EnemyHpP());
        RegisterProxy(new BossHpP());
        
        //��Ϸ�е�UI���ݴ���


        // ע������
        //�����ջ���command
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

        //������Ϸ�е�UI

        // ע���н���
        RegisterMediator(new PlayerStateM(UIManager.MainInstance.GetPanel(UIConst.PlayerStatePanel)));
        RegisterMediator(new EnemyHpM());
        RegisterMediator(new BossHpM(UIManager.MainInstance.GetPanel(UIConst.BossStatePanel)));
        //RegisterMediator(new PlayerAttrM(UIManager.MainInstance.GetPanel(UIConst.PlayingPanel)));
        //ע����Ϸ�е����


    }



    public void StartUp()
    {
        //��ʼ�߼�
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
        // ע�����ݴ���
        RemoveProxy(PMConst.PlayerStateP);
        RemoveProxy(PMConst.EnemyHpP);
        RemoveProxy(PMConst.BossHpP);
        
        //��Ϸ�е�UI���ݴ���


        // ע������
        //�����ջ���command

        RemoveCommand(PMConst.PHpUpdateCommand); 
        RemoveCommand(PMConst.EHpUpdateCommand); 
        RemoveCommand(PMConst.BossHpUpdateCommand); 
        RemoveCommand(PMConst.PMaxHpUpdateCommand);
        RemoveCommand(PMConst.BossNameUpdateCommand);
        //RemoveCommand(PMConst.PAttrUpdateCommand);
        //������Ϸ�е�UI

        RemoveMediator(PMConst.PlayerStateM);
        RemoveMediator(PMConst.EnemyHpM);
        RemoveMediator(PMConst.BossHpM);
        //RemoveMediator(PMConst.PlayerAttrM);
        //ע����Ϸ�е����
        // ����ڲ�����

        // ���õ�������ѡ�����ݿ����ƣ�
        instance = null;
    }

}
