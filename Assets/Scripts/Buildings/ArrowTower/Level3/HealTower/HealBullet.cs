using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBullet : Bullet
{

    protected override void OnTriggerEnterLogic(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject == attackTarget)
        {
            attackTarget.GetComponent<FriendlyUnitHealth>().ReceiveHealing(damage_Friendly);
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
        }

        //if (other.TryGetComponent<FriendlyUnitHealth>(out FriendlyUnitHealth friendlyUnitHealth))       
        //{

        //     friendlyUnitHealth.ReceiveHealing(damage);
        //     ObjectPoolManager.Instance().ReleaseObject(gameObject);
        //}

        //if (other.gameObject.tag == "Gound")        //后续可能重构
        //{
        //    ObjectPoolManager.Instance().ReleaseObject(gameObject);
        //}

        if ((1 << other.gameObject.layer & goundLayer) != 0)
        {
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
        }
    }

}
