using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FriendlyOBject))]
public abstract class Building : MonoBehaviour
{
    [Header("数值")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int maxLevel = 2;

    [Header("升级数值")]
    [SerializeField] protected List<LevelUpEnhance> LevelUPEnhance_L2;
    [SerializeField] protected List<LevelUpEnhance> LevelUPEnhance_L3;

    [Header("建筑通用脚本组件")]

    [SerializeField] protected Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void ReceiveDamage(float damage)
    {
        health -= Math.Max(health, damage);
        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);    //等待重构
    }
}
