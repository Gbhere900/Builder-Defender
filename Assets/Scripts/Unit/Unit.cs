using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("��ֵ")]
    [SerializeField] private bool canFly;
    //[SerializeField] private List<FriendlyUnitType> aimFriendlyUnitType;

    //���Բ��ִ������ع�


    [SerializeField] private float originalSpeed;
    private float speed;

    [SerializeField] private float originalDamage;
    [SerializeField] private float damage;

    [SerializeField] private float attackCD;

    [SerializeField] private float attackRadius = 5;

    [SerializeField] public List<Enemy> enemiesInDetectRange;
    [SerializeField] public List<Enemy> enemiesInAttackRange;

    private Enemy aimEnemy;
    private bool attackReady = true;


    [Header("�ű����")]
    private Rigidbody rigidbody;


    private void Awake()
    {
        enemiesInDetectRange = new List<Enemy>();
        enemiesInAttackRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        Initialize();
        //SetAimEnemy();
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Initialize()
    {
        //����buffϵͳ���ع�
        speed = originalSpeed;
        damage = originalDamage;
        rigidbody = GetComponent<Rigidbody>();

        rigidbody = GetComponent<Rigidbody>();
        enemiesInDetectRange.Clear();
        enemiesInAttackRange.Clear();

    }



    private void Update()
    {
        SetClosestEnemyInDetectRangeAsAimEnemy();
        TryAttack();
    }
    private void FixedUpdate()
    {
        TryMove();
        
    }



    private void SetClosestEnemyInDetectRangeAsAimEnemy()       //none���������ò����������ﲢ����
    {

        float minDistance = float.MaxValue;
        int index = -1;
        for (int i = 0; i < enemiesInDetectRange.Count; i++)
        {

           
            if (Vector3.Distance(transform.position, enemiesInDetectRange[i].transform.position) < minDistance)      //������е�λ���ع�����ս��λ���ܹ��������е�λҲ�������е����е�λ
            {
                minDistance = Vector3.Distance(transform.position, enemiesInDetectRange[i].transform.position);
                index = i;
            }
        }
        if (index != -1)
        {
            aimEnemy = enemiesInDetectRange[index];
            
            return;
        }
        Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��\n" );
        aimEnemy = null;
    }  


    private bool TryMove()              //������bug����Ϊ�ƶ���������Χ��ֻ�к�С�Ĵ���ʱ�������
    {
        rigidbody.velocity = Vector3.zero;
        if (aimEnemy == null)
        {
            return false;
        }

        transform.forward = aimEnemy.transform.position - transform.position;

        if(enemiesInAttackRange.Contains(aimEnemy))
            return false;

        rigidbody.velocity = (aimEnemy.transform.position - transform.position).normalized * speed * Time.deltaTime;
        return true;       

    }

    protected virtual void TryAttack()
    {
        if (!attackReady)
            return;
        if (enemiesInAttackRange.Count > 0)
            AttackAimEnemyOrFirstInRange();
    }

    protected virtual void AttackAimEnemyOrFirstInRange()
    {
        Debug.Log(gameObject.name + "����" + enemiesInAttackRange[0].name);
        attackReady = false;
        StartCoroutine(WaitForAttackCD());

        for(int i = 0;i<enemiesInAttackRange.Count;i++)
        {
            if (enemiesInAttackRange[i] == aimEnemy)
            {
                aimEnemy.ReceiveDamage(damage);
                return ;
            }
        }

        enemiesInAttackRange[0].ReceiveDamage(damage);
    }
}

