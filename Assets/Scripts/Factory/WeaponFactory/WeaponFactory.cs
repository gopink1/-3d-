using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBuilderAttr
{
    private string weaponIndex; //��������
    private string modelPath;   //ģ��·��
    private string comboName;
    public string WeaponIndex
    {
        get => weaponIndex;
    }

    public string ModelPath
    {
        get => modelPath;
    }
    public string ComboName
    {
        get => comboName;
    }

    public WeaponBuilderAttr(string weaponIndex,  string modelPath,string comboname)
    {
        this.weaponIndex=weaponIndex;
        this.modelPath=modelPath;
        this.comboName=comboname;
    }
}
public class WeaponFactory : IWeaponFactory
{
    private Dictionary<string,WeaponBuilderAttr> weaponsConfig;


    public WeaponFactory()
    {
        InitConfig();
    }
    private void InitConfig()
    {

        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<string, WeaponBuilderAttr>> jsonParse = factory.CreateParser<Dictionary<string, WeaponBuilderAttr>>();
        weaponsConfig = jsonParse.ParseJsonContext(JsonCfgName.WeaponBuilderParamCfg);
        #region guoiqu
        //weaponsConfig = new Dictionary<string, WeaponBuilderAttr>();
        ////�½�������Ϣ
        //WeaponBuilderAttr weapon0 = new WeaponBuilderAttr("S0_0", "Weapon/Sword0_Green", "SwordCombo0");
        //WeaponBuilderAttr weapon1 = new WeaponBuilderAttr("S0_1", "Weapon/Sword0_Red", "SwordCombo0");
        //WeaponBuilderAttr weapon2 = new WeaponBuilderAttr("S0_2", "Weapon/Sword0_Yellow", "SwordCombo0");
        //weaponsConfig.Add(weapon0.WeaponIndex, weapon0);
        //weaponsConfig.Add(weapon1.WeaponIndex, weapon1);
        //weaponsConfig.Add(weapon2.WeaponIndex, weapon2);

        //WeaponBuilderAttr weapon3 = new WeaponBuilderAttr("BA0_0", "Weapon/Battleaxe0_Blue", "BattleaxeCombo0");
        //WeaponBuilderAttr weapon4 = new WeaponBuilderAttr("BA0_1", "Weapon/Battleaxe0_Cyan", "BattleaxeCombo0");
        //WeaponBuilderAttr weapon5 = new WeaponBuilderAttr("BA0_2", "Weapon/Battleaxe0_Green", "BattleaxeCombo0");
        //WeaponBuilderAttr weapon6 = new WeaponBuilderAttr("BA0_3", "Weapon/Battleaxe0_Orange", "BattleaxeCombo0");
        //WeaponBuilderAttr weapon7 = new WeaponBuilderAttr("BA0_4", "Weapon/Battleaxe0_Red", "BattleaxeCombo0");
        //weaponsConfig.Add(weapon3.WeaponIndex, weapon3);
        //weaponsConfig.Add(weapon4.WeaponIndex, weapon4);
        //weaponsConfig.Add(weapon5.WeaponIndex, weapon5);
        //weaponsConfig.Add(weapon6.WeaponIndex, weapon6);
        //weaponsConfig.Add(weapon7.WeaponIndex, weapon7);
        #endregion

    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="sword">����������</param>
    /// <returns></returns>
    public override IWeapon CreateSword(string weaponIndex)
    {
        //���������Ĳ���
        //��������
        IWeapon weapon = new SwordWeapon();
        string path = weaponsConfig[weaponIndex].ModelPath;

        //����ģ��
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject weaponModel = factory.LoadModel(path);
        weapon.SetModel(weaponModel);
        //��������
        AttrFactory factory1 = GameBaseFactory.GetAttrFactory() as AttrFactory;
        weapon.SetAttr(factory1.GetWeaponAttr(weaponsConfig[weaponIndex].WeaponIndex));
        //�������б�
        weapon.SetComboSO(factory.LoadSO("ComboSO/Player/SwordSO/" + weaponsConfig[weaponIndex].ComboName) as CharacterComboSO);
        //����Ч

        //����Ч

        return weapon;
    }

    /// <summary>
    /// ����ս����
    /// </summary>
    /// <param name="sword">�޸���������</param>
    /// <returns></returns>
    public override IWeapon CreateBattleaxe(string weaponIndex)
    {
        //���������Ĳ���
        //��������
        IWeapon weapon = new SwordWeapon();
        string path = weaponsConfig[weaponIndex].ModelPath;
        //����ģ��
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject weaponModel = factory.LoadModel(path);
        weapon.SetModel(weaponModel);
        //��������
        AttrFactory factory1 = GameBaseFactory.GetAttrFactory() as AttrFactory;
        weapon.SetAttr(factory1.GetWeaponAttr(weaponsConfig[weaponIndex].WeaponIndex));
        //�������б�
        weapon.SetComboSO(factory.LoadSO("ComboSO/Player/BattleaxeSO/" + weaponsConfig[weaponIndex].ComboName) as CharacterComboSO);
        //����Ч

        //����Ч

        return weapon;
    }

    /// <summary>
    /// ����ս����
    /// </summary>
    /// <param name="sword">�޸���������</param>
    /// <returns></returns>
    public override IWeapon CreateIceFire(string weaponIndex)
    {
        //���������Ĳ���
        //��������
        IWeapon weapon = new RangedWeapon();
        string path = weaponsConfig[weaponIndex].ModelPath;
        //����ģ��
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        GameObject weaponModel = factory.LoadModel(path);
        (weapon as RangedWeapon).SetVFX(weaponModel);
        //��������
        AttrFactory factory1 = GameBaseFactory.GetAttrFactory() as AttrFactory;
        weapon.SetAttr(factory1.GetWeaponAttr(weaponsConfig[weaponIndex].WeaponIndex));
        //�������б�
        weapon.SetComboSO(factory.LoadSO("ComboSO/Player/FireCombo/" + weaponsConfig[weaponIndex].ComboName) as CharacterComboSO);
        //����Ч

        //����Ч

        return weapon;
    }

}
