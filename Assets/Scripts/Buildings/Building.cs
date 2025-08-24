using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FriendlyObject))]
public abstract class Building : MonoBehaviour
{

    [Header("数值")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;

    

    [Header("建筑通用脚本组件")]

    protected Rigidbody rb;

    public Action<float,float> OnHealthChanged;

    protected virtual void Awake()              //后面的Awake和Onenabe记得base
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()           //后面的Awake和Onenabe记得base
    {
          
    }

    public void ReceiveDamage(Damage damage)        //增加对建筑易伤加入判断重构
    {
        float formerHealth = health;
        health -= Math.Min(health, damage.damage);
        OnHealthChanged.Invoke(formerHealth,health);
        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);    //等待重构
    }

    public float GetMaxHealth()
    {

        return maxHealth; 
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public abstract void ApplyAllBuildingUpgrades(BuildingData buildingData);

     
}
