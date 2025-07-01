using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInventoryPanel : PanelBase
{
    private SkillCardData data;
    public SkillCardData Data
    {
        get { return data; }
    }
    SkillEquipedSelectPanel Q;
    SkillEquipedSelectPanel E;
    Transform Back;
    private void Awake()
    {
        //初始化选项
        Q = transform.Find("Q").GetComponent<SkillEquipedSelectPanel>();
        E = transform.Find("E").GetComponent<SkillEquipedSelectPanel>();
        Back = transform.Find("Back");
        Back.GetComponent<Button>().onClick.AddListener(BackDown);
        //从玩家哪里获取到选择装备的技能
        Sprite qIcon = null;
        Sprite eIcon = null;
        if (GameBase.MainInstance.GetPlayerEqiupedSkillCardData(SkillBar.Q) == null)
        {
            //设置qicon为默认

        }
        else
        {
            qIcon = GameBase.MainInstance.GetPlayerEqiupedSkillCardData(SkillBar.Q).Icon;
        }
        if (GameBase.MainInstance.GetPlayerEqiupedSkillCardData(SkillBar.E) == null)
        {

        }
        else
        {
            eIcon = GameBase.MainInstance.GetPlayerEqiupedSkillCardData(SkillBar.E).Icon;
        }


        //设置图标
        Q.SetIcon(qIcon);
        E.SetIcon(eIcon);
    }

    //传入选择的技能卡片的内容
    public void IntroducedCard(SkillCardData card)
    {
        //传入的data，当之后选择哪个技能后会进行替换
        data = card;
    }
    public void SelectDown()
    {
        UIManager.MainInstance.ClosePanel(UIConst.SkillInventoryPanel);
    }
    public void BackDown()
    {
        //按下返回按钮
        //取消替换直接关闭页面
        UIManager.MainInstance.ClosePanel(UIConst.SkillInventoryPanel);
    }
}
