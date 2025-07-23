using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Arrow : MonoBehaviour
{
    private float damage;
    private float speed;
    [SerializeField] private float timeToLive;
    [SerializeField] private AnimationCurve heightOffsetCurve;
    [SerializeField] private AnimationCurve PositionCurve;

    private float sumDistance = 0;
    private Vector3 directionXZ;
    private float beginX;
    private float beginY;
    private float beginZ;
    private Enemy attackTarget;
    private Vector3 attackTargetPosition;

    [SerializeField] private LayerMask goundLayer;

    private void OnEnable()
    {
        StartCoroutine(WaitForRelease());
    }

    IEnumerator WaitForRelease()
    {
        yield return new WaitForSeconds(timeToLive);
        ObjectPoolManager.Instance().ReleaseObject(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.ReceiveDamage(damage);
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
        }

        //if (other.gameObject.tag == "Gound")        //后续可能重构
        //{
        //    ObjectPoolManager.Instance().ReleaseObject(gameObject);
        //}

       if((1 << other.gameObject.layer & goundLayer) != 0)
       {
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
       }
    }




    private void Update()
    {

        Move();
    }

    private void Move()
    {
        if(attackTarget != null)
        {
            attackTargetPosition = attackTarget.transform.position;
        }
        
        float height = 3f;

        float deltaX = attackTargetPosition.x - transform.position.x;
        float deltaZ = attackTargetPosition.z - transform.position.z;
        directionXZ = new Vector3(deltaX, 0, deltaZ).normalized;

        Vector3 offset = directionXZ * speed *Time.deltaTime;

        transform.position = transform.position + offset;

        float distanceXZ = Vector2.Distance(
    new Vector2(transform.position.x, transform.position.z),
    new Vector2(attackTargetPosition.x, attackTargetPosition.z)
);
        float percent = 1 - distanceXZ / sumDistance;
        float heightOffset = height * heightOffsetCurve.Evaluate(percent);

        float y = (1 - percent) * beginY + (percent) * attackTargetPosition.y;

        Vector3 position = new Vector3(transform.position.x, y + heightOffset, transform.position.z);

        transform.position = position;

        CalculateForwardVector();
    }

    public void Initialize(Vector3 position, Enemy attackTarget,float damage,float speed)
    {
        Debug.Log(position);
        transform.position = position;

        this.attackTarget = attackTarget;
        this.damage = damage;
        this.speed = speed;

        //float deltaX = attackTarget.transform.position.x - transform.position.x;
        //float deltaZ = attackTarget.transform.position.z - transform.position.z;
        //directionXZ = new Vector3 (deltaX,0,deltaZ).normalized;

        sumDistance = Vector2.Distance(
    new Vector2(transform.position.x, transform.position.z),
    new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));

        
        beginY = transform.position.y;

    }

   private void CalculateForwardVector()
    {
        transform.forward = attackTargetPosition - transform.position;
    }
}
