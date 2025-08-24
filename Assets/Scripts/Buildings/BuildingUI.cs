using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private Building building;

    private void OnEnable()
    {
        Initialize();
        building.OnHealthChanged += Building_OnHealthChanged;
    }
    private void OnDisable()
    {
        building.OnHealthChanged -= Building_OnHealthChanged;
    }


    private void Building_OnHealthChanged(float fomerHealth,float currentHealrh)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = building.GetHealth() / building.GetMaxHealth();
    }


    private void Initialize()
    {
        building = GetComponent<Building>();
    }
}
