using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Enemy : MonoBehaviour
{
    [Header("��ֵ")]
    [SerializeField] private bool canFly;
    [SerializeField] private List<FriendlyUnitType> aimFriendlyUnitType;

    [SerializeField] private float originalMaxHealth;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float health;

    [SerializeField] private float originalSpeed;
    private float speed;

    [SerializeField] private float originalDamageToUnit;
    private float damageToUnit;

    [SerializeField] private float originalDamageToPlayer;
    private float damageToPlayer;

    [SerializeField] private float originalDamageToBuilding;
    private float damageToBuilding;

    [SerializeField] private float originalDamageToHero;
    private float damageToHero;

    [SerializeField] private float attackCD;

    private FriendlyOBject aimFriendlyUnit;
    private bool attackReady = true;


    [SerializeField]private List<FriendlyOBject> FriendlyObjectInAttackRange;

    [Header("�ű����")]
    [SerializeField] private Rigidbody rigidbody;

    //�¼�
    public Action OnHealthChanged;
    public Action<Enemy> OnDead;


    private void Awake()
    {
        FriendlyObjectInAttackRange = new List<FriendlyOBject>();
    }

    private void OnEnable()
    {
        Initialize();
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Update()
    {
        SetAimFriendlyUnit();
        TryAttack();
    }

    private void TryAttack()
    {
        if (!attackReady) return;
        if (FriendlyObjectInAttackRange.Count == 0)
            return;

        AttackAimFriendlyUnitOrFirstInRange();
    }
    private void FixedUpdate()
    {
        TryMove();
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (!attackReady) return;
    //    if (collision.gameObject.GetComponent<FriendlyUnitHealth>())       //�ȴ��ع�
    //        Attack();

    //}



    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.TryGetComponent<FriendlyOBject>(out FriendlyOBject friendlyObject))
        {
            FriendlyObjectInAttackRange.Add(friendlyObject);
            friendlyObject.OnDestroyed += OnFriendlyUnitDead;
        }
    }

    private void OnFriendlyUnitDead(FriendlyOBject friendlyOBject)
    {
        if (FriendlyObjectInAttackRange.Contains(friendlyOBject))
        {
            Debug.Log("PlayerAttackĿ���Enemy����������ӹ���Ŀ���б��Ƴ�");
            FriendlyObjectInAttackRange.Remove(friendlyOBject);
        }
        else
        {
            Debug.LogWarning("���˲���PlayerAttack�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            friendlyOBject.OnDestroyed -= OnFriendlyUnitDead;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.TryGetComponent<FriendlyOBject>(out FriendlyOBject friendlyObject))
        {
            FriendlyObjectInAttackRange.Remove(friendlyObject);
            friendlyObject.OnDestroyed -= OnFriendlyUnitDead;
        }
    }

    private void TryMove()
    {
        rigidbody.velocity = Vector3.zero;
        if (aimFriendlyUnit == null)
            return;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyUnit))
            return;
        Move();
    }
    private void Move()
    {
        Vector3 direction = (aimFriendlyUnit.transform.position - transform.position).normalized;
        rigidbody.velocity = direction * Time.deltaTime * speed;
    }
    
    private void AttackAimFriendlyUnitOrFirstInRange()
    {
        attackReady = false;
        FriendlyOBject tempFriendlyObject;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyUnit))
        {
            tempFriendlyObject = aimFriendlyUnit;
        }
        else
        {
            tempFriendlyObject = FriendlyObjectInAttackRange[0];
        }
        switch (tempFriendlyObject.GetFriendlyUnitType())
        {

            case (FriendlyUnitType.Unit):
                tempFriendlyObject.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToUnit);
                break;
            case (FriendlyUnitType.Player):
                tempFriendlyObject.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToPlayer);
                break;
            case (FriendlyUnitType.Hero):
                tempFriendlyObject.GetComponent<FriendlyUnitHealth>().ReceiveDamage(damageToHero);
                break;
            case (FriendlyUnitType.Building):
                tempFriendlyObject.GetComponent<Building>().ReceiveDamage(damageToBuilding);
                break;
        }

        StartCoroutine(WaitForAttackCD());
    }


    public void ReceiveDamage(float damage)
    {
        health -= Mathf.Min(health, damage);
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            Die();
        }
        

    }


    public void Die()
    {
        ObjectPoolManager.Instance().ReleaseObject(this.gameObject);  
        OnDead?.Invoke(this);
    }
    private void Initialize()
    {
        //����buffϵͳ���ع�
        MaxHealth = originalMaxHealth;
        health = MaxHealth;
        speed = originalSpeed;
        damageToUnit = originalDamageToUnit;
        damageToPlayer = originalDamageToPlayer;
        damageToBuilding = originalDamageToBuilding;

        rigidbody = GetComponent<Rigidbody>();
        FriendlyObjectInAttackRange.Clear();
    }

    public float GetHealth()
    {
        return health; 
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    private void SetAimFriendlyUnit()
    {
        foreach(FriendlyUnitType friendlyUnitType in aimFriendlyUnitType)
        {
            float minDistance = float.MaxValue;
            int index = -1;
            List<FriendlyOBject> friendlyObjectList = FriendlyOBjectManager.Instance().GetFriendlyObjetcList();
            for (int i = 0; i <friendlyObjectList.Count;i++)
            {
                if (friendlyObjectList[i].GetFriendlyUnitType() != friendlyUnitType)
                {
                    continue;
                }

                //Debug.Log("���˾���" + friendlyObjectList[i].name +"�ľ���Ϊ" + Vector3.Distance(transform.position, friendlyObjectList[i].transform.position));
                if (Vector3.Distance(transform.position, friendlyObjectList[i].transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, friendlyObjectList[i].transform.position);
                    index = i;
                }
            }
            if(index != -1)
            {
                aimFriendlyUnit = friendlyObjectList[index];
                Debug.Log(gameObject.name + "���ҵ�Ŀ��\n"+ aimFriendlyUnit.gameObject.name);
                return;
            }
        }

        aimFriendlyUnit = null;
        Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��");
        
    }


}
