using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 direction;

    [Header("½Å±¾×é¼þ")]
    [SerializeField]private Rigidbody rigidbody;
    [SerializeField]private Collider collider;

    private void Awake()
    {
        
    }

    private void Update()
    {
        MoveInput();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void MoveInput()
    {
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        
    }

    private void Move()
    {
        rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
    }
}
