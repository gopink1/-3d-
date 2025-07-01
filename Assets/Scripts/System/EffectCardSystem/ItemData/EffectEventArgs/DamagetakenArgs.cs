using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagetakenArgs : EffectEventArgs
{
    private float damage;
    public float Damage
    {
        get => damage;
    }
    private GameObject attacker;
    public GameObject Attacker
    {
        get => attacker;
    }
    public DamagetakenArgs(float damage, GameObject attacker)
    {
        this.damage=damage;
        this.attacker=attacker;
    }
}
