using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeaponFactory
{
    public abstract IWeapon CreateSword(string weaponIndex);
    public abstract IWeapon CreateBattleaxe(string weaponIndex);
    public abstract IWeapon CreateIceFire(string weaponIndex);
}
