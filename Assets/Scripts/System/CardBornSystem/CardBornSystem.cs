using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CardBornSystem : IGameSystem
{
    //��������ϵͳ
    //������ɿ��Ƶ��߼�


    public CardBornSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        //��ʼ����������ϵͳ

    }

    public override void Release()
    {
        
    }

    public override void Update()
    {
        
    }
    /// <summary>
    /// ����������Կ�Ƭ
    /// </summary>
    public void RandomBornAttrCard()
    {
        //����ٷֱȻ��ǹ̶�
        //���boolֵ
        bool isPercentage = UnityEngine.Random.Range(0, 2) == 1;
        //�������
        //���PlayerAttributeö��
        Array values = Enum.GetValues(typeof(PlayerAttribute));
        PlayerAttribute randomAttribute = (PlayerAttribute)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        if(randomAttribute == PlayerAttribute.MaxMP)
        {
            randomAttribute = PlayerAttribute.MaxHP;
        }
        //���count
        //���float
        int baseValue = (int)UnityEngine.Random.Range(1f, 4f); // ����ֵ��Χ
        float randomMultiplier0 = UnityEngine.Random.Range(0.1f, 0.2f); // �������
        float randomMultiplier = (float)Math.Round(randomMultiplier0, 2);
        float finalValue = isPercentage ? randomMultiplier : baseValue; // ����һλС��
        //����UIԤ����
        CardFactory factory = GameBaseFactory.GetCardFactory();
        AttrCard  attr =  factory.CreateAttrCard() as AttrCard;
        PanelBase panel = UIManager.MainInstance.GetPanel(UIConst.AbilityCardPanel);
        attr.gameObject.transform.SetParent(panel.gameObject.transform,false);

        //����Ԥ��������
        attr.SetCardData(randomAttribute, finalValue, isPercentage);
        //���ÿ��ƽ���Ч��
        // ������϶���
        RectTransform rectTransform = attr.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
        attr.transform.GetComponent<CanvasGroup>().alpha = 0.1f;
        DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(rectTransform.DOScale(1f, 1.2f)).SetUpdate(true)  // ���Ŷ���
            .Join(attr.transform.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true));          // ͸���ȶ���
        attr.gameObject.SetActive(true);
    }

    public void RandomBornSkillCard()
    {
        CardFactory cardFactory = GameBaseFactory.GetCardFactory();
        int index = UnityEngine.Random.Range(0, 5);
        if (index == 0) index =1;
        SkillCard card = cardFactory.CreateSkillCard(index) as SkillCard;

        PanelBase panel = UIManager.MainInstance.GetPanel(UIConst.AbilityCardPanel);
        card.gameObject.transform.SetParent(panel.gameObject.transform, false);

        RectTransform rectTransform = card.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        card.transform.GetComponent<CanvasGroup>().alpha = 0.1f;
        DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(rectTransform.DOScale(1f, 1.2f)).SetUpdate(true)  // ���Ŷ���
            .Join(card.transform.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true));          // ͸���ȶ���
        card.gameObject.SetActive(true);

    }

    public void RandomBornItemCard()
    {
        CardFactory cardFactory = GameBaseFactory.GetCardFactory();
        int index = UnityEngine.Random.Range(0, 8);
        if (index == 1) index = 7;
        ItemCard card = cardFactory.CreateItemCard(index) as ItemCard;

        PanelBase panel = UIManager.MainInstance.GetPanel(UIConst.AbilityCardPanel);
        card.gameObject.transform.SetParent(panel.gameObject.transform, false);

        RectTransform rectTransform = card.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        card.transform.GetComponent<CanvasGroup>().alpha = 0.1f;
        DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(rectTransform.DOScale(1f, 1.2f)).SetUpdate(true)  // ���Ŷ���
            .Join(card.transform.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true));          // ͸���ȶ���
        card.gameObject.SetActive(true);

    }
}
