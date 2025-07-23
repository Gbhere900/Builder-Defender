using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : AttackBuilding
{
    [SerializeField] private Arrow arrowPrefabs;
    [SerializeField] private float arrowSpeed;
    protected override void Attack()
    {
        ShootArrow();
    }

    private void ShootArrow()
    {
        Arrow arrow = ObjectPoolManager.Instance().GetObject(arrowPrefabs.gameObject).GetComponent<Arrow>();
       // Arrow arrow = GameObject.Instantiate(arrowPrefabs,shootPoint);//对象池实现后重构
        arrow.Initialize(shootPoint.position,attackTarget,damage,arrowSpeed);
    }

}
