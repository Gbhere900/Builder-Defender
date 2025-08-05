using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    protected Damage_Friendly damage_Friendly;
    protected float speed;
    [SerializeField] protected float height;
    [SerializeField] protected float timeToLive;
    [SerializeField] protected AnimationCurve heightOffsetCurve;

    protected float sumDistance = 0;
    protected Vector3 directionXZ;
    protected float beginX;
    protected float beginY;
    protected float beginZ;
    protected GameObject attackTarget;
    protected Vector3 attackTargetPosition;

    [SerializeField] protected LayerMask goundLayer;

    protected void OnEnable()
    {
        StartCoroutine(WaitForRelease());
    }

    IEnumerator WaitForRelease()
    {
        yield return new WaitForSeconds(timeToLive);
        ObjectPoolManager.Instance().ReleaseObject(gameObject);
    }
    protected  void  OnTriggerEnter(Collider other)          //子类可以重写
    {
        OnTriggerEnterLogic(other);
    }

    protected virtual void OnTriggerEnterLogic(Collider other)
    {
        if (other.isTrigger)
            return;
        if(other.gameObject == attackTarget)
        {
            attackTarget.GetComponent<Enemy>().ReceiveDamage(damage_Friendly);
            ObjectPoolManager.Instance().ReleaseObject(gameObject);
        }
        //if (other.TryGetComponent<Enemy>(out Enemy enemy))
        //{
        //    enemy.ReceiveDamage(damage);
        //    ObjectPoolManager.Instance().ReleaseObject(gameObject);
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



    protected void Update()
    {

        Move();
    }

    protected void Move()
    {
        if (attackTarget != null)
        {
            attackTargetPosition = attackTarget.transform.position;
        }

      

        float deltaX = attackTargetPosition.x - transform.position.x;
        float deltaZ = attackTargetPosition.z - transform.position.z;
        directionXZ = new Vector3(deltaX, 0, deltaZ).normalized;

        Vector3 offset = directionXZ * speed * Time.deltaTime;

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

    public void Initialize(Vector3 position,GameObject attackTarget, Damage_Friendly damage_Friendly, float speed, float timeToLive = 3,float height = 3)
    {
        transform.position = position;

        this.attackTarget = attackTarget;
        this.damage_Friendly = damage_Friendly;
        this.speed = speed;
        this.timeToLive = timeToLive;
        this.height = height;

        //float deltaX = attackTarget.transform.position.x - transform.position.x;
        //float deltaZ = attackTarget.transform.position.z - transform.position.z;
        //directionXZ = new Vector3 (deltaX,0,deltaZ).normalized;

        sumDistance = Vector2.Distance(
    new Vector2(transform.position.x, transform.position.z),
    new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));


        beginY = transform.position.y;

    }

    protected void CalculateForwardVector()
    {
        transform.forward = attackTargetPosition - transform.position;
    }

}
