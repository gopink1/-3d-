using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Զ�������Ľű�
/// </summary>
public class RangedWeapon : IWeapon
{
    private GameObject weaponEffect;//������Ч

    public void SetVFX( GameObject obj)
    {
        weaponEffect = obj;
    }
    protected override void ShowEffet()
    {


    }
    public void Fire(Vector3 startPos,Vector3 targetPos,float distance)
    {
        GameObject obj = GameObject.Instantiate(weaponEffect,startPos,Quaternion.identity);

        obj.GetComponent<IceFireControl>().Launch(targetPos, distance);
    }
    protected override void ShowVoice()
    {
        throw new System.NotImplementedException();
    }
}
