using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

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

    [SerializeField] private float originalDamageToHero;
    private float damageToHero;

    [SerializeField] private float attackCD;

    private FriendlyOBject aimFriendlyUnit;
    private bool attackReady = true;


    [SerializeField]private List<FriendlyOBject> FriendlyObjectInAttackRange;

    [Header("脚本组件")]
    [SerializeField] private Rigidbody rigidbody;

    //事件
    public Action OnHealthChanged;
    public Action<Enemy> OnDead;


    private void Awake()
    {
        FriendlyObjectInAttackRange = new List<FriendlyOBject>();
    }

    private void OnEnable()
    {
        Initialize();
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Update()
    {
        SetAimFriendlyUnit();
        TryAttack();
    }

    private void TryAttack()
    {
        if (!attackReady) return;
        if (FriendlyObjectInAttackRange.Count == 0)
            return;

        AttackAimFriendlyUnitOrFirstInRange();
    }
    private void FixedUpdate()
    {
        TryMove();
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (!attackReady) return;
    //    if (collision.gameObject.GetComponent<FriendlyUnitHealth>())       //等待重构
    //        Attack();

    //}



    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.TryGetComponent<FriendlyOBject>(out FriendlyOBject friendlyObject))
        {
            FriendlyObjectInAttackRange.Add(friendlyObject);
            friendlyObject.OnDestroyed += OnFriendlyUnitDead;
        }
    }

    private void OnFriendlyUnitDead(FriendlyOBject friendlyOBject)
    {
        if (FriendlyObjectInAttackRange.Contains(friendlyOBject))
        {
            Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            FriendlyObjectInAttackRange.Remove(friendlyOBject);
        }
        else
        {
            Debug.LogWarning("敌人不在PlayerAttack列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            friendlyOBject.OnDestroyed -= OnFriendlyUnitDead;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.TryGetComponent<FriendlyOBject>(out FriendlyOBject friendlyObject))
        {
            FriendlyObjectInAttackRange.Remove(friendlyObject);
            friendlyObject.OnDestroyed -= OnFriendlyUnitDead;
        }
    }

    private void TryMove()
    {
        rigidbody.velocity = Vector3.zero;
        if (aimFriendlyUnit == null)
            return;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyUnit))
            return;
        Move();
    }
    private void Move()
    {
        Vector3 direction = (aimFriendlyUnit.transform.position - transform.position).normalized;
        rigidbody.velocity = direction * Time.deltaTime * speed;
    }
    
    private void AttackAimFriendlyUnitOrFirstInRange()
    {
        attackReady = false;
        FriendlyOBject tempFriendlyObject;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyUnit))
        {
            tempFriendlyObject = aimFriendlyUnit;
        }
        else
        {
            tempFriendlyObject = FriendlyObjectInAttackRange[0];
        }
        switch (tempFriendlyObject.GetFriendlyUnitType())
        {

            case (FriendlyUnitType.Unit):
                tempFriendlyObject.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToUnit);
                break;
            case (FriendlyUnitType.Player):
                tempFriendlyObject.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToPlayer);
                break;
            case (FriendlyUnitType.Hero):
                tempFriendlyObject.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToHero);
                break;
            case (FriendlyUnitType.Building):
                tempFriendlyObject.GetComponent<Building>().ReceiveDamage(damageToBuilding);
                break;
        }

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
        OnDead?.Invoke(this);
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

        rigidbody = GetComponent<Rigidbody>();
        FriendlyObjectInAttackRange.Clear();
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
                return;
            }
        }

        aimFriendlyUnit = null;
        Debug.LogWarning(gameObject.name + "未找到目标");
        
    }


}
