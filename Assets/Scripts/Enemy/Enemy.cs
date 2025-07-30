using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("数值")]
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

    private FriendlyOBject aimFriendlyUnit;
    private bool attackReady = true;


    [Header("脚本组件")]
    [SerializeField] private Collider collider;
    [SerializeField] private Rigidbody rigidbody;

    //事件
    public Action OnHealthChanged;
    public Action<Enemy> OnDead;

    private void Awake()
    {
        
       // AimFriendlyUnit = GameObject.FindFirstObjectByType<PlayerHealth>(); //需要重构
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
        if (aimFriendlyUnit == null)
            return;
        Move();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!attackReady) return;
        if (collision.gameObject.GetComponent<FriendlyUnitHealth>())       //等待重构
            Attack();

    }

    private void Move()
    {
        Vector3 direction = aimFriendlyUnit.transform.position - transform.position;
        rigidbody.velocity = direction * Time.deltaTime * speed;
    }
    private void Attack()
    {
        attackReady = false;
        aimFriendlyUnit.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToPlayer);         //等待重构，用触发器控制造成伤害
        StartCoroutine(WaitForAttackCD());
    }

    public void ReceiveDamage(float damage)
    {
        health -= Mathf.Min(health, damage);
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            Die();
        }
        

    }


    public void Die()
    {
        ObjectPoolManager.Instance().ReleaseObject(this.gameObject);  
        OnDead.Invoke(this);
    }
    private void Initialize()
    {
        //增加buff系统后重构
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
        if(aimFriendlyUnit !=null)
        {
            aimFriendlyUnit.OnDestroyed -= SetAimFriendlyUnit;
        }
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

                //Debug.Log("敌人距离" + friendlyObjectList[i].name +"的距离为" + Vector3.Distance(transform.position, friendlyObjectList[i].transform.position));
                if (Vector3.Distance(transform.position, friendlyObjectList[i].transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, friendlyObjectList[i].transform.position);
                    index = i;
                }
            }
            if(index != -1)
            {
                aimFriendlyUnit = friendlyObjectList[index];
                Debug.Log(gameObject.name + "已找到目标\n"+ aimFriendlyUnit.gameObject.name);
                aimFriendlyUnit.OnDestroyed +=  SetAimFriendlyUnit;
                return;
            }
        }

        aimFriendlyUnit = null;
        Debug.LogWarning(gameObject.name + "未找到目标");
        
    }
}
