using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Bullet
{
    [SerializeField] private float boomRadius = 1;
    protected override void OnTriggerEnterLogic(Collider other)
    {
        if (other.isTrigger)
            return;
        if (other.gameObject == attackTarget)
        {
            //attackTarget.GetComponent<Enemy>().ReceiveDamage(damage_Friendly);
            DamageEnemiesInRange();
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
        }

        if ((1 << other.gameObject.layer & goundLayer) != 0)
        {
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
        }
    }

    public void DamageEnemiesInRange()
    {
        // 检测球形范围内的所有碰撞体
        Collider[] collidersInRange = Physics.OverlapSphere(
            transform.position, // 检测中心（当前物体位置）
            boomRadius     // 半径      
        );

        // 遍历结果
        foreach (var collider in collidersInRange)
        {
            if( collider.TryGetComponent<Enemy>(out Enemy targetEnemy))
            {
                targetEnemy.ReceiveDamage(damage_Friendly);
            }

            
        }
    }

    // 在Scene视图中绘制检测范围（辅助调试）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boomRadius);
    }
}
