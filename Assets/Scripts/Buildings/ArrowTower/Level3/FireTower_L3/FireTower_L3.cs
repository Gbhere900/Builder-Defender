using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower_L3 : MonoBehaviour
{
    private AttackBuilding attackBuilding;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Bullet fireArrowPrefab;
    [SerializeField] private float fireCD = 3;

    private void OnEnable()
    {
        attackBuilding = GetComponent<AttackBuilding>();
        attackBuilding.SetBulletPrefab(fireArrowPrefab);
        StartCoroutine(WaitForFireCD());
    }

    IEnumerator WaitForFireCD()
    {
        while(true)
        {
            yield return new WaitForSeconds(fireCD);
            SpawnFire();
        }

    }

    private void SpawnFire()
    {
        ObjectPoolManager.Instance().GetObject(firePrefab).transform.position = transform.position;
    }
}
