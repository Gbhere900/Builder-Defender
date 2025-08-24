using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : AttackBuilding
{
    protected override void OnTriggerEnterLogic(Collider other)
    {
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyUnitHealth))
        {
            attackTargetList.Add(friendlyUnitHealth.gameObject);
            friendlyUnitHealth.OnDead += OnFriendlyUnitDead;
        }
    }

    protected override void OnTriggerExitLogic(Collider other)
    {
        if (other.isTrigger)         //排除是触发器进入的情况
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyHealth))
        {
            Debug.Log(friendlyHealth.name);
            attackTargetList.Remove(friendlyHealth.gameObject);
            friendlyHealth.OnDead -= OnFriendlyUnitDead;
        }
    }


    private void OnFriendlyUnitDead(FriendlyUnitHealth friendUnitHealth)  ////新增兵种时重构
    {
        if (attackTargetList.Contains(friendUnitHealth.gameObject))
        {
            Debug.Log("HealTower目标的friendUnitHealth死亡，将其从攻击目标列表移除");
            attackTargetList.Remove(friendUnitHealth.gameObject);
        }
        else
        {
            Debug.LogWarning("friendUnitHealth不在HealTower列表中时死亡事件触发，说明敌人在离开玩家攻击范围后未取消订阅事件，现在执行取消订阅");
            friendUnitHealth.OnDead -= OnFriendlyUnitDead;
        }

    }

    protected override bool TryAttack()
    {
        if (!attackReady)
            return false;
        if (attackTarget == null)
        {
            return false;

        }
        //if (attackTargetList[0] == null)
        //{
        //    attackTargetList.RemoveAt(0);
        //    return false;
        //}

        
        AttackAttackTarget();
        attackReady = false;
        StartCoroutine(WaitForAttackCD());
        return true;
    }

    protected override void SetClosestGameObjectInDetectRangeAsAimEnemy()
    {
        float minDistance = float.MaxValue;
        int index = -1;
        for (int i = 0; i < attackTargetList.Count; i++)
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
            attackTarget = attackTargetList[index];

            return;
        }
        //  Debug.LogWarning(gameObject.name + "未找到目标\n");
        attackTarget = null;
    }
}
