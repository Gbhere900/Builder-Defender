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
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyUnitHealth))
        {
            attackBuilding.GetAttackTargetList().Add(friendlyUnitHealth.gameObject);
            friendlyUnitHealth.OnDead += OnFriendlyUnitDead;
        }
    }

    protected void OnTriggerExitLogic(Collider other)
    {
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyHealth))
        {
            Debug.Log(friendlyHealth.name);
            attackBuilding.GetAttackTargetList().Remove(friendlyHealth.gameObject);
            friendlyHealth.OnDead -= OnFriendlyUnitDead;
        }
    }


    private void OnFriendlyUnitDead(FriendlyUnitHealth friendUnitHealth)  ////新增兵种时重构
    {
        if (attackBuilding.GetAttackTargetList().Contains(friendUnitHealth.gameObject))
        {
            Debug.Log("HealTower目标的friendUnitHealth死亡，将其从攻击目标列表移除");
            attackBuilding.GetAttackTargetList().Remove(friendUnitHealth.gameObject);
        }
        else
        {
            Debug.LogWarning("friendUnitHealth不在HealTower列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
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

            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //加入飞行单位后重构，近战单位不能攻击到飞行单位也不会索敌到飞行单位
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
        //  Debug.LogWarning(gameObject.name + "未找到目标\n");
        attackBuilding.SetAttackTarget (null);
    }
}
