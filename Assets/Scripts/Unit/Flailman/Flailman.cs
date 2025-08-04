using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flailman : Unit
{
    protected override void TryAttack()
    {
        if (!attackReady)
            return;
        if (enemiesInAttackRange.Count > 0)
            AttackAllEnemiesInAttackRange();
    }

    private void AttackAllEnemiesInAttackRange()
    {
        attackReady = false;
        StartCoroutine(WaitForAttackCD());

        for (int i = 0; i < enemiesInAttackRange.Count; i++)
        {
            enemiesInAttackRange[i].ReceiveDamage(damage);
        }
    }
}
