using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform FollowObeject;
    private Vector3 deltaVector;

    private void Awake()
    {
        
        deltaVector = transform.position - FollowObeject.position;
        //transform.forward = FollowObeject.transform.position - transform.position;
    }
    private void Update()
    {
        
        transform.position = deltaVector  + FollowObeject.position;
        
    }
}
