using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FriendlyObject))]
public class FriendlyUnitHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
   // [SerializeField] private FriendlyUnitType friendlyUnitType;         //Ӧ�ÿ���ɾȥ��ֻ��FriendlyObject�д洢���ѷ���λ����
  //  private bool isDead = false;

    public Action<FriendlyUnitHealth> OnDead;
    public Action OnHealthChanged;    //Ӧ��������һ������Ҫ��������������Ҫ�ټ�

    private void OnEnable()
    {
        health = maxHealth;
    }
    public void ReceiveDamage(float damage)
    {
        Debug.Log("�ѷ���λ�յ��˺�" + damage);
        health -= Math.Min(damage,health);
        OnHealthChanged.Invoke();
        if(health <= 0)
        {
            Die();
        }
    }

    //Die����ͨ����enable��Ϊfalse����FriendlyObject��OnDisable,���������¼�
    public virtual void Die()       //������ܸĳɳ�����         
    {
       // isDead = true;
        OnDead?.Invoke(this);
        Debug.Log(gameObject.name + "����");
        gameObject.SetActive(false);
    }


    public void ReceiveHealing(float damage)
    {
        Debug.Log("�ѷ���λ�յ�����" + damage);
        health = Math.Min(maxHealth, health + damage);
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
