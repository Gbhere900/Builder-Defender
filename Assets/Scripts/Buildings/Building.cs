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

    public Action OnHealthChanged;

    protected virtual void Awake()              //�����Awake��Onenabe�ǵ�base
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()           //�����Awake��Onenabe�ǵ�base
    {
        Initialize();   
    }

    public void ReceiveDamage(Damage damage)        //���ӶԽ������˼����ж��ع�
    {
        health -= Math.Min(health, damage.damage);
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

    private void Initialize()       //�����
    {

    }

    public abstract void ApplyAllBuildingUpgrades(BuildingData buildingData);

     
}
