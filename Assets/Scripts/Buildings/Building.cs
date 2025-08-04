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
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int maxLevel = 2;

    [Header("升级数值")]
    [SerializeField] protected List<LevelUpEnhance_AttackBuilding> LevelUPEnhance_L2;
    [SerializeField] protected List<LevelUpEnhance_AttackBuilding> LevelUPEnhance_L3;

    [Header("建筑通用脚本组件")]

    protected Rigidbody rb;

    public Action OnHealthChanged;

    protected virtual void Awake()              //后面的Awake和Onenabe记得base
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()           //后面的Awake和Onenabe记得base
    {

    }

    public void ReceiveDamage(float damage)
    {
        health -= Math.Min(health, damage);
        OnHealthChanged.Invoke();
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
}
