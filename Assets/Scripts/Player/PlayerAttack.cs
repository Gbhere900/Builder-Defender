using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


//��Ҵ����BUFF���֣���ɺ�ǵü���Initialize��������
public class PlayerAttack : MonoBehaviour
{
    private Enemy attackTarget;
    private List<Enemy> attackTargetList;
    private int attackIndex = 0;                        //��index��ListRemove����ܻ���BUg
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Arrow arrowPrefab;

    [SerializeField] private Damage damage_Friendly;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowTimeToLive;
    [SerializeField] private float attackCD;
    private bool canAttack = true;

    private void OnEnable()
    {
        attackTargetList = new List<Enemy>();
        Initialize();
    }
    private void Attack()
    {
        Arrow arrow = ObjectPoolManager.Instance().GetObject(arrowPrefab.gameObject).GetComponent<Arrow>();
        // Arrow arrow = GameObject.Instantiate(arrowPrefabs,shootPoint);//�����ʵ�ֺ��ع�
        arrow.Initialize(shootPoint.position, attackTarget.gameObject, damage_Friendly, arrowSpeed,arrowTimeToLive);

        canAttack = false;
        StartCoroutine(WaitForAttackCD());
    }

    public void SetNextAttackTarget()
    {
        if(attackTargetList.Count == 0)
        {
            Debug.LogWarning("����Ŀ���б�Ϊ�գ��޷�������һ������Ŀ��");
            attackTarget = null;
            attackIndex = -1;
            return;
        }

        attackIndex++;
        if(attackIndex >= attackTargetList.Count)
        {
            attackIndex = 0;
        }

        SetEnemyAtIndexAsAttackTarget(attackIndex);
        
    }
    //��ҵ������߼��ǣ�����һ�����˽���ʱֱ���������������л���������ĿǰĿ���������뿪������Χʱ�л������������
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
           // Debug.Log(enemy.gameObject.name + "������ҹ�����Χ");
            attackTargetList.Add(enemy);
            enemy.OnDead += OnEnemyDead;

            if(attackTargetList.Count == 1 )
            {
                attackIndex = 0;
                SetEnemyAtIndexAsAttackTarget(attackIndex);
            }
        }
    }

    private void OnEnemyDead(Enemy enemy)
    {
        if(attackTargetList.Contains(enemy))
        {
          //  Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            attackTargetList.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("���˲���PlayerAttack�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            enemy.OnDead -= OnEnemyDead;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
            return;
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.gameObject.name + "�뿪��Χ");
            attackTargetList.Remove(enemy);
            enemy.OnDead -=  OnEnemyDead;
            if(attackTarget = enemy)
            {
               // Debug.Log("��ǰ����Ŀ���뿪������Χ");
                SetClosestAttackTargetAsAttackTarget(enemy);
                
            }
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetNextAttackTarget();
        }

        if(canAttack && attackTarget != null)
        {
            if(attackTarget == null )
            {

            }
            Attack();
        }
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack  = true;
    }

    private void SetClosestAttackTargetAsAttackTarget(Enemy enemy)   //�����Enemy�β�û��
    {
        if(attackTargetList.Count == 0)
        {
         //   Debug.Log("������Χ�����޵��ˣ������������Ŀ��Ϊ��");
            attackTarget.OnDead -= SetClosestAttackTargetAsAttackTarget;
            attackTarget = null;
            attackIndex = -1;
            return;
        }
            
        float minDistance = float.MaxValue;
        for(int i = 0;i<attackTargetList.Count;i++)
        {

            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) <minDistance)
            {
                attackIndex = i;
                minDistance = Vector3.Distance(transform.position, attackTargetList[i].transform.position);
            }
        }

        SetEnemyAtIndexAsAttackTarget(attackIndex);
       
    }

    private void SetEnemyAtIndexAsAttackTarget(int Index)   //��������attackTarget���߼���Ҫ��������
    {
        if (Index >= attackTargetList.Count)
        {
            Debug.LogWarning("Ŀ�������������List��������ɫ���ù���Ŀ��ʧ��");
            attackTarget = null;
            attackIndex = -1;
            return;
            
        }
        if(attackTarget != null)
        {
            attackTarget.OnDead -= SetClosestAttackTargetAsAttackTarget;
        }

        attackTarget = attackTargetList[attackIndex];
        attackTarget.OnDead +=  SetClosestAttackTargetAsAttackTarget;   //����Ҫ�ع����װһ��
    }

    private void Initialize()
    {
        damage_Friendly.damage = damage_Friendly.originalDamage;
        damage_Friendly.damageSource = this.gameObject;
    }
    
}
