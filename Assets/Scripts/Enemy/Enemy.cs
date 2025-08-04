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
    private float health;

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

    private FriendlyObject aimFriendlyObject;
    private bool attackReady = true;

    [SerializeField] private float turnSpeed = 10;

  //  private bool isSlow = false;
    private Coroutine slowCoroutine;


    [SerializeField]private List<FriendlyObject> FriendlyObjectInAttackRange;

    [Header("脚本组件")]
    private Rigidbody rb;

    //事件
    public Action OnHealthChanged;
    public Action<Enemy> OnDead;


    private void Awake()
    {
        FriendlyObjectInAttackRange = new List<FriendlyObject>();
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
        TryRotateToAimFriendlyObject();
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

        if (other.gameObject.TryGetComponent<FriendlyObject>(out FriendlyObject friendlyObject))
        {
            Debug.LogWarning(other.gameObject.name + "进入范围");
            FriendlyObjectInAttackRange.Add(friendlyObject);
            friendlyObject.OnDestroyed += OnFriendlyUnitDead;
        }
    }

    private void OnFriendlyUnitDead(FriendlyObject friendlyOBject)
    {
        if (FriendlyObjectInAttackRange.Contains(friendlyOBject))
        {
            Debug.Log("Enemy目标的FriendlyObject死亡，将其从攻击目标列表移除");
            FriendlyObjectInAttackRange.Remove(friendlyOBject);
        }
        else
        {
            Debug.LogWarning("友方单位不在Enemy列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            friendlyOBject.OnDestroyed -= OnFriendlyUnitDead;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.TryGetComponent<FriendlyObject>(out FriendlyObject friendlyObject))
        {
            FriendlyObjectInAttackRange.Remove(friendlyObject);
            friendlyObject.OnDestroyed -= OnFriendlyUnitDead;
        }
    }

    private void TryMove()
    {
        rb.velocity = Vector3.zero;
        if (aimFriendlyObject == null)
            return;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyObject))
        {
            Debug.Log(gameObject.name + "的攻击目标在攻击范围中，停止移动");
            return;
        }
            
        Move();
    }

    private void TryRotateToAimFriendlyObject()
    {
        if (aimFriendlyObject == null)
            return;
        RotateToAimFriendlyObject();
    }
    private void RotateToAimFriendlyObject()
    {
        if (aimFriendlyObject != null)
        {
            Vector3 targetDirection = aimFriendlyObject.transform.position - transform.position;
            targetDirection.y = 0;

            if (targetDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // 渐进式转向：使用Slerp实现平滑过渡
                Quaternion newRotation = Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    turnSpeed * Time.fixedDeltaTime
                );

                rb.MoveRotation(newRotation);
            }
        }
    }
    private void Move()
    {
        Vector3 direction = (aimFriendlyObject.transform.position - transform.position).normalized;
        rb.velocity = direction * Time.deltaTime * speed;
    }
    
    private void AttackAimFriendlyUnitOrFirstInRange()
    {
        attackReady = false;
        FriendlyObject tempFriendlyObject;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyObject))
        {
            tempFriendlyObject = aimFriendlyObject;
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
        damageToHero = originalDamageToHero;

        rb = GetComponent<Rigidbody>();
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
            List<FriendlyObject> friendlyObjectList = FriendlyOBjectManager.Instance().GetFriendlyObjetcList();
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
                aimFriendlyObject = friendlyObjectList[index];
                Debug.Log(gameObject.name + "已找到目标\n"+ aimFriendlyObject);
                return;
            }
        }

        aimFriendlyObject = null;
        Debug.LogWarning(gameObject.name + "未找到目标");
        
    }

    public bool TrySlowSpeeddForSeconds(float seconds, float percent)
    {
        if(percent < speed / originalSpeed * 100)
        {
            ChangeSpeedForSeconds(seconds,percent);
            return true;
        }

        return false;

    }
    public void ChangeSpeedForSeconds(float seconds, float percent)
    {
        Debug.Log("已改变速度");
        speed =originalSpeed * percent/100;                 //后续看情况再重构得更有拓展性，目前只支持在originalSpeed基础上进行加减速，无法处理多种加减速效果共同影响的情况
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
            slowCoroutine = StartCoroutine(WaitForSpeedRecover(seconds));
    }

IEnumerator WaitForSpeedRecover(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        speed = originalSpeed;                      //完成build系统后重构
        Debug.Log("速度恢复");
    }
}
