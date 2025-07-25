using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private Vector3 healthBarOffset;
    private Enemy enemy;
    private CinemachineBrain brain;

    private void OnEnable()
    {
        Initialize();
        enemy.OnHealthChanged += Enemy_OnHealthChanged;
    }

    private void OnDisable()
    {
        enemy.OnHealthChanged -= Enemy_OnHealthChanged;
    }


    private void Enemy_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        Debug.Log(healthBar.fillAmount);
        healthBar.fillAmount =enemy.GetHealth() / enemy.GetMaxHealth();
    }
    private void Update()
    {

    }


    private void Initialize()
    {
        enemy = GetComponent<Enemy>();

        healthBarOffset = healthBar.gameObject.transform.position - transform.position;

        //// ��ȡ��������ϵ�CinemachineBrain���
        //Camera mainCamera = Camera.main;
        //brain = mainCamera.GetComponent<CinemachineBrain>();

        //if (brain == null)
        //{
        //    Debug.LogError("���������δ�ҵ�CinemachineBrain�����");
        //    return;
        //}

        //// ��ȡ��ǰ�����Virtual Camera
        //activeVirtualCamera = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
    }

}
