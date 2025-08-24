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

            if (Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //������е�λ���ع�����ս��λ���ܹ��������е�λҲ�������е����е�λ
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
        //  Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��\n");
        attackTarget = null;
    }
}
