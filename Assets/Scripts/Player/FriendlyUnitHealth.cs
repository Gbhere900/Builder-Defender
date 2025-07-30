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
        Debug.Log("友方单位收到伤害" + damage);
        health -= Math.Max(damage,health);
        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()       //后面可能改成抽象类
    {
        isDead = true;
        OnDead.Invoke(this);
        Debug.Log("玩家死亡");
    }


    public void ReceiveHealing(float damage)
    {
        Debug.Log("友方单位收到治疗" + damage);
        health = Math.Min(maxHealth, health + damage);
        
    }

    public bool IsFullHealth()
    {
        return health >= maxHealth;
    }
}
