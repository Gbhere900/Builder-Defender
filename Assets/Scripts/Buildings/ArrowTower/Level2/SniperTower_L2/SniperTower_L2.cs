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
            if ( Vector3.Distance(transform.position, attackTargetList[i].transform.position) < minDistance)      //加入飞行单位后重构，近战单位不能攻击到飞行单位也不会索敌到飞行单位
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
          Debug.LogWarning(gameObject.name + "未找到目标\n");
        attackBuilding.SetAttackTarget(null);
    }
}
