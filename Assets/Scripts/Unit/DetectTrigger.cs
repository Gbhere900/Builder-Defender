using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTrigger : MonoBehaviour
{
    [SerializeField] private float detectRadius = 10;
    private CapsuleCollider detectCollider;
    private Unit unit;

    private void OnEnable()
    {
       Initialize();
    }

    private void Initialize()
    {
        detectCollider = GetComponent<CapsuleCollider>();
        detectCollider.radius = detectRadius;
        unit = GetComponentInParent<Unit>();
    }

    private void OnDisable()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("detect触发");
        if (other.isTrigger)
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            unit.enemiesInDetectRange.Add(enemy);
            enemy.OnDead += OnEnemyDead;
        }  
    }

    private void OnEnemyDead(Enemy enemy)
    {
        if (unit.enemiesInDetectRange.Contains(enemy))
        {
            Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            unit.enemiesInDetectRange.Remove(enemy);
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
            unit.enemiesInDetectRange.Remove(enemy);
            enemy.OnDead -= OnEnemyDead;
        }
    }
}
