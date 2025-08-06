using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Unit
{
    [SerializeField] private float reduceSpeedTime = 1.0f;
    [SerializeField] private float speedPercent = 50;
    protected override void AttackAimEnemyOrFirstInRange()
    {
       // Debug.Log(gameObject.name + "¹¥»÷" + enemiesInAttackRange[0].name);
        attackReady = false;
        StartCoroutine(WaitForAttackCD());

        for (int i = 0; i < enemiesInAttackRange.Count; i++)
        {
            if (enemiesInAttackRange[i] == aimEnemy)
            {
                aimEnemy.ReceiveDamage(damage_Friendly);
                aimEnemy.ChangeSpeedForSeconds(reduceSpeedTime,speedPercent);
                return;
            }
        }

        enemiesInAttackRange[0].ReceiveDamage(damage_Friendly);
        aimEnemy.ChangeSpeedForSeconds(reduceSpeedTime, speedPercent);
    }


}
