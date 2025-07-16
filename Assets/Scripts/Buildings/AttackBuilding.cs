using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBuilding : Building
{
    [SerializeField] protected float attackCD;
    [SerializeField] protected bool attackReady = true;
    [SerializeField] protected float originalDamage;
    [SerializeField] protected float damage;

    [SerializeField] protected Transform shootPoint;

    [Header("½Å±¾×é¼þ")]
    [SerializeField] protected CapsuleCollider attackCollider;

    protected List<Enemy> attackTargetList;
    protected Enemy attackTarget;

    
    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }

    private void Update()
    {
        TryAttack();
    }

    private void Awake()
    {
        attackTargetList = new List<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.name);
            attackTargetList.Add(enemy);
        }
            
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.name);
            attackTargetList.Remove(enemy);
        }
            
    }

    private bool TryAttack()
    {
        if (!attackReady)
            return false;
        if(attackTargetList.Count != 0)
        {
            attackTarget = attackTargetList[0];
        }

        Attack();
        attackReady = false;
        StartCoroutine(WaitForAttackCD());
        return true;
    }

    protected abstract void Attack();

}
