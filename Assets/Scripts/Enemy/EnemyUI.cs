using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private Vector3 healthBarOffset;
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
        Debug.Log(healthBar.fillAmount);
        healthBar.fillAmount =enemy.GetHealth() / enemy.GetMaxHealth();
    }
    private void Update()
    {
        UpdateHealthBarPosition();
        UpdateHealthBarRotation();
    }

    private void UpdateHealthBarPosition()
    {
       // Debug.Log(healthBarOffset);
        healthBar.transform.position = enemy.transform.position + healthBarOffset;
    }
    private void Initialize()
    {
        enemy = GetComponent<Enemy>();
        healthBarOffset = healthBar.gameObject.transform.position - transform.position;
    }
    private void UpdateHealthBarRotation()
    {
        healthBar.transform.up = Camera.main.transform.position - healthBar.transform.position;
       // healthBar.transform.forward = - Camera.main.transform.right;
    }
}
