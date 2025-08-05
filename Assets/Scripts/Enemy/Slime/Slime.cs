using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float rangedDamageAdjustment;
    public override void ReceiveDamage(Damage_Friendly damage_Friendly)  //子类根据类型重写这个函数,例如根据是否为近战伤害重新计算damage，根据damageSource的组件判断是否有额外伤害
    {
        float damage;
        if (damage_Friendly.damageType_Friendly == DamageType_Friendly.Ranged)
        {
            damage = damage_Friendly.damage * rangedDamageAdjustment/100;
        }
        else
        {
            damage = damage_Friendly.damage;
        }

        health -= Mathf.Min(health, damage);
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            Die();
        }


    }
}
