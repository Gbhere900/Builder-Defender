using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class AttackBuilding : Building
{
    [SerializeField] protected Bullet bulletPrefabs;
    [SerializeField] protected float arrowSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCD;
    [SerializeField] protected bool attackReady = true;
    //[SerializeField] protected float originalDamage;        加入Buff系统后重构
    [SerializeField] protected float damage;

    [SerializeField] protected Transform shootPoint;

    [Header("攻击建筑脚本组件")]
    [SerializeField] protected CapsuleCollider attackCollider;

    protected List<GameObject> attackTargetList;
    protected GameObject attackTarget;

    
    protected IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Update()
    {
        TryAttack();
    }

    protected override void Awake()             //后面的Awake和Onenabe记得base
    {   
        base.Awake();
        attackTargetList = new List<GameObject>();
       
    }

    protected override void OnEnable()              //后面的Awake和Onenabe记得base
    {
        base.OnEnable();
        attackCollider.radius = attackRange;                //初始化时也需要使  attackCollider.radius = attackRange;   
    }


    protected void OnTriggerEnter(Collider other)     //回血塔重写这个函数
    {
        OnTriggerEnterLogic(other);     
    }

    protected virtual void OnTriggerEnterLogic(Collider other)
    {
        if(other.isTrigger)         //排除是触发器进入的情况
            return ;
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            attackTargetList.Add(enemy.gameObject);
            enemy.OnDead += OnEnemyDead;
        }
    }
    private void OnEnemyDead(Enemy enemy)
    {
        if (attackTargetList.Contains(enemy.gameObject))
        {
            Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
            attackTargetList.Remove(enemy.gameObject);
        }
        else
        {
            Debug.LogWarning("敌人不在PlayerAttack列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            enemy.OnDead -= OnEnemyDead;
        }

    }

    protected void OnTriggerExit(Collider other)        //回血塔重写这个函数
    {
        OnTriggerExitLogic(other);
            
    }

    protected virtual void OnTriggerExitLogic(Collider other)
    {
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.name);
            attackTargetList.Remove(enemy.gameObject);
            enemy.OnDead -= OnEnemyDead;
        }
    }

    protected virtual bool TryAttack()
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



    public void LevelUP_L2(int choice)
    {
        int index= choice - 1;
        maxHealth += LevelUPEnhance_L2[index].maxHealthEnhance;
        damage *= (1 + LevelUPEnhance_L2[index].damageEnhance/100);
        attackRange *= (1 + LevelUPEnhance_L2[index].attackRangeEnhance/100);
        attackCD = attackCD / (1 + LevelUPEnhance_L2[index].attackSpeedEnhance/100);
        arrowSpeed *= (1 + LevelUPEnhance_L2[index].arrowSpeedEnhance/100);

        AttackBuilding LevelUPBuilding =  GameObject.Instantiate
            (LevelUPEnhance_L2[index].LevelUpBuilding, transform.position, Quaternion.identity);
        LevelUPBuilding.Initialize(maxHealth,damage,attackRange,attackCD,arrowSpeed);                   
        Destroy(this.gameObject);                   //

        Debug.Log("升级到2级，选项为" + index);
    }

    public void LevelUP_L3(int choice)
    {
        int index = choice - 1;
        maxHealth += LevelUPEnhance_L3[index].maxHealthEnhance;
        damage *= (1 + LevelUPEnhance_L3[index].damageEnhance/100);
        attackCollider.radius *= (1 + LevelUPEnhance_L3[index].attackRangeEnhance/100);
        attackCD = attackCD / (1 + LevelUPEnhance_L3[index].attackSpeedEnhance/100);
        arrowSpeed *= (1 + LevelUPEnhance_L3[index].arrowSpeedEnhance/100);

        AttackBuilding LevelUPBuilding = GameObject.Instantiate
    (LevelUPEnhance_L3[index].LevelUpBuilding, transform.position, Quaternion.identity);
        LevelUPBuilding.Initialize(maxHealth, damage, attackRange, attackCD, arrowSpeed);

        Destroy(this.gameObject);
        Debug.Log("升级到3级，选项为" + index);
    }

    protected virtual void Attack()
    {
        ShootBullet();           
    }

    private void ShootBullet()
    {
        Debug.LogWarning(bulletPrefabs.name);
        Bullet bullet = ObjectPoolManager.Instance().GetObject(bulletPrefabs.gameObject).GetComponent<Bullet>();
        bullet.Initialize(shootPoint.position, attackTarget, damage, arrowSpeed);        //arrow换成投射物父类
    }

    public void Initialize(float maxHealth, float damage,float attackRange, float attackCD, float arrowSpeed)   //后续增加建设点时再重构
    {
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.attackRange = attackRange;
        this.attackCD = attackCD;
        this.arrowSpeed = arrowSpeed;
        attackCollider.radius = attackRange;
    }
}
