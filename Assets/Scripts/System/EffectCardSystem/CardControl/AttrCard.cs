using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttrCard : CardBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    AttrCardData data;

    private Transform Content;
    private Transform Select;
    private void Awake()
    {
        Content = transform.Find("Content");
        Select = transform.Find("Select");
    }
    /// <summary>
    /// 设置卡片的数据
    /// 属性卡片不需id索引相关信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="isPercentage"></param>
    public void SetCardData(PlayerAttribute type,float count,bool isPercentage)
    {
        //设定卡片数据
        data = new AttrCardData(0, "UI/Panel/InstancePanel/AttrCard", CardType.AttrCard, type,count,isPercentage);
        //设置卡片显示   
        if (isPercentage)
        {
            Content.GetComponent<TextMeshProUGUI>().text = data.AttributeType.ToString() + "+" + data.Count * 100f + "%";
        }
        else
        {
            Content.GetComponent<TextMeshProUGUI>().text = data.AttributeType.ToString() + "+" + data.Count;
        }

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
        //选择当前的卡牌效果
        //关闭页面，通知属性的选择
        GameEventManager.MainInstance.CallEvent<CardData>(EventHash.ADDAttrCard, data);
        UIManager.MainInstance.ClosePanel(UIConst.AbilityCardPanel);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
