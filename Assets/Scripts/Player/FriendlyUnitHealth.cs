using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FriendlyOBject))]
public class FriendlyUnitHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    private bool isDead = false;

    public Action<FriendlyUnitHealth> OnDead;
    public void ReceiveDamage(float damage)
    {
        Debug.Log("�ѷ���λ�յ��˺�" + damage);
        health -= Math.Max(damage,health);
        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()       //������ܸĳɳ�����
    {
        isDead = true;
        OnDead.Invoke(this);
        Debug.Log("�������");
    }


    public void ReceiveHealing(float damage)
    {
        Debug.Log("�ѷ���λ�յ�����" + damage);
        health = Math.Min(maxHealth, health + damage);
        
    }

    public bool IsFullHealth()
    {
        return health >= maxHealth;
    }
}
