using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower_L2 : MonoBehaviour
{

    private AttackBuilding attackBuilding;
    private void OnEnable()
    {
        attackBuilding = GetComponent<AttackBuilding>();
        attackBuilding.OnSetAttackTarget += SetClosestGameObjectInDetectRangeAsAimEnemy;
    }

    private void OnDisable()
    {
        attackBuilding.OnSetAttackTarget -= SetClosestGameObjectInDetectRangeAsAimEnemy;
    }

    protected void SetClosestGameObjectInDetectRangeAsAimEnemy()
    {
        float minDistance = float.MaxValue;
        int index = -1;
        List<GameObject> attackTargetList = attackBuilding.GetAttackTargetList();
        for (int i = 0; i < attackTargetList.Count; i++)
        {
           
            if (!(attackTargetList[i].GetComponent<Enemy>().GetEnemyFeature().enemyType == EnemyType.Siege))
                continue;
            if ( Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //������е�λ���ع�����ս��λ���ܹ��������е�λҲ�������е����е�λ
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

        minDistance = float.MaxValue;
        index = -1;

        for (int i = 0; i < attackTargetList.Count; i++)
        {


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
          Debug.LogWarning(gameObject.name + "δ�ҵ�Ŀ��\n");
        attackBuilding.SetAttackTarget(null);
    }
}
