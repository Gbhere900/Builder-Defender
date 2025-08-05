using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

[Serializable]
public class FriendlyUnitTypes
{
    public List<FriendlyUnitType> friendUnitTypeList;
}

public class Enemy : MonoBehaviour
{
    [Header("��ֵ")] 
    [SerializeField] protected bool canFly;
    [SerializeField] public List<FriendlyUnitTypes> aimFriendlyUnitTypesList;

[SerializeField] protected float originalMaxHealth;
    [SerializeField] protected float MaxHealth;
    protected float health;

    [SerializeField] protected float originalSpeed;
    protected float speed;

    [SerializeField] protected float originalDamageToUnit;
    protected float damageToUnit;

    [SerializeField] protected float originalDamageToPlayer;
    protected float damageToPlayer;

    [SerializeField] protected float originalDamageToBuilding;
    protected float damageToBuilding;

    [SerializeField] protected float originalDamageToHero;
    protected float damageToHero;

    [SerializeField] protected float attackCD;

    protected FriendlyObject aimFriendlyObject;
    protected bool attackReady = true;

    [SerializeField] protected float turnSpeed = 10;

    //  protected bool isSlow = false;
    protected Coroutine slowCoroutine;


    [SerializeField] protected List<FriendlyObject> FriendlyObjectInAttackRange;


    [Header("�ű����")]
    protected Rigidbody rb;

    //�¼�
    public Action OnHealthChanged;
    public Action<Enemy> OnDead;


    private void Awake()
    {
        FriendlyObjectInAttackRange = new List<FriendlyObject>();
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
        TryRotateToAimFriendlyObject();
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

        if (other.gameObject.TryGetComponent<FriendlyObject>(out FriendlyObject friendlyObject))
        {
            Debug.LogWarning(other.gameObject.name + "���뷶Χ");
            FriendlyObjectInAttackRange.Add(friendlyObject);
            friendlyObject.OnDestroyed += OnFriendlyUnitDead;
        }
    }

    private void OnFriendlyUnitDead(FriendlyObject friendlyOBject)
    {
        if (FriendlyObjectInAttackRange.Contains(friendlyOBject))
        {
            Debug.Log("EnemyĿ���FriendlyObject����������ӹ���Ŀ���б��Ƴ�");
            FriendlyObjectInAttackRange.Remove(friendlyOBject);
        }
        else
        {
            Debug.LogWarning("�ѷ���λ����Enemy�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            friendlyOBject.OnDestroyed -= OnFriendlyUnitDead;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.TryGetComponent<FriendlyObject>(out FriendlyObject friendlyObject))
        {
            FriendlyObjectInAttackRange.Remove(friendlyObject);
            friendlyObject.OnDestroyed -= OnFriendlyUnitDead;
        }
    }

    private void TryMove()
    {
        rb.velocity = Vector3.zero;
        if (aimFriendlyObject == null)
            return;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyObject))
        {
            Debug.Log(gameObject.name + "�Ĺ���Ŀ���ڹ�����Χ�У�ֹͣ�ƶ�");
            return;
        }
            
        Move();
    }

