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
    //卡牌生成系统
    //随机生成卡牌的逻辑


    public CardBornSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        //初始化卡牌生成系统

    }

    public override void Release()
    {
        
    }

    public override void Update()
    {
        
    }
    /// <summary>
    /// 随机生成属性卡片
    /// </summary>
    public void RandomBornAttrCard()
    {
        //随机百分比还是固定
        //随机bool值
        bool isPercentage = UnityEngine.Random.Range(0, 2) == 1;
        //随机属性
        //随机PlayerAttribute枚举
        Array values = Enum.GetValues(typeof(PlayerAttribute));
        PlayerAttribute randomAttribute = (PlayerAttribute)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        if(randomAttribute == PlayerAttribute.MaxMP)
        {
            randomAttribute = PlayerAttribute.MaxHP;
        }
        //随机count
        //随机float
        int baseValue = (int)UnityEngine.Random.Range(1f, 4f); // 基础值范围
        float randomMultiplier0 = UnityEngine.Random.Range(0.1f, 0.2f); // 随机倍率
        float randomMultiplier = (float)Math.Round(randomMultiplier0, 2);
        float finalValue = isPercentage ? randomMultiplier : baseValue; // 保留一位小数
        //生成UI预制体
        CardFactory factory = GameBaseFactory.GetCardFactory();
        AttrCard  attr =  factory.CreateAttrCard() as AttrCard;
        PanelBase panel = UIManager.MainInstance.GetPanel(UIConst.AbilityCardPanel);
        attr.gameObject.transform.SetParent(panel.gameObject.transform,false);

        //赋予预制体属性
        attr.SetCardData(randomAttribute, finalValue, isPercentage);
        //设置卡牌渐入效果
        // 创建组合动画
        RectTransform rectTransform = attr.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
        attr.transform.GetComponent<CanvasGroup>().alpha = 0.1f;
        DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(rectTransform.DOScale(1f, 1.2f)).SetUpdate(true)  // 缩放动画
            .Join(attr.transform.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true));          // 透明度动画
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
            .Append(rectTransform.DOScale(1f, 1.2f)).SetUpdate(true)  // 缩放动画
            .Join(card.transform.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true));          // 透明度动画
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
            .Append(rectTransform.DOScale(1f, 1.2f)).SetUpdate(true)  // 缩放动画
            .Join(card.transform.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true));          // 透明度动画
        card.gameObject.SetActive(true);

    }
}
