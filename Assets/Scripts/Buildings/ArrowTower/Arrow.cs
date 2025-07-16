using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Arrow : MonoBehaviour
{
    private float damage;
    private float speed;
    [SerializeField] private AnimationCurve heightOffsetCurve;
    [SerializeField] private AnimationCurve PositionCurve;

    private float sumDistance = 0;
    private Vector3 directionXZ;
    private float beginX;
    private float beginY;
    private float beginZ;
    private Enemy attackTarget;

    private void OnEnable()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>())
        {
            Enemy.
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float height = 1f;

        Vector3 offset = directionXZ * speed *Time.deltaTime;

        transform.position = transform.position + offset;

        float distanceXZ = Vector2.Distance(
    new Vector2(transform.position.x, transform.position.z),
    new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z)

);
        float percent = 1 - distanceXZ / sumDistance;
        float heightOffset = height * heightOffsetCurve.Evaluate(percent);

        float y = percent * beginY + (1-percent) * attackTarget.transform.position.y;

        Vector3 position = new Vector3(transform.position.x, y + heightOffset, transform.position.z);

        transform.position = position;
        transform.forward = attackTarget.transform.position - transform.position;
    }

    public void Initialize(Enemy attackTarget,float damage,float speed)
    {
        this.attackTarget = attackTarget;
        this.damage = damage;
        this.speed = speed;

        float deltaX = attackTarget.transform.position.x - transform.position.x;
        float deltaZ = attackTarget.transform.position.z - transform.position.z;
        directionXZ = new Vector3 (deltaX,0,deltaZ);

        sumDistance = Vector2.Distance(
    new Vector2(transform.position.x, transform.position.z),
    new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));

        
        beginY = transform.position.y;
    }
}
