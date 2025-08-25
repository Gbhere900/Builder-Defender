using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackBuilding;

public class HealTower_L3 : MonoBehaviour
{
    private AttackBuilding attackBuilding;
    [SerializeField] private HealBullet healBulletPrefab;

    private void OnEnable()
    {
        attackBuilding = GetComponent<AttackBuilding>();
        attackBuilding.SetBulletPrefab(healBulletPrefab);
        attackBuilding.OnAttackTriggerEnter += AttackBuilding_OnAttackTriggerEnter;
        attackBuilding.OnAttackTriggerExit += AttackBuilding_OnAttackTriggerExit;
        attackBuilding.OnSetAttackTarget += AttackBuilding_OnSetAttackTarget;

    }

    private void OnDisable()
    {
        attackBuilding.OnAttackTriggerEnter -= AttackBuilding_OnAttackTriggerEnter;
        attackBuilding.OnAttackTriggerExit -= AttackBuilding_OnAttackTriggerExit;
        attackBuilding.OnSetAttackTarget -= AttackBuilding_OnSetAttackTarget;
    }

    private void AttackBuilding_OnAttackTriggerEnter(Collider other, OnAttackTrigerEnterCallBack RemoveEnemyInAttackTargetList)
    {
        AdjustAttackTargetListOnEnter(other,RemoveEnemyInAttackTargetList);
    }

    private void AttackBuilding_OnAttackTriggerExit(Collider other)
    { 
        AdjustAttackTargetListOnExit(other);
    }

    private void AdjustAttackTargetListOnEnter(Collider other, OnAttackTrigerEnterCallBack RemoveEnemyInAttackTargetList)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            RemoveEnemyInAttackTargetList(enemy);
        }

        OnTriggerEnterLogic(other);
    }

    private void AdjustAttackTargetListOnExit(Collider other)
    {
        OnTriggerExitLogic(other);
    }

    protected void OnTriggerEnterLogic(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyUnitHealth))
        {
            attackBuilding.GetAttackTargetList().Add(friendlyUnitHealth.gameObject);
            friendlyUnitHealth.OnDead += OnFriendlyUnitDead;
        }
    }

    protected void OnTriggerExitLogic(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyHealth))
        {
            Debug.Log(friendlyHealth.name);
            attackBuilding.GetAttackTargetList().Remove(friendlyHealth.gameObject);
            friendlyHealth.OnDead -= OnFriendlyUnitDead;
        }
    }


    private void OnFriendlyUnitDead(FriendlyUnitHealth friendUnitHealth)  ////��������ʱ�ع�
    {
        if (attackBuilding.GetAttackTargetList().Contains(friendUnitHealth.gameObject))
        {
            Debug.Log("HealTowerĿ���friendUnitHealth����������ӹ���Ŀ���б��Ƴ�");
            attackBuilding.GetAttackTargetList().Remove(friendUnitHealth.gameObject);
        }
        else
        {
            Debug.LogWarning("friendUnitHealth����HealTower�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            friendUnitHealth.OnDead -= OnFriendlyUnitDead;
        }

    }



    private void AttackBuilding_OnSetAttackTarget()
    {
        Debug.Log(1);
        SetClosestGameObjectInDetectRangeAsAimEnemy();
    }
    protected  void SetClosestGameObjectInDetectRangeAsAimEnemy()
    {
        float minDistance = float.MaxValue;
        int index = -1;
        List<GameObject> attackTargetList = attackBuilding.GetAttackTargetList();
        Debug.Log(attackTargetList);
        
        for (int i = 0; i <  attackTargetList.Count; i++)
        {
            if ((attackTargetList[i].GetComponent<FriendlyUnitHealth>().IsFullHealth()))
                continue;

            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //������е�λ���ع�����ս��λ���ܹ��������е�λҲ�������е����е�λ
            {
                minDistance = Vector3.Distance(transform.position, attackTargetList[i].transform.position);
                index = i;
            }
        }
        if (index != -1)
        {
            attackBuilding.SetAttackTarget(attackTargetList[index]);

            return;
        }
        //  Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��\n");
        attackBuilding.SetAttackTarget (null);
    }
}
