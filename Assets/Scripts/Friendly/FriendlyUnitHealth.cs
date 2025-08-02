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
    public Action OnHealthChanged;    //应该像上面一样不需要带参数，后续需要再加
    public void ReceiveDamage(float damage)
    {
        Debug.Log("友方单位收到伤害" + damage);
        health -= Math.Min(damage,health);
        OnHealthChanged.Invoke();
        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()       //后面可能改成抽象类
    {
        isDead = true;
        OnDead?.Invoke(this);
        Debug.Log("玩家死亡");
    }


    public void ReceiveHealing(float damage)
    {
        Debug.Log("友方单位收到治疗" + damage);
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
