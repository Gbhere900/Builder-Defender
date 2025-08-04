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
        Debug.Log("detect����");
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
            Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            unit.enemiesInDetectRange.Remove(enemy);
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
            unit.enemiesInDetectRange.Remove(enemy);
            enemy.OnDead -= OnEnemyDead;
        }
    }
}
