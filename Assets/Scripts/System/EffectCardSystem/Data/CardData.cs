using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 卡片的数据类
/// </summary>
public class CardData
{
    protected int id; //卡片的id
    public int ID
    {
        get => id;
    }
    protected string cardUIPath;//卡片的路径
    public string CardUIPath
    {
        get => cardUIPath;
    }
    protected CardType cardType;//卡片的类型
    public CardType CardType
    {
        get => cardType;
    }

    public CardData(int id,string cardUIPath,CardType cardType)
    {
        this.id = id;
        this.cardUIPath = cardUIPath;
        this.cardType = cardType;
    }
}
public enum CardType
{
    AttrCard,
    SkillCard,
    ItemCard
}
