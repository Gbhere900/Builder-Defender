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
        // ������η�Χ�ڵ�������ײ��
        Collider[] collidersInRange = Physics.OverlapSphere(
            transform.position, // ������ģ���ǰ����λ�ã�
            boomRadius     // �뾶      
        );

        // �������
        foreach (var collider in collidersInRange)
        {
            if( collider.TryGetComponent<Enemy>(out Enemy targetEnemy))
            {
                targetEnemy.ReceiveDamage(damage_Friendly);
            }

            
        }
    }

    // ��Scene��ͼ�л��Ƽ�ⷶΧ���������ԣ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boomRadius);
    }
}
