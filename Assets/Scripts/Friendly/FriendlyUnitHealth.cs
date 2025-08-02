using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FriendlyOBject))]
public class FriendlyUnitHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private FriendlyUnitType friendlyUnitType;
    private bool isDead = false;

    public Action<FriendlyUnitHealth> OnDead;
    public Action OnHealthChanged;    //Ӧ��������һ������Ҫ��������������Ҫ�ټ�
    public void ReceiveDamage(float damage)
    {
        Debug.Log("�ѷ���λ�յ��˺�" + damage);
        health -= Math.Min(damage,health);
        OnHealthChanged.Invoke();
        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()       //������ܸĳɳ�����
    {
        isDead = true;
        OnDead?.Invoke(this);
        Debug.Log("�������");
    }


    public void ReceiveHealing(float damage)
    {
        Debug.Log("�ѷ���λ�յ�����" + damage);
        health = Math.Min(maxHealth, health + damage);
        OnHealthChanged.Invoke();
        
    }

    public bool IsFullHealth()
    {
        return health >= maxHealth;
    }


    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public FriendlyUnitType GetFriendlyUnitType()
    {
        return friendlyUnitType;
    }
}
