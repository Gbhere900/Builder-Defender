using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


//玩家待完成BUFF部分，完成后记得加上Initialize（）函数
public class PlayerAttack : MonoBehaviour
{
    private Enemy attackTarget;
    private List<Enemy> attackTargetList;
    private int attackIndex = 0;                        //用index在ListRemove后可能会有BUg
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Arrow arrowPrefab;

    [SerializeField] private Damage damage_Friendly;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowTimeToLive;
    [SerializeField] private float attackCD;
    private bool canAttack = true;

    private void OnEnable()
    {
        attackTargetList = new List<Enemy>();
        Initialize();
    }
    private void Attack()
    {
        Arrow arrow = ObjectPoolManager.Instance().GetObject(arrowPrefab.gameObject).GetComponent<Arrow>();
        // Arrow arrow = GameObject.Instantiate(arrowPrefabs,shootPoint);//对象池实现后重构
        arrow.Initialize(shootPoint.position, attackTarget.gameObject, damage_Friendly, arrowSpeed,arrowTimeToLive);

        canAttack = false;
        StartCoroutine(WaitForAttackCD());
    }

    public void SetNextAttackTarget()
    {
        if(attackTargetList.Count == 0)
        {
            Debug.LogWarning("攻击目标列表为空，无法设置下一个攻击目标");
            attackTarget = null;
            attackIndex = -1;
            return;
        }

        attackIndex++;
        if(attackIndex >= attackTargetList.Count)
        {
            attackIndex = 0;
        }

        SetEnemyAtIndexAsAttackTarget(attackIndex);
        
    }
    //玩家的索敌逻辑是，当第一个敌人进入时直接锁定，点击鼠标切换锁定，当目前目标死亡和离开攻击范围时切换锁定最近敌人
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
           // Debug.Log(enemy.gameObject.name + "进入玩家攻击范围");
            attackTargetList.Add(enemy);
            enemy.OnDead += OnEnemyDead;

            if(attackTargetList.Count == 1 )
            {
                attackIndex = 0;
                SetEnemyAtIndexAsAttackTarget(attackIndex);
            }
        }
    }

    private void OnEnemyDead(Enemy enemy)
    {
        if(attackTargetList.Contains(enemy))
        {
          //  Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            attackTargetList.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("敌人不在PlayerAttack列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            enemy.OnDead -= OnEnemyDead;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.gameObject.name + "离开范围");
            attackTargetList.Remove(enemy);
            enemy.OnDead -=  OnEnemyDead;
            if(attackTarget = enemy)
            {
               // Debug.Log("当前攻击目标离开攻击范围");
                SetClosestAttackTargetAsAttackTarget(enemy);
                
            }
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetNextAttackTarget();
        }

        if(canAttack && attackTarget != null)
        {
            if(attackTarget == null )
            {

            }
            Attack();
        }
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack  = true;
    }

    private void SetClosestAttackTargetAsAttackTarget(Enemy enemy)   //这里的Enemy形参没用
    {
        if(attackTargetList.Count == 0)
        {
         //   Debug.Log("攻击范围内已无敌人，设置最近攻击目标为空");
            attackTarget.OnDead -= SetClosestAttackTargetAsAttackTarget;
            attackTarget = null;
            attackIndex = -1;
            return;
        }
            
        float minDistance = float.MaxValue;
        for(int i = 0;i<attackTargetList.Count;i++)
        {

            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) <minDistance)
            {
                attackIndex = i;
                minDistance = Vector3.Distance(transform.position, attackTargetList[i].transform.position);
            }
        }

        SetEnemyAtIndexAsAttackTarget(attackIndex);
       
    }

    private void SetEnemyAtIndexAsAttackTarget(int Index)   //所有设置attackTarget的逻辑都要经过这里
    {
        if (Index >= attackTargetList.Count)
        {
            Debug.LogWarning("目标敌人索引大于List容量，角色设置攻击目标失败");
            attackTarget = null;
            attackIndex = -1;
            return;
            
        }
        if(attackTarget != null)
        {
            attackTarget.OnDead -= SetClosestAttackTargetAsAttackTarget;
        }

        attackTarget = attackTargetList[attackIndex];
        attackTarget.OnDead +=  SetClosestAttackTargetAsAttackTarget;   //可能要重构多封装一层
    }

    private void Initialize()
    {
        damage_Friendly.damage = damage_Friendly.originalDamage;
        damage_Friendly.damageSource = this.gameObject;
    }
    
}
