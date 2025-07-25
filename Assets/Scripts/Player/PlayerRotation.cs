using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Vector3 direction;
    private void Update()
    {
        direction = MousePositionManager.GetMousePosition() - transform.position;
        direction.y = 0;
        transform.forward = direction;
    }
}
