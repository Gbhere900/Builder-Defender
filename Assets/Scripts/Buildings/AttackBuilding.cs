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
    //[SerializeField] protected float originalDamage;        ����Buffϵͳ���ع�
    [SerializeField] protected Damage damage;

    [SerializeField] protected Transform shootPoint;

    [Header("���������ű����")]
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



    public void LevelUP_L2(int choice)
    {
        int index= choice - 1;
        maxHealth += LevelUPEnhance_L2[index].maxHealthEnhance;
        damage.originalDamage *= (1 + LevelUPEnhance_L3[index].damageEnhance / 100);
        damage.damage = damage.originalDamage;
        attackRange *= (1 + LevelUPEnhance_L2[index].attackRangeEnhance/100);
        attackCD = attackCD / (1 + LevelUPEnhance_L2[index].attackSpeedEnhance/100);
        arrowSpeed *= (1 + LevelUPEnhance_L2[index].arrowSpeedEnhance/100);

        AttackBuilding LevelUPBuilding =  GameObject.Instantiate
            (LevelUPEnhance_L2[index].LevelUpBuilding, transform.position, Quaternion.identity);
        LevelUPBuilding.ApplyLevelUP(maxHealth, damage, attackRange,attackCD,arrowSpeed);                   
        Destroy(this.gameObject);                   //

        Debug.Log("������2����ѡ��Ϊ" + index);
    }

    public void LevelUP_L3(int choice)
    {
        int index = choice - 1;
        maxHealth += LevelUPEnhance_L3[index].maxHealthEnhance;
        damage.originalDamage *= (1 + LevelUPEnhance_L3[index].damageEnhance/100);
        damage.damage = damage.originalDamage;                                //�����ع�
        attackRange *= (1 + LevelUPEnhance_L2[index].attackRangeEnhance / 100);
        attackCD = attackCD / (1 + LevelUPEnhance_L3[index].attackSpeedEnhance/100);
        arrowSpeed *= (1 + LevelUPEnhance_L3[index].arrowSpeedEnhance/100);

        AttackBuilding LevelUPBuilding = GameObject.Instantiate
    (LevelUPEnhance_L3[index].LevelUpBuilding, transform.position, Quaternion.identity);
        LevelUPBuilding.ApplyLevelUP(maxHealth, damage, attackRange, attackCD, arrowSpeed);

        Destroy(this.gameObject);
        Debug.Log("������3����ѡ��Ϊ" + index);
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

    public void ApplyLevelUP(float maxHealth, Damage damage_Friendly, float attackRange, float attackCD, float arrowSpeed)   //�������ӽ����ʱ���ع�
    {
        this.maxHealth = maxHealth;
        this.damage = damage_Friendly;
        this.attackRange = attackRange;
        this.attackCD = attackCD;
        this.arrowSpeed = arrowSpeed;
        attackCollider.radius = attackRange;
        health = maxHealth;

        
    }

    public void Initialize()        //�����,��Ϊ��������û��original����
    {
        attackCollider.radius = attackRange;
        damage.damage = damage.originalDamage;
        damage.damageSource = this.gameObject;
    }
}
