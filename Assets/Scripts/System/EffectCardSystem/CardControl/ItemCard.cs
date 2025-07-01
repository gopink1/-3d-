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
    /// ���ÿ�Ƭ������
    /// ���Կ�Ƭ����id���������Ϣ
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="isPercentage"></param>
    public void SetCardData(ItemBase item)
    {
        ItemDataBase itemData = item.ItemData;
        //���ÿ�Ƭ��ʾ
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        Texture2D icon = factory.LoadSprite(itemData.IconPath);
        // ��Texture2D����Sprite
        Sprite iconSprite = Sprite.Create(
            icon,
            new Rect(0, 0, icon.width, icon.height),
            new Vector2(0.5f, 0.5f) // ���ĵ�
        );
        Icon.GetComponent<Image>().sprite = iconSprite;
        //�����ı�
        Name.GetComponent<TextMeshProUGUI>().text = itemData.Name;
        Content.GetComponent<TextMeshProUGUI>().text = itemData.Describe;

        //�趨��Ƭ����
        data = new ItemCardData(itemData.Id, itemData.IconPath,CardType.ItemCard,item);

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
        //���º�
        GameEventManager.MainInstance.CallEvent<ItemCardData>(EventHash.ADDItemCard, data);//�����¼�1.����ϵͳ�洢���ݸı�  2.��ɫ����ϵͳ����
        UIManager.MainInstance.ClosePanel(UIConst.AbilityCardPanel);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
