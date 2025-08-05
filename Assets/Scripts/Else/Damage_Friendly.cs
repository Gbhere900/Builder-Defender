using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damage_Friendly 
{
    public DamageType_Friendly damageType_Friendly;
    public float originalDamage;
    public float damage;
    public GameObject damageSource;
    Damage_Friendly(DamageType_Friendly damageType_Friendly,float originalDamage,GameObject damageSource)
    {
        this.damageType_Friendly = damageType_Friendly;
        this.originalDamage = originalDamage;
        this.damageSource = damageSource;
        this.damage = originalDamage;
    }


}
