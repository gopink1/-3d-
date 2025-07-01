using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ƭ��������
/// </summary>
public class CardData
{
    protected int id; //��Ƭ��id
    public int ID
    {
        get => id;
    }
    protected string cardUIPath;//��Ƭ��·��
    public string CardUIPath
    {
        get => cardUIPath;
    }
    protected CardType cardType;//��Ƭ������
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
