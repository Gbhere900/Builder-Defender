using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damage 
{
    public DamageType damageType;
    public float originalDamage;
    public float damage;
    public GameObject damageSource;
    Damage(DamageType damageType_Friendly,float originalDamage,GameObject damageSource)
    {
        this.damageType = damageType_Friendly;
        this.originalDamage = originalDamage;
        this.damageSource = damageSource;
        this.damage = originalDamage;
    }


}
