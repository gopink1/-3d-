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
        //��ʼ��ѡ��
        Q = transform.Find("Q").GetComponent<SkillEquipedSelectPanel>();
        E = transform.Find("E").GetComponent<SkillEquipedSelectPanel>();
        Back = transform.Find("Back");
        Back.GetComponent<Button>().onClick.AddListener(BackDown);
        //����������ȡ��ѡ��װ���ļ���
        Sprite qIcon = null;
        Sprite eIcon = null;
        if (GameBase.MainInstance.GetPlayerEqiupedSkillCardData(SkillBar.Q) == null)
        {
            //����qiconΪĬ��

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


        //����ͼ��
        Q.SetIcon(qIcon);
        E.SetIcon(eIcon);
    }

    //����ѡ��ļ��ܿ�Ƭ������
    public void IntroducedCard(SkillCardData card)
    {
        //�����data����֮��ѡ���ĸ����ܺ������滻
        data = card;
    }
    public void SelectDown()
    {
        UIManager.MainInstance.ClosePanel(UIConst.SkillInventoryPanel);
    }
    public void BackDown()
    {
        //���·��ذ�ť
        //ȡ���滻ֱ�ӹر�ҳ��
        UIManager.MainInstance.ClosePanel(UIConst.SkillInventoryPanel);
    }
}
