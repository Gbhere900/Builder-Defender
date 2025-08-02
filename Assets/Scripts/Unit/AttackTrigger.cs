using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private float attackRadius = 10;
    private CapsuleCollider attackCollider;
    private Unit unit;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        attackCollider = GetComponent<CapsuleCollider>();
        attackCollider.radius = attackRadius;
        unit = GetComponentInParent<Unit>();
    }

    private void OnDisable()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("attack触发");
        if (other.isTrigger)
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            unit.enemiesInAttackRange.Add(enemy);
            enemy.OnDead += OnEnemyDead;
        }
    }

    private void OnEnemyDead(Enemy enemy)
    {
        if (unit.enemiesInAttackRange.Contains(enemy))
        {
            Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            unit.enemiesInAttackRange.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("敌人不在PlayerAttack列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            enemy.OnDead -= OnEnemyDead;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            unit.enemiesInAttackRange.Remove(enemy);
            enemy.OnDead -= OnEnemyDead;
        }
    }
}
