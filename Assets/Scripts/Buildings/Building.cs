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

    

    [Header("����ͨ�ýű����")]

    protected Rigidbody rb;

    public Action<float,float> OnHealthChanged;

    protected virtual void Awake()              //�����Awake��Onenabe�ǵ�base
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()           //�����Awake��Onenabe�ǵ�base
    {
          
    }

    public void ReceiveDamage(Damage damage)        //���ӶԽ������˼����ж��ع�
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

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public abstract void ApplyAllBuildingUpgrades(BuildingData buildingData);

     
}
