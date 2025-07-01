using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    AttrUp,
    AtkTrigger,
    SkillUp,
    StateSet,
    Resources,
    ZhaoHuan
}
public class ItemDataBase
{
    protected int id;             //��Ʒid
    public int Id
    {
        get { return id; }
    }
    protected string name;        //��Ʒ������
    public string Name
    {
        get { return name; }
    }
    protected string iconPath;    //��Ʒ��ͼ��·��
    public string IconPath
    {
        get { return iconPath; }
    }
    protected ItemType type;      //��Ʒ������
    public ItemType Type
    {
        get { return type; }
    }
    protected string describe;    //��Ʒ����
    public string Describe
    {
        get { return describe; }
    }
    //protected EffectEventArgs effect;
    //public EffectEventArgs Effect
    //{
    //    get => effect;
    //}
    public ItemDataBase(int id, string name, string iconPath, ItemType type, string describe/*, EffectEventArgs effect*/)
    {
        this.id=id;
        this.name=name;
        this.iconPath=iconPath;
        this.type=type;
        this.describe=describe;
        //this.effect=effect;
    }
}
public class AttrItemData : ItemDataBase
{
    //��������

    //���Ե�����
    private PlayerAttribute attrType;
    public PlayerAttribute AttrType
    {
        get { return attrType; }
    }
    private float value;
    public float Value
    {
        get { return value; }
    }
    private bool isPersentage;
    public bool IsPersentage
    {
        get { return isPersentage; }
    }

    public AttrItemData(int id, string name, string iconPath, ItemType type, string describe,PlayerAttribute attrType, float value, bool isPersentage) : 
        base(id, name, iconPath, type, describe/*, effect*/)
    {
        this.attrType=attrType;
        this.value=value;
        this.isPersentage=isPersentage;
    }
}
public class AttrItemDataD : ItemDataBase
{
    //��������

    //���Ե�����
    private PlayerAttribute[] attrType;
    public PlayerAttribute[] AttrType
    {
        get { return attrType; }
    }
    private float[] value;
    public float[] Value
    {
        get { return value; }
    }
    private bool[] isPersentage;
    public bool[] IsPersentage
    {
        get { return isPersentage; }
    }

    public AttrItemDataD(int id, string name, string iconPath, ItemType type, string describe, PlayerAttribute[] attrType, float[] value, bool[] isPersentage) :
        base(id, name, iconPath, type, describe/*, effect*/)
    {
        this.attrType=attrType;
        this.value=value;
        this.isPersentage=isPersentage;
    }
}
/// <summary>
/// �ı�ĳһ����ֵ
/// </summary>
public class OneValueChangeItemData : ItemDataBase
{
    //������˺�����Ʒ
    private float value;
    public float Value
    {
        get { return value; }
    }
    public OneValueChangeItemData(int id, string name, string iconPath, ItemType type, string describe,float value) : base(id, name, iconPath, type, describe)
    {
        this.value=value;
    }
}

public class SkillCdUpItemData : ItemDataBase
{
    //��������
    private TriggerType triggerType;
    private float value;

    public SkillCdUpItemData(int id, string name, string iconPath, ItemType type, string describe, TriggerType triggerType,float value) : base(id, name, iconPath, type, describe)
    {
        this.triggerType=triggerType;
        this.value=value;
    }
}

