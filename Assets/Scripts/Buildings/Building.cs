using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FriendlyObject))]
public abstract class Building : MonoBehaviour
{
    [Header("��ֵ")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int maxLevel = 2;

    [Header("������ֵ")]
    [SerializeField] protected List<LevelUpEnhance_AttackBuilding> LevelUPEnhance_L2;
    [SerializeField] protected List<LevelUpEnhance_AttackBuilding> LevelUPEnhance_L3;

    [Header("����ͨ�ýű����")]

    protected Rigidbody rb;

    public Action OnHealthChanged;

    protected virtual void Awake()              //�����Awake��Onenabe�ǵ�base
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()           //�����Awake��Onenabe�ǵ�base
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
        Destroy(gameObject);    //�ȴ��ع�
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
