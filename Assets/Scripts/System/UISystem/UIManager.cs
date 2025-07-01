using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UIConst
{
    //UI�������ڶ���,������ֵļ�¼
    public const string Text = "Text";
    public const string StartButton = "StartButton";
    public const string MenuPanel = "MenuPanel";
    public const string LoadingPanel = "LoadingPanel";
    public const string OptionsPanel = "OptionsPanel";
    public const string PlayerStatePanel = "PlayerStatePanel";
    public const string BossStatePanel = "BossStatePanel";
    public const string AbilityCardPanel = "AbilityCardPanel";
    public const string SkillInventoryPanel = "SkillInventoryPanel";
    public const string TriggerMassagePanel = "TriggerMassagePanel";
    public const string PlayingPanel = "PlayingPanel";
    public const string FloatingTextPanel = "FloatingTextPanel";
    public const string DeadPanel = "DeadPanel";
}
public class UIManager
{
    private static UIManager instance;

    private static readonly object instanceLock = new object();

    public static UIManager MainInstance
    {
        get
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UIManager();
                        return instance;
                    }
                }
            }
            return instance;
        }
    }


    //·������
    private Dictionary<string, string> pathDict;
    //ui�Ӳ㼶
    UILayer layer;
    private UIManager()
    {
        InitDict();
    }
    private void InitDict()
    {
        //��ʼ��·��
        pathDict = new Dictionary<string, string>() {
            { UIConst.Text,"UI/Panel/Image"},
            { UIConst.StartButton,"UI/Panel/StartButton"},
            { UIConst.MenuPanel,"UI/Panel/MenuPanel"},
            { UIConst.LoadingPanel,"UI/Panel/LoadingPanel"},
            { UIConst.OptionsPanel,"UI/Panel/OptionsPanel"},
            { UIConst.PlayerStatePanel,"UI/Panel/PlayerStatePanel"},
            { UIConst.BossStatePanel,"UI/Panel/BossStatePanel"},
            { UIConst.AbilityCardPanel,"UI/Panel/AbilityCardPanel"},
            { UIConst.SkillInventoryPanel,"UI/Panel/SkillInventoryPanel"},
            { UIConst.TriggerMassagePanel,"UI/Panel/TriggerMassagePanel"},
            { UIConst.PlayingPanel ,"UI/Panel/PlayingPanel"},
            { UIConst.FloatingTextPanel,"UI/Panel/FloatingTextPanel"},
            { UIConst.DeadPanel,"UI/Panel/DeadPanel"}
        };
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, PanelBase>();
        layer = new UILayer();
    }


    //��ȡÿ��panel��Ҫ�����Ļ���
    //������Ҫ������򵥵�ֻ��Ҫһ��

    //����UITYPE�ж��Root
    private Transform canvas;

    public Transform UIRoot
    {
        get
        {
            if (canvas == null)
            {
                if (GameObject.Find("Canvas"))
                {
                    canvas = GameObject.Find("Canvas").transform;
                }
                else
                {
                    //�½�һ��canvas
                    canvas = new GameObject("Canvas").transform;

                }
            }
            return canvas;
        }
    }
    //�򿪺͹ر�panel�ķ���
    //�����ֵ�
    //һ���洢���е�Ԥ����
    //һ���洢�������ڴ򿪵�UI����
    private Dictionary<string, GameObject> prefabDict;//���е�Ԥ����

    public Dictionary<string, PanelBase> panelDict;//�ѿ���

    public PanelBase OpenPanel(string name)
    {
        //�����
        PanelBase panel = null;

        //��ǰ��Ҫ�������Ƿ�����Ϣ����ȷ������Ѿ�������ô����Ҫ
        if (panelDict.TryGetValue(name, out panel))
        {
            //�Ѿ�������ֱ����������
            Debug.Log(name + "��Ҫ�򿪵�����Ѿ�����");
            return panel;
        }

        //�ټ��Ԥ�����ֵ�ǰ��Ҫ��·�������жϣ��鿴·���Ƿ�����
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            //û�����ö�Ӧpanel��·��
            Debug.Log(name + "·��û������");
        }
        //�ȼ�鵱ǰ��Ԥ�����ֵ����Ƿ��и����Ƶ�panel
        if (!prefabDict.TryGetValue(name, out GameObject prefab))
        {
            //���Ԥ�Ƽ�û����ͨ��Resources���л���
            prefab = Resources.Load(path) as GameObject;//ͨ��·����ȡԤ�Ƽ�
            prefabDict.Add(name, prefab);
        }
        //��������ļ����Ѿ�Ϊname��panel�������뵽prefabdict�͸���PanelBase
        //ֻ��Ҫ�������ȡ��Ȼ������
        GameObject panelObj = GameObject.Instantiate<GameObject>(prefab, UIRoot);
        panel = panelObj.GetComponent<PanelBase>();
        panelDict.Add(name, panel);
        //Debug.Log("�����"+ name);
        panel.OpenPanel(name);
        //���ÿ���ҳ��㼶
        layer.SetLayer(panelObj);

        return panel;

    }
    public bool ClosePanel(string name)
    {
        PanelBase panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            //ҳ��δ����ֱ�ӷ���
            Debug.Log("�ر�ҳ��" +  name + "ʧ��");
            return false; 
        }
        //�ߵ���˵��ҳ���Ѿ�����ֻ��Ҫ��ҳ��ر�
        //����panel��ֵ�Ѿ����Դ������
        panel.ClosePanel();
        return true;
    }
    public bool CloseAllPanel()
    {
        if (panelDict == null || panelDict.Count == 0) return false;

        string[] panelNames = panelDict.Keys.ToArray();
        foreach (string name in panelNames)
        {
            if (panelDict.TryGetValue(name, out PanelBase panel))
            {
                panel.ClosePanel();
            }
        }
        return true;
    }

    public PanelBase GetPanel(string name)
    {
        PanelBase panel1 = null;
        foreach(var key in panelDict.Keys)
        {
            if(name == key)
            {
                panel1 = panelDict[name];
                return panel1;
            }
        }
        Debug.LogWarning("�������Ϊ"+name+"����岢δ����");
        return panel1;
    }
}
