using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBuilding : Building
{
    
    [SerializeField] protected Bullet bulletPrefabs;
    [SerializeField] protected AttackBuildingAttribute basicAttribute;

    [SerializeField] protected float arrowSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCD;
    [SerializeField] protected bool attackReady = true;
    //[SerializeField] protected float originalDamage;        ����Buffϵͳ���ع�
    [SerializeField] protected Damage damage;

    [SerializeField] protected Transform shootPoint;

    [Header("���������ű����")]
    [SerializeField] protected CapsuleCollider attackCollider;

    protected List<GameObject> attackTargetList;
    protected GameObject attackTarget;

    
    [SerializeField] protected List<UpgradeOptions> upgradeOptions;

    [Serializable]
    public class UpgradeOptions
    {
        public List<UpgradeOption> upgradeOptions;
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

    protected override void Awake()             //�����Awake��Onenabe�ǵ�base
    {   
        base.Awake();
        attackTargetList = new List<GameObject>();
       
    }

    protected override void OnEnable()              //�����Awake��Onenabe�ǵ�base
    {
        base.OnEnable();
        Initialize();             //��ʼ��ʱҲ��Ҫʹ  attackCollider.radius = attackRange;   
    }


    protected void OnTriggerEnter(Collider other)     //��Ѫ����д�������
    {
        OnTriggerEnterLogic(other);     
    }

    protected virtual void OnTriggerEnterLogic(Collider other)
    {
        if(other.isTrigger)         //�ų��Ǵ�������������
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
            //Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            attackTargetList.Remove(enemy.gameObject);
        }
        else
        {
            Debug.LogWarning("���˲���PlayerAttack�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            enemy.OnDead -= OnEnemyDead;
        }

    }

    protected void OnTriggerExit(Collider other)        //��Ѫ����д�������
    {
        OnTriggerExitLogic(other);
            
    }

    protected virtual void OnTriggerExitLogic(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
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
        //if (attackTargetList[0] == null)
        //{
        //    attackTargetList.RemoveAt(0);
        //    return false;
        //}

                 //������attackTarget��Attack;
        AttackAttackTarget();
        attackReady = false;
        StartCoroutine(WaitForAttackCD());
        return true;
    }



    public override void ApplyAllBuildingUpgrades(BuildingData buildingData)
    {
        //�Ȱ���ScriptableObject��ԭһ��ʱ������

        returnToBasicAttributes();
        
     
        //�ٸ��ݽ����������ݻ�ԭ��ǰ�Ǽ�����������
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
        Debug.Log(gameObject.name + "������" + upgradeChoice);
    }

    private void ApplyBuildingUpgradeOption(UpgradeOption upgradeOption)
    {
        maxHealth += upgradeOption.healthBoost_flat;
        damage.damage *= (1 + upgradeOption.damageBoost_percent / 100);
        attackRange *= (1 + upgradeOption.attackRangeBoost_percent / 100);
        attackCollider.radius = attackRange;
        arrowSpeed *= (1 + upgradeOption.arrowSpeedBoost_percent / 100);
        attackCD = attackCD / (1 + upgradeOption.attackSpeedBoost_percent / 100);
    }
    
    protected virtual void AttackAttackTarget()
    {
        ShootBullet();           
    }

    private void ShootBullet()
    {
        Bullet bullet = ObjectPoolManager.Instance().GetObject(bulletPrefabs.gameObject).GetComponent<Bullet>();
        bullet.Initialize(shootPoint.position, attackTarget, damage, arrowSpeed);        //arrow����Ͷ���︸��
    }

    protected virtual void SetClosestGameObjectInDetectRangeAsAimEnemy()       //none���������ò����������ﲢ����
    {

        float minDistance = float.MaxValue;
        int index = -1;
        for (int i = 0; i < attackTargetList.Count; i++)
        {


            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //������е�λ���ع�����ս��λ���ܹ��������е�λҲ�������е����е�λ
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
      //  Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��\n");
        attackTarget = null;
    }


    public void Initialize()        //ÿ��enable���ó�ʼ������ԭԭʼ����
    {
        damage.damageSource = this.gameObject;
        returnToBasicAttributes();
    }

    public void returnToBasicAttributes()
    {
        maxHealth = basicAttribute.basicMaxHealth;
        damage.damage = basicAttribute.basicDamage;
        attackRange = basicAttribute.basicAttackRange;
        attackCollider.radius = attackRange;
        arrowSpeed = basicAttribute.basicArrowSpeed;
        attackCD = basicAttribute.basicAttackCD;
    }
}
