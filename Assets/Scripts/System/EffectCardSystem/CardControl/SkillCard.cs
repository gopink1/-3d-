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
    /// ���ÿ�Ƭ������
    /// ���Կ�Ƭ����id���������Ϣ
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="isPercentage"></param>
    public void SetCardData(SkillData skillData)
    {
        //���ÿ�Ƭ��ʾ
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        Texture2D icon = factory.LoadSprite(skillData.IconUIPath);
        // ��Texture2D����Sprite
        Sprite iconSprite = Sprite.Create(
            icon,
            new Rect(0, 0, icon.width, icon.height),
            new Vector2(0.5f, 0.5f) // ���ĵ�
        );
        Icon.GetComponent<Image>().sprite = iconSprite;
        //�����ı�
        Name.GetComponent<TextMeshProUGUI>().text = skillData.Name;
        Content.GetComponent<TextMeshProUGUI>().text = skillData.SkillDescribe;
        //�趨��Ƭ����
        data = new SkillCardData(skillData.Id, skillData.IconUIPath, CardType.SkillCard, skillData, iconSprite);
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
        //��װ�������� ��ѡ�����
        SkillInventoryPanel panel =  UIManager.MainInstance.OpenPanel(UIConst.SkillInventoryPanel) as SkillInventoryPanel;
        //����ѡ��ļ��ܿ�Ƭ
        //Ȼ�����ѡ�����滻�¼� ���滻�����͹ر�ҳ��
        panel.IntroducedCard(data);
    }
    private void OnSkillCardEquiped(SkillBar bar,SkillCardData data)
    {
        //�ж��ǲ���������ƣ�����Ǿ�ɾ������
        if(data.ID == this.data.ID)
        {
            //�ر�ҳ�棬֪ͨ���Ե�ѡ��
            UIManager.MainInstance.ClosePanel(UIConst.AbilityCardPanel);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
