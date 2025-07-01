using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttr
{
    private string weaponIndex;
    private float atk;
    private float range;

    private string weaponName;
    private string animatorName;
    public WeaponAttr(string weaponIndex, float atk,float range, string weaponName, string animatorName)
    {
        this.atk = atk;
        this.range = range;
        this.weaponName=weaponName;
        this.animatorName=animatorName;
        this.weaponIndex=weaponIndex;
    }

    public float GetAtk()
    {
        return atk;
    }

    public float GetRange()
    {
        return range;
    }
    public string GetWeaponIndex()
    {
        return weaponIndex;
    }
    public string GetName()
    {
        return weaponName;
    }
    public string GetAnimatorName()
    {
        return animatorName;
    }
}
