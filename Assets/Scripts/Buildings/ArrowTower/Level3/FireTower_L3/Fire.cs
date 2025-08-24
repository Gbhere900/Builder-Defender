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
            //Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            enemiesInRange.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("敌人不在EnenmiesInRange列表中时死亡事件触发，说明敌人在离开火焰伤害范围后未取消订阅事件，现在执行取消订阅");
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
