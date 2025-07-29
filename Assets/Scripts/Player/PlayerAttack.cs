using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Enemy attackTarget;
    private List<Enemy> attackTargetList;
    private int attackIndex = 0;                        //用index在ListRemove后可能会有BUg
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Arrow arrowPrefab;

    [SerializeField] private float damage;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowTimeToLive;
    [SerializeField] private float attackCD;
    private bool canAttack = true;

    private void OnEnable()
    {
        attackTargetList = new List<Enemy>();
    }
    private void Attack()
    {
        Arrow arrow = ObjectPoolManager.Instance().GetObject(arrowPrefab.gameObject).GetComponent<Arrow>();
        // Arrow arrow = GameObject.Instantiate(arrowPrefabs,shootPoint);//对象池实现后重构
        arrow.Initialize(shootPoint.position, attackTarget, damage, arrowSpeed,arrowTimeToLive);

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.gameObject.name + "进入范围");
            attackTargetList.Add(enemy);
            if(attackTargetList.Count == 1 )
            {
                attackIndex = 0;
                SetEnemyAtIndexAsAttackTarget(attackIndex);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.gameObject.name + "离开范围");
            
            attackTargetList.Remove(enemy);
            if(attackTarget = enemy)
            {
                Debug.Log("当前攻击目标离开攻击范围");
                SetClosestAttackTargetAsAttackTarget();
                
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

    private void SetClosestAttackTargetAsAttackTarget()
    {
        if(attackTargetList.Count == 0)
        {
            Debug.Log("攻击范围内已无敌人，设置最近攻击目标为空");
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

    
}
