using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ICharacterFactory
{
    public abstract GameObject CreateEnemy(int enemyIndex);
    public abstract GameObject CreateBoss(int enemyIndex);
}
