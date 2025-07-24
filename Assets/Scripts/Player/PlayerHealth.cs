using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FriendlyOBject))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healh;
    private bool isDead = false;
    

    public void ReceiveDamage(float damage)
    {
        Debug.Log("玩家收到伤害" + damage);
        healh -= Math.Max(damage,healh);
        if(healh <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("玩家死亡");
    }
}
