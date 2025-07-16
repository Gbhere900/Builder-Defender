using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Building : MonoBehaviour
{
    [Header("数值")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;

    [Header("脚本组件")]

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
        Destroy(gameObject);
    }
}
