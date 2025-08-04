using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHealth : FriendlyUnitHealth
{
    [SerializeField] private float damageCorrection = 1;
    public override void ReceiveDamage(float damage)
    {
        Debug.Log("�ѷ���λ�յ��˺�" + damage);
        health -= Math.Min(damage * damageCorrection, health);
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            Die();
        }
    }
}
