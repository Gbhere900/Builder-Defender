using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase activeVirtualCamera;

    private void Awake()
    {
        activeVirtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        //if (activeVirtualCamera == null)
        //    Debug.LogError("activeVirtualCamera ЮЊПе");
        //healthBar.transform.up = activeVirtualCamera.gameObject.transform.position - healthBar.transform.position;

        transform.forward = activeVirtualCamera.transform.position - transform.position;
        transform.right = activeVirtualCamera.transform.up;
        // healthBar.transform.forward = - Camera.main.transform.right;
    }


}
