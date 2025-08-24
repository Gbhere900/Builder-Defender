using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortTower_L2 : MonoBehaviour
{
    private AttackBuilding attackbuilding;

    private float originalAttackCD = 0;
    private float currentAttackCDBoost = 0;
    [SerializeField] private float attackCDBoostEachAttack = -5;
    [SerializeField] private float maxAttackCDBoost = -70;
    [SerializeField] private float timeForAttackCDRecover;
    private Coroutine currentRecoveryCoroutine;
    private void OnEnable()
    {
        attackbuilding = GetComponent<AttackBuilding>();
        attackbuilding.OnShootbullet += BoostAttackCD;
        originalAttackCD = attackbuilding.GetAttackCD();
       
    }

    private void OnDisable()
    {
        attackbuilding.OnShootbullet -= BoostAttackCD;
    }

    public void  BoostAttackCD()
    {
        if (currentAttackCDBoost == 0)          //如果为0代表此时防御塔攻速为正常攻速，此时设置初始攻击CD;
            originalAttackCD = attackbuilding.GetAttackCD();

        attackbuilding.SetAttackCD(originalAttackCD);
        currentAttackCDBoost = Math.Max(currentAttackCDBoost + attackCDBoostEachAttack, maxAttackCDBoost);
        attackbuilding.BoostAttackCD(currentAttackCDBoost);

        if(currentRecoveryCoroutine != null)
            StopCoroutine(currentRecoveryCoroutine);
        currentRecoveryCoroutine =  StartCoroutine(WaitForAttackCDRecover());
    }

   IEnumerator WaitForAttackCDRecover()
    {
        yield return new WaitForSeconds(timeForAttackCDRecover);
        currentAttackCDBoost = 0;
        attackbuilding.SetAttackCD(originalAttackCD);
    }


}
