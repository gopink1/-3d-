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
    /// ���ÿ�Ƭ������
    /// ���Կ�Ƭ����id���������Ϣ
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="isPercentage"></param>
    public void SetCardData(PlayerAttribute type,float count,bool isPercentage)
    {
        //�趨��Ƭ����
        data = new AttrCardData(0, "UI/Panel/InstancePanel/AttrCard", CardType.AttrCard, type,count,isPercentage);
        //���ÿ�Ƭ��ʾ   
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
        //�����뿨��
        Select.gameObject.SetActive(true);

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //����뿪����
        Select.gameObject.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //�������
        //ѡ��ǰ�Ŀ���Ч��
        //�ر�ҳ�棬֪ͨ���Ե�ѡ��
        GameEventManager.MainInstance.CallEvent<CardData>(EventHash.ADDAttrCard, data);
        UIManager.MainInstance.ClosePanel(UIConst.AbilityCardPanel);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
