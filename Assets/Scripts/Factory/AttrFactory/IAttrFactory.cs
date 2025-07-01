using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAttrFactory
{
    public abstract EnemyAttr GetEnemyAttr(int AttrIndex);

    public abstract WeaponAttr GetWeaponAttr(string AttrIndex);
}
