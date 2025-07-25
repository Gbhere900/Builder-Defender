using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("��ֵ")]
    [SerializeField] private bool canFly;
    [SerializeField] private List<FriendlyUnitType> aimFriendlyUnitType;

    [SerializeField] private float originalMaxHealth;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float health;

    [SerializeField] private float originalSpeed;
    private float speed;

    [SerializeField] private float originalDamageToUnit;
    private float damageToUnit;

    [SerializeField] private float originalDamageToPlayer;
    private float damageToPlayer;

    [SerializeField] private float originalDamageToBuilding;
    private float damageToBuilding;

    [SerializeField] private float attackCD;

    private FriendlyOBject AimFriendlyUnit;
    private bool attackReady = true;


    [Header("�ű����")]
    [SerializeField] private Collider collider;
    [SerializeField] private Rigidbody rigidbody;

    //�¼�
    public Action OnHealthChanged;

    private void Awake()
    {
        
       // AimFriendlyUnit = GameObject.FindFirstObjectByType<PlayerHealth>(); //��Ҫ�ع�
    }

    private void OnEnable()
    {
        Initialize();
        SetAimFriendlyUnit();
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!attackReady) return;
        if (collision.gameObject.GetComponent<PlayerHealth>())
            Attack();

    }

    private void Move()
    {
        Vector3 direction = AimFriendlyUnit.transform.position - transform.position;
        rigidbody.velocity = direction * Time.deltaTime * speed;
    }
    private void Attack()
    {
        attackReady = false;
        AimFriendlyUnit.GetComponent<PlayerHealth>().ReceiveDamage(damageToPlayer);
        StartCoroutine(WaitForAttackCD());
    }

    public void ReceiveDamage(float damage)
    {
        health -= Mathf.Min(health, damage);
        if (health <= 0)
        {
            Die();
        }
        OnHealthChanged.Invoke();

    }


    public void Die()
    {
        GameObject.Destroy(gameObject);//ʵ�ֵ��˶���غ��ع�
    }
    private void Initialize()
    {
        //����buffϵͳ���ع�
        MaxHealth = originalMaxHealth;
        health = MaxHealth;
        speed = originalSpeed;
        damageToUnit = originalDamageToUnit;
        damageToPlayer = originalDamageToPlayer;
        damageToBuilding = originalDamageToBuilding;

        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public float GetHealth()
    {
        return health; 
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    private void SetAimFriendlyUnit()
    {
        foreach(FriendlyUnitType friendlyUnitType in aimFriendlyUnitType)
        {
            float minDistance = float.MaxValue;
            int index = -1;
            List<FriendlyOBject> friendlyObjectList = FriendlyOBjectManager.Instance().GetFriendlyObjetcList();
            for (int i = 0; i <friendlyObjectList.Count;i++)
            {
                if (friendlyObjectList[i].GetFriendlyUnitType() != friendlyUnitType)
                {
                    continue;
                }
                    
                if (Vector3.Distance(transform.position, friendlyObjectList[i].transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, friendlyObjectList[i].transform.position);
                    index = i;
                }
            }
            if(index != -1)
            {
                AimFriendlyUnit = friendlyObjectList[index];
                Debug.Log(gameObject.name + "���ҵ�Ŀ��\n"+ AimFriendlyUnit.gameObject.name);
                AimFriendlyUnit.OnDestroyed += () => SetAimFriendlyUnit();
                return;
            }
        }

        //Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��");
        
    }
}
