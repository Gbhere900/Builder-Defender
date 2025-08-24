using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private List<Enemy> enemiesInRange;
    [SerializeField] private float timeToLive = 2;
    [SerializeField] private float damageCD;
    [SerializeField] private Damage damage;
    private void Awake()
    {
        enemiesInRange = new List<Enemy>();
        damage.damageSource = gameObject;
    }
    private void OnEnable()
    {
        enemiesInRange.Clear();
        StartCoroutine(WaitForRecycle());
        StartCoroutine(WaitForDamage());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy ))
        {
            enemiesInRange.Add(enemy);
            enemy.OnDead += OnEnemyDead;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRange.Remove(enemy);
            enemy.OnDead -= OnEnemyDead;
        }
    }

    private void OnEnemyDead(Enemy enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            //Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            enemiesInRange.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("���˲���EnenmiesInRange�б���ʱ�����¼�������˵���������뿪�����˺���Χ��δȡ�������¼�������ִ��ȡ������");
            enemy.OnDead -= OnEnemyDead;
        }

    }

    IEnumerator WaitForDamage()
    {
        while (true)
        {
            
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].ReceiveDamage(damage);
            }
            yield return new WaitForSeconds(damageCD);
        }

    }

    IEnumerator WaitForRecycle()
    {
        yield return new WaitForSeconds(timeToLive);
        ObjectPoolManager.Instance().ReleaseObject(this.gameObject);
    }
}
