using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePositionManager : MonoBehaviour
{
    static private MousePositionManager instance;
    private Ray ray;
    [SerializeField] private LayerMask mask;
    private void Awake()
    {
        instance = this;
    }

    static public Vector3 GetMousePosition()
    {
        Vector3 mousePosition;
        instance.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(instance.ray, out RaycastHit rayCastHit, float.MaxValue, instance.mask);
        mousePosition = rayCastHit.point;
   
        return mousePosition;
    }
    public static MousePositionManager Instance()
    {
        return instance;
    }
}