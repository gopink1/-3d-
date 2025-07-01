using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCardData : CardData
{
    private SkillData skillData;
    public SkillData SkillData
    {
        get { return skillData; }
    }
    private Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
    }
    
    public SkillCardData(int id, string cardUIPath, CardType cardType, SkillData skillData,Sprite icon) : base(id, cardUIPath, cardType)
    {
        this.skillData = skillData;
        this.icon = icon;
    }
}
