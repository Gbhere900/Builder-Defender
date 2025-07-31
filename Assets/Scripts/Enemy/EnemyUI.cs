using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private Enemy enemy;

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
        healthBar.fillAmount =enemy.GetHealth() / enemy.GetMaxHealth();
    }



    private void Initialize()
    {
        enemy = GetComponent<Enemy>();

    }

}
