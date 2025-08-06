using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FriendlyObject))]
public class FriendlyUnitHealth : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
   // [SerializeField] private FriendlyUnitType friendlyUnitType;         //应该可以删去，只用FriendlyObject中存储的友方单位类型
  //  private bool isDead = false;

    public Action<FriendlyUnitHealth> OnDead;
    public Action OnHealthChanged;    //应该像上面一样不需要带参数，后续需要再加

    private void OnEnable()
    {
        health = maxHealth;
    }
    public virtual void ReceiveDamage(Damage damage)            //加入易伤后重构
    {
        Debug.Log("友方单位收到伤害" + damage.damage);
        health -= Math.Min(damage.damage,health);
        OnHealthChanged.Invoke();
        if(health <= 0)
        {
            Die();
        }
    }

    //Die函数通过将enable设为false触发FriendlyObject的OnDisable,触发死亡事件
    public virtual void Die()       //后面可能改成抽象类         
    {
       // isDead = true;
        OnDead?.Invoke(this);
        Debug.Log(gameObject.name + "死亡");
        gameObject.SetActive(false);
    }


    public void ReceiveHealing(Damage damage_Friendly)
    {
        Debug.Log("友方单位收到治疗" + damage_Friendly.damage);
        health = Math.Min(maxHealth, health + damage_Friendly.damage);
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

    //public FriendlyUnitType GetFriendlyUnitType()
    //{
    //    return friendlyUnitType;
    //}
}
