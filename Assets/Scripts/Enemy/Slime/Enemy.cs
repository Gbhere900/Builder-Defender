using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("数值")]
    [SerializeField] private bool canFly;

    [SerializeField] private float originalSpeed;
    private float speed;

    [SerializeField] private float originalDamageToUnit;
    private float damageToUnit;

    [SerializeField] private float originalDamageToPlayer;
    private float damageToPlayer;

    [SerializeField] private float originalDamageToBuilding;
    private float damageToBuilding;

    [SerializeField] private float attackCD;

    private PlayerHealth attackAim;
    private bool attackReady = true;


    [Header("脚本组件")]
    [SerializeField] private Collider collider;
    [SerializeField] private Rigidbody rigidbody;

    private void Awake()
    {
        Initialize();
        attackAim = GameObject.FindFirstObjectByType<PlayerHealth>(); //需要重构
    }

    IEnumerator WaitForAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackReady = true;
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!attackReady) return;
        if (collision.gameObject.GetComponent<PlayerHealth>())
            Attack();

    }

    private void Move()
    {
        Vector3 direction = attackAim.transform.position - transform.position;
        rigidbody.velocity = direction * Time.deltaTime * speed;
    }
    private void Attack()
    {
        attackReady = false;
        attackAim.GetComponent<PlayerHealth>().ReceiveDamage(damageToPlayer);
        StartCoroutine(WaitForAttackCD());
    }

    private void Initialize()
    {
        speed = originalSpeed;
        damageToUnit = originalDamageToUnit;
        damageToPlayer = originalDamageToPlayer;
        damageToBuilding = originalDamageToBuilding;

        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }
}
