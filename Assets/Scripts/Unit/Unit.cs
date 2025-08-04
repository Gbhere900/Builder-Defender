using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
[RequireComponent(typeof(FriendlyUnitUI))]
[RequireComponent(typeof(FriendlyObject))]
public class Unit : MonoBehaviour
{
    [Header("��ֵ")]
    [SerializeField] protected bool canFly;
    //[SerializeField] private List<FriendlyUnitType> aimFriendlyUnitType;

    //���Բ��ִ������ع�


    [SerializeField] protected float originalSpeed;
    private float speed;

    [SerializeField] protected float originalDamage;
    [SerializeField] protected float damage;

    [SerializeField] protected float attackCD;

    [SerializeField] protected float attackRadius = 5;

    [SerializeField] private float turnSpeed = 10;

    [SerializeField] public List<Enemy> enemiesInDetectRange;
    [SerializeField] public List<Enemy> enemiesInAttackRange;

    protected Enemy aimEnemy;
    protected bool attackReady = true;




    [Header("�ű����")]
    protected Rigidbody rb;


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

    protected IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Initialize()
    {
        //����buffϵͳ���ع�
        speed = originalSpeed;
        damage = originalDamage;
        rb = GetComponent<Rigidbody>();

        rb = GetComponent<Rigidbody>();
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
        TryRotateToAimFriendlyObject();
    }

    private void TryRotateToAimFriendlyObject()
    {
        if (aimEnemy == null)
            return;
        RotateToAimFriendlyObject();
    }
    private void RotateToAimFriendlyObject()
    {
        if (aimEnemy != null)
        {
            Vector3 targetDirection = aimEnemy.transform.position - transform.position;
            targetDirection.y = 0;

            if (targetDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // ����ʽת��ʹ��Slerpʵ��ƽ������
                Quaternion newRotation = Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    turnSpeed * Time.fixedDeltaTime
                );

                rb.MoveRotation(newRotation);
            }
        }
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
        rb.velocity = Vector3.zero;
        if (aimEnemy == null)
        {
            return false;
        }

       
        if(enemiesInAttackRange.Contains(aimEnemy))
            return false;

        rb.velocity = (aimEnemy.transform.position - transform.position).normalized * speed * Time.deltaTime;
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

