using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("数值")]
    [SerializeField] private bool canFly;
    //[SerializeField] private List<FriendlyUnitType> aimFriendlyUnitType;

    //属性部分待后面重构


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


    [Header("脚本组件")]
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
        //增加buff系统后重构
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



    private void SetClosestEnemyInDetectRangeAsAimEnemy()       //none参数可能用不到，在这里并不是
    {

        float minDistance = float.MaxValue;
        int index = -1;
        for (int i = 0; i < enemiesInDetectRange.Count; i++)
        {

           
            if (Vector3.Distance(transform.position, enemiesInDetectRange[i].transform.position) < minDistance)      //加入飞行单位后重构，近战单位不能攻击到飞行单位也不会索敌到飞行单位
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
        Debug.LogWarning(gameObject.name + "未找到目标\n" );
        aimEnemy = null;
    }  


    private bool TryMove()              //可能有bug，因为移动到攻击范围后只有很小的窗口时间给攻击
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
        Debug.Log(gameObject.name + "攻击" + enemiesInAttackRange[0].name);
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