    private void TryRotateToAimFriendlyObject()
    {
        if (aimFriendlyObject == null)
            return;
        RotateToAimFriendlyObject();
    }
    private void RotateToAimFriendlyObject()
    {
        if (aimFriendlyObject != null)
        {
            Vector3 targetDirection = aimFriendlyObject.transform.position - transform.position;
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
    private void Move()
    {
        Vector3 direction = (aimFriendlyObject.transform.position - transform.position).normalized;
        rb.velocity = direction * Time.deltaTime * speed;
    }
    
    private void AttackAimFriendlyUnitOrFirstInRange()
    {
        attackReady = false;
        FriendlyObject tempFriendlyObject;
        if (FriendlyObjectInAttackRange.Contains(aimFriendlyObject))
        {
            tempFriendlyObject = aimFriendlyObject;
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


    public virtual void ReceiveDamage(Damage_Friendly damage_Friendly)  //�������������д�������,��������Ƿ�Ϊ��ս�˺����¼���damage������damageSource������ж��Ƿ��ж����˺�
    {
        float damage = damage_Friendly.damage;             
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
        damageToHero = originalDamageToHero;

        rb = GetComponent<Rigidbody>();
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

    //private void SetAimFriendlyUnit()
    //{
    //    foreach(FriendlyUnitType friendlyUnitType in aimFriendlyUnitType)
    //    {
    //        float minDistance = float.MaxValue;
    //        int index = -1;
    //        List<FriendlyObject> friendlyObjectList = FriendlyOBjectManager.Instance().GetFriendlyObjetcList();
    //        for (int i = 0; i <friendlyObjectList.Count;i++)
    //        {
    //            if (friendlyObjectList[i].GetFriendlyUnitType() != friendlyUnitType)
    //            {
    //                continue;
    //            }

    //            //Debug.Log("���˾���" + friendlyObjectList[i].name +"�ľ���Ϊ" + Vector3.Distance(transform.position, friendlyObjectList[i].transform.position));
    //            if (Vector3.Distance(transform.position, friendlyObjectList[i].transform.position) < minDistance)
    //            {
    //                minDistance = Vector3.Distance(transform.position, friendlyObjectList[i].transform.position);
    //                index = i;
    //            }
    //        }
    //        if(index != -1)
    //        {
    //            aimFriendlyObject = friendlyObjectList[index];
    //            Debug.Log(gameObject.name + "���ҵ�Ŀ��\n"+ aimFriendlyObject);
    //            return;
    //        }
    //    }

    //    aimFriendlyObject = null;
    //    Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��");
        
    //}

    private void SetAimFriendlyUnit()
    {
        aimFriendlyObject = null;
        foreach (FriendlyUnitTypes friendlyUnitTypes in aimFriendlyUnitTypesList)
        {
            float minDistance = float.MaxValue;
            foreach(FriendlyUnitType friendlyUnitType in friendlyUnitTypes.friendUnitTypeList)
            {
                List<FriendlyObject> friendlyObjectList = FriendlyOBjectManager.Instance().GetFriendlyObjetcList();
                for (int i = 0; i < friendlyObjectList.Count; i++)
                {
                    if (friendlyObjectList[i].GetFriendlyUnitType() != friendlyUnitType)
                    {
                        continue;
                    }

                    //Debug.Log("���˾���" + friendlyObjectList[i].name +"�ľ���Ϊ" + Vector3.Distance(transform.position, friendlyObjectList[i].transform.position));
                    if (Vector3.Distance(transform.position, friendlyObjectList[i].transform.position) < minDistance)
                    {
                        minDistance = Vector3.Distance(transform.position, friendlyObjectList[i].transform.position);
                        aimFriendlyObject = friendlyObjectList[i];
                    }
                }
            }
            if (aimFriendlyObject != null)
            {
                Debug.Log(gameObject.name + "���ҵ�Ŀ��\n" + aimFriendlyObject);
                return;
            }

        }

        
        Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��");

    }

    public bool TrySlowSpeeddForSeconds(float seconds, float percent)
    {
        if(percent < speed / originalSpeed * 100)
        {
            ChangeSpeedForSeconds(seconds,percent);
            return true;
        }

        return false;

    }
    public void ChangeSpeedForSeconds(float seconds, float percent)
    {
        Debug.Log("�Ѹı��ٶ�");
        speed =originalSpeed * percent/100;                 //������������ع��ø�����չ�ԣ�Ŀǰֻ֧����originalSpeed�����Ͻ��мӼ��٣��޷�������ּӼ���Ч����ͬӰ������
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
            slowCoroutine = StartCoroutine(WaitForSpeedRecover(seconds));
    }

IEnumerator WaitForSpeedRecover(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        speed = originalSpeed;                      //���buildϵͳ���ع�
        Debug.Log("�ٶȻָ�");
    }
}
