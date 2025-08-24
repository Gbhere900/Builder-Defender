using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float rangedDamageAdjustment;
    public override void ReceiveDamage(Damage damage_Friendly)  //�������������д�������,��������Ƿ�Ϊ��ս�˺����¼���damage������damageSource������ж��Ƿ��ж����˺�
    {
        float damage;
        if (damage_Friendly.damageType == DamageType.Ranged)
        {
            damage = damage_Friendly.damage * rangedDamageAdjustment/100;
        }
        else
        {
            damage = damage_Friendly.damage;
        }

        health -= Mathf.Min(health, damage);
        OnHealthChanged?.Invoke();
        if (health <= 0)
        {
            Die();
        }


    }
}
