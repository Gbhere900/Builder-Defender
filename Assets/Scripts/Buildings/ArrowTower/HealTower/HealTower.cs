using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : AttackBuilding
{
    protected override void OnTriggerEnterLogic(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyUnitHealth))
        {
            attackTargetList.Add(friendlyUnitHealth.gameObject);
            friendlyUnitHealth.OnDead += OnFriendlyUnitDead;
        }
    }

    protected override void OnTriggerExitLogic(Collider other)
    {
        if (other.isTrigger)         //�ų��Ǵ�������������
            return;
        if (other.gameObject.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyHealth))
        {
            Debug.Log(friendlyHealth.name);
            attackTargetList.Remove(friendlyHealth.gameObject);
            friendlyHealth.OnDead -= OnFriendlyUnitDead;
        }
    }


    private void OnFriendlyUnitDead(FriendlyUnitHealth friendUnitHealth)  ////��������ʱ�ع�
    {
        if (attackTargetList.Contains(friendUnitHealth.gameObject))
        {
            Debug.Log("HealTowerĿ���friendUnitHealth����������ӹ���Ŀ���б��Ƴ�");
            attackTargetList.Remove(friendUnitHealth.gameObject);
        }
        else
        {
            Debug.LogWarning("friendUnitHealth����HealTower�б���ʱ�����¼�������˵���������뿪��ҹ�����Χ��δȡ�������¼�������ִ��ȡ������");
            friendUnitHealth.OnDead -= OnFriendlyUnitDead;
        }

    }

    protected override bool TryAttack()
    {
        if (!attackReady)
            return false;
        if (attackTargetList.Count == 0)
        {
            return false;

        }
        if (attackTargetList[0] == null)
        {
            attackTargetList.RemoveAt(0);
            return false;
        }

        attackTarget = attackTargetList[0];
        AttackAttackTarget();
        attackReady = false;
        StartCoroutine(WaitForAttackCD());
        return true;
    }
}
