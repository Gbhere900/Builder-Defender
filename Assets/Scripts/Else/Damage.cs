using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damage 
{
    public DamageType damageType;
    public float damage;
    public GameObject damageSource;
    Damage(DamageType damageType_Friendly,float damage,GameObject damageSource)
    {
        this.damageType = damageType_Friendly;
        this.damageSource = damageSource;
        this.damage = damage;
    }


}
