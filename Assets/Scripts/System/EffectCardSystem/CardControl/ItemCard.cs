using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCard : CardBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    ItemCardData data;

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

        //GameEventManager.MainInstance.AddEventListening<SkillBar, SkillCardData>(EventHash.EquipSkill, OnSkillCardEquiped);
    }
    /// <summary>
    /// 设置卡片的数据
    /// 属性卡片不需id索引相关信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="isPercentage"></param>
    public void SetCardData(ItemBase item)
    {
        ItemDataBase itemData = item.ItemData;
        //设置卡片显示
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        Texture2D icon = factory.LoadSprite(itemData.IconPath);
        // 从Texture2D创建Sprite
        Sprite iconSprite = Sprite.Create(
            icon,
            new Rect(0, 0, icon.width, icon.height),
            new Vector2(0.5f, 0.5f) // 中心点
        );
        Icon.GetComponent<Image>().sprite = iconSprite;
        //设置文本
        Name.GetComponent<TextMeshProUGUI>().text = itemData.Name;
        Content.GetComponent<TextMeshProUGUI>().text = itemData.Describe;

        //设定卡片数据
        data = new ItemCardData(itemData.Id, itemData.IconPath,CardType.ItemCard,item);

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
        //按下后
        GameEventManager.MainInstance.CallEvent<ItemCardData>(EventHash.ADDItemCard, data);//触发事件1.背包系统存储数据改变  2.角色数据系统更新
        UIManager.MainInstance.ClosePanel(UIConst.AbilityCardPanel);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
