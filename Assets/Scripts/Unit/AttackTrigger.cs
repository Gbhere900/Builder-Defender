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
        Debug.Log("attack����");
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
            Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            unit.enemiesInAttackRange.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("���˲���PlayerAttack�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
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
