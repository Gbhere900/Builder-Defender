using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBuilding : Building
{
    [SerializeField] protected Arrow arrowPrefabs;
    [SerializeField] protected float arrowSpeed;

    [SerializeField] protected float attackCD;
    [SerializeField] protected bool attackReady = true;
    [SerializeField] protected float originalDamage;
    [SerializeField] protected float damage;

    [SerializeField] protected Transform shootPoint;

    [Header("攻击建筑脚本组件")]
    [SerializeField] protected CapsuleCollider attackCollider;

    protected List<Enemy> attackTargetList;
    protected Enemy attackTarget;

    
    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Update()
    {
        TryAttack();
    }

    private void Awake()
    {
        attackTargetList = new List<Enemy>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.name);
            attackTargetList.Add(enemy);
            enemy.OnDead += OnEnemyDead;
        }
            
    }
    private void OnEnemyDead(Enemy enemy)
    {
        if (attackTargetList.Contains(enemy))
        {
            Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            attackTargetList.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("敌人不在PlayerAttack列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            enemy.OnDead -= OnEnemyDead;
        }

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.name);
            attackTargetList.Remove(enemy);
            enemy.OnDead -= OnEnemyDead;
        }
            
    }

    private bool TryAttack()
    {
        if (!attackReady)
            return false;
        if(attackTargetList.Count == 0)
        {
            return false;
            
        }
        if (attackTargetList[0] == null)
        {
            attackTargetList.RemoveAt(0);
            return false;
        }

        attackTarget = attackTargetList[0];
        Attack();
        attackReady = false;
        StartCoroutine(WaitForAttackCD());
        return true;
    }


    public void LevelUP_L2(int index)
    {
        maxHealth += LevelUPEnhance_L2[index].maxHealthEnhance;
        damage *= (1 + LevelUPEnhance_L2[index].damageEnhance);
        attackCollider.radius *= (1 + LevelUPEnhance_L2[index].attackRangeEnhance);
        attackCD = attackCD / (1 + LevelUPEnhance_L2[index].attackSpeedEnhance);
        arrowSpeed *= (1 + LevelUPEnhance_L2[index].arrowSpeedEnhance);
        Debug.Log("升级到2级，选项为" + index);
    }

    public void LevelUP_L3(int index)
    {
        maxHealth += LevelUPEnhance_L3[index].maxHealthEnhance;
        damage *= (1 + LevelUPEnhance_L3[index].damageEnhance);
        attackCollider.radius *= (1 + LevelUPEnhance_L3[index].attackRangeEnhance);
        attackCD = attackCD / (1 + LevelUPEnhance_L3[index].attackSpeedEnhance);
        arrowSpeed *= (1 + LevelUPEnhance_L3[index].arrowSpeedEnhance);
        Debug.Log("升级到3级，选项为" + index);
    }

    protected virtual void Attack()
    {
        ShootArrow();           
    }

    private void ShootArrow()
    {
        Arrow arrow = ObjectPoolManager.Instance().GetObject(arrowPrefabs.gameObject).GetComponent<Arrow>();
        // Arrow arrow = GameObject.Instantiate(arrowPrefabs,shootPoint);//对象池实现后重构
        arrow.Initialize(shootPoint.position, attackTarget, damage, arrowSpeed);        //arrow换成投射物父类
    }
}
