using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackBuilding;

public abstract class AttackBuilding : Building
{
    
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected AttackBuildingAttribute basicAttribute;

    [SerializeField] protected float arrowSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCD;
    [SerializeField] protected bool attackReady = true;
    //[SerializeField] protected float originalDamage;        加入Buff系统后重构
    [SerializeField] protected Damage damage;

    [SerializeField] protected Transform shootPoint;

    [Header("攻击建筑脚本组件")]
    [SerializeField] protected CapsuleCollider attackCollider;

    protected List<GameObject> attackTargetList;
    protected GameObject attackTarget;

    
    [SerializeField] protected List<UpgradeOptions> upgradeOptions;

    public Action OnSetAttackTarget;
    public Action OnShootbullet;

    [Serializable]
    public class UpgradeOptions
    {
        public List<UpgradeOption> upgradeOptions;
    }

    protected override void Awake()             //后面的Awake和Onenabe记得base
    {   
        base.Awake();
        attackTargetList = new List<GameObject>();
       
    }

    protected override void OnEnable()              //后面的Awake和Onenabe记得base
    {
        base.OnEnable();
        Initialize();             //初始化时也需要使  attackCollider.radius = attackRange;   
    }


    protected IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Update()
    {
        SetClosestGameObjectInDetectRangeAsAimEnemy();
        TryAttack();
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
            //Debug.Log("PlayerAttack目标的Enemy死亡，将其从攻击目标列表移除");
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

        AttackAttackTarget();
        attackReady = false;
        StartCoroutine(WaitForAttackCD());
        return true;
    }



    public override void ApplyAllBuildingUpgrades(BuildingData buildingData)
    {
        //先按照ScriptableObject复原一级时的属性

        returnToBasicAttributes();
        
     
        //再根据建筑升级数据还原当前登记下完整属性
        for(int i= 0;i<buildingData.currentLevel - 1;i++)
        {
            int choice = buildingData.upgradeChoices[i].choice;
            ApplyBuildingUpgradeOption(upgradeOptions[i].upgradeOptions[choice - 1]);
        }

        
    }

    public void ApplyBuidingUpgradeChoice(UpgradeChoice upgradeChoice)
    {
        int levelFrom = upgradeChoice.levelFrom;
        int choice = upgradeChoice.choice;
        ApplyBuildingUpgradeOption(upgradeOptions[levelFrom - 1].upgradeOptions[choice]);
        Debug.Log(gameObject.name + "已升级" + upgradeChoice);
    }

    private void ApplyBuildingUpgradeOption(UpgradeOption upgradeOption)
    {
        maxHealth += upgradeOption.healthBoost_flat;
        damage.damage *= (1 + upgradeOption.damageBoost_percent / 100);
        attackRange *= (1 + upgradeOption.attackRangeBoost_percent / 100);
        attackCollider.radius = attackRange;
        arrowSpeed *= (1 + upgradeOption.arrowSpeedBoost_percent / 100);
        attackCD *= (1 + upgradeOption.attackCDBoost_percent / 100);
    }
    
    protected virtual void AttackAttackTarget()
    {
        ShootBullet();           
    }

    private void ShootBullet()
    {
        Bullet bullet = ObjectPoolManager.Instance().GetObject(bulletPrefab.gameObject).GetComponent<Bullet>();
        bullet.Initialize(shootPoint.position, attackTarget, damage, arrowSpeed);
        OnShootbullet?.Invoke();
    }

    protected virtual void SetClosestGameObjectInDetectRangeAsAimEnemy()       //none参数可能用不到，在这里并不是
    {

        float minDistance = float.MaxValue;
        int index = -1;
        for (int i = 0; i < attackTargetList.Count; i++)
        {


            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //加入飞行单位后重构，近战单位不能攻击到飞行单位也不会索敌到飞行单位
            {
                minDistance = Vector3.Distance(transform.position, attackTargetList[i].transform.position);
                index = i;
            }
        }
        if (index != -1)
        {
            attackTarget = attackTargetList[index];

            return;
        }
      //  Debug.LogWarning(gameObject.name + "未找到目标\n");
        attackTarget = null;

        OnSetAttackTarget?.Invoke();         //可以触发狙击塔的重索敌；
    }


    public void Initialize()        //每次enable调用初始化，还原原始属性
    {
        damage.damageSource = this.gameObject;
        returnToBasicAttributes();
    }

    public void returnToBasicAttributes()       //经济类建筑要另外写还原属性的函数
    {
        maxHealth = basicAttribute.basicMaxHealth;
        health = maxHealth;
        damage.damage = basicAttribute.basicDamage;
        attackRange = basicAttribute.basicAttackRange;
        attackCollider.radius = attackRange;
        arrowSpeed = basicAttribute.basicArrowSpeed;
        attackCD = basicAttribute.basicAttackCD;
    }

    public List<GameObject> GetAttackTargetList()
    {
        return attackTargetList;
    }

    public void SetAttackTarget(GameObject attackTaret)
    {
        this.attackTarget = attackTaret;
    }

    public void BoostAttackCD(float attackCDBoost_Percent)
    {
        attackCD *= (1 + attackCDBoost_Percent / 100);
    }

    public float GetAttackCD()
    {
         return attackCD; 
    }

    public void SetAttackCD(float attackCD)
    {
        this.attackCD = attackCD;
    }

    public void SetBulletPrefab(Bullet bullet)
    {
        bulletPrefab = bullet;
    }
    

}
