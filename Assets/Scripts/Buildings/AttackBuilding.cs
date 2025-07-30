using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class AttackBuilding : Building
{
    [SerializeField] protected Bullet bulletPrefabs;
    [SerializeField] protected float arrowSpeed;

    [SerializeField] protected float attackCD;
    [SerializeField] protected bool attackReady = true;
    [SerializeField] protected float originalDamage;
    [SerializeField] protected float damage;

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
        TryAttack();
    }

    private void Awake()
    {
        attackTargetList = new List<GameObject>();
    }

    protected virtual void OnTriggerEnter(Collider other)     //��Ѫ����д�������
    {
        OnTriggerEnterLogic(other);     
    }

    protected virtual void OnTriggerEnterLogic(Collider other)
    {
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
            Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            attackTargetList.Remove(enemy.gameObject);
        }
        else
        {
            Debug.LogWarning("���˲���PlayerAttack�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            enemy.OnDead -= OnEnemyDead;
        }

    }

    protected virtual void OnTriggerExit(Collider other)        //��Ѫ����д�������
    {
        OnTriggerEnterLogic(other);
            
    }

    protected virtual void OnTriggerExitLogic(Collider other)
    {
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


    public void LevelUP_L2(int index)
    {
        maxHealth += LevelUPEnhance_L2[index].maxHealthEnhance;
        damage *= (1 + LevelUPEnhance_L2[index].damageEnhance);
        attackCollider.radius *= (1 + LevelUPEnhance_L2[index].attackRangeEnhance);
        attackCD = attackCD / (1 + LevelUPEnhance_L2[index].attackSpeedEnhance);
        arrowSpeed *= (1 + LevelUPEnhance_L2[index].arrowSpeedEnhance);
        Debug.Log("������2����ѡ��Ϊ" + index);
    }

    public void LevelUP_L3(int index)
    {
        maxHealth += LevelUPEnhance_L3[index].maxHealthEnhance;
        damage *= (1 + LevelUPEnhance_L3[index].damageEnhance);
        attackCollider.radius *= (1 + LevelUPEnhance_L3[index].attackRangeEnhance);
        attackCD = attackCD / (1 + LevelUPEnhance_L3[index].attackSpeedEnhance);
        arrowSpeed *= (1 + LevelUPEnhance_L3[index].arrowSpeedEnhance);
        Debug.Log("������3����ѡ��Ϊ" + index);
    }

    protected virtual void Attack()
    {
        ShootBullet();           
    }

    private void ShootBullet()
    {
        Bullet bullet = ObjectPoolManager.Instance().GetObject(bulletPrefabs.gameObject).GetComponent<Bullet>();
        // Arrow arrow = GameObject.Instantiate(arrowPrefabs,shootPoint);//�����ʵ�ֺ��ع�
        bullet.Initialize(shootPoint.position, attackTarget, damage, arrowSpeed);        //arrow����Ͷ���︸��
    }
}
