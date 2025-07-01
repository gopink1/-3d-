using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillCard : CardBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    SkillCardData data;

    private Transform Icon;
    private Transform Name;
    private Transform Content;
    private Transform Select;
    private void Awake()
    {
        Icon = transform.Find("Icon");
        Name = transform.Find("Name");
        Content = transform.Find("Content");
        Select = transform.Find("Select");

        GameEventManager.MainInstance.AddEventListening<SkillBar, SkillCardData>(EventHash.EquipSkill, OnSkillCardEquiped);
    }
    /// <summary>
    /// 设置卡片的数据
    /// 属性卡片不需id索引相关信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="isPercentage"></param>
    public void SetCardData(SkillData skillData)
    {
        //设置卡片显示
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        Texture2D icon = factory.LoadSprite(skillData.IconUIPath);
        // 从Texture2D创建Sprite
        Sprite iconSprite = Sprite.Create(
            icon,
            new Rect(0, 0, icon.width, icon.height),
            new Vector2(0.5f, 0.5f) // 中心点
        );
        Icon.GetComponent<Image>().sprite = iconSprite;
        //设置文本
        Name.GetComponent<TextMeshProUGUI>().text = skillData.Name;
        Content.GetComponent<TextMeshProUGUI>().text = skillData.SkillDescribe;
        //设定卡片数据
        data = new SkillCardData(skillData.Id, skillData.IconUIPath, CardType.SkillCard, skillData, iconSprite);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //鼠标进入卡牌
        Select.gameObject.SetActive(true);

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //鼠标离开卡牌
        Select.gameObject.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //点击卡牌
        //打开装备在哪里 的选项面板
        SkillInventoryPanel panel =  UIManager.MainInstance.OpenPanel(UIConst.SkillInventoryPanel) as SkillInventoryPanel;
        //传入选择的技能卡片
        //然后监听选择技能替换事件 当替换发生就关闭页面
        panel.IntroducedCard(data);
    }
    private void OnSkillCardEquiped(SkillBar bar,SkillCardData data)
    {
        //判断是不是这个卡牌，如果是就删除卡牌
        if(data.ID == this.data.ID)
        {
            //关闭页面，通知属性的选择
            UIManager.MainInstance.ClosePanel(UIConst.AbilityCardPanel);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
