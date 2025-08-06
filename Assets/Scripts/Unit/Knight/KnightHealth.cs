using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHealth : FriendlyUnitHealth
{
    [SerializeField] private float rangedDamageCorrection = 100;
    public override void ReceiveDamage(Damage damage)
    {
        float d;
        if (damage.damageType == DamageType.Ranged)
        {
            d = damage.damage * rangedDamageCorrection/100;
        }
        else
        {
            d= damage.damage;
        }

        
        Debug.Log("�ѷ���λ�յ��˺�" + damage);
        health -= Math.Min(d, health);
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            Die();
        }
    }
}
