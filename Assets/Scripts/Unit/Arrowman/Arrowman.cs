using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowman : Unit
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float timeToLive;
    [SerializeField] private float height;
    protected override void AttackAimEnemyOrFirstInRange()
    {

        attackReady = false;
        StartCoroutine(WaitForAttackCD());

        Arrow arrow = ObjectPoolManager.Instance().GetObject(arrowPrefab).GetComponent<Arrow>();
        
        for (int i = 0; i < enemiesInAttackRange.Count; i++)
        {
            if (enemiesInAttackRange[i] == aimEnemy)
            {
                arrow.Initialize(shootPoint.position, aimEnemy.gameObject, damage_Friendly, arrowSpeed, timeToLive, height);
                return;
            }
        }

        arrow.Initialize(shootPoint.position, enemiesInAttackRange[0].gameObject, damage_Friendly, arrowSpeed, timeToLive, height);
    }
}
