using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyUnitUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private FriendlyUnitHealth friendlyUnitHealth;
    private CinemachineBrain brain;

    private void OnEnable()
    {
        Initialize();
        friendlyUnitHealth.OnHealthChanged += FriendlyUnitHeaelth_OnHealthChanged;
    }

    private void OnDisable()
    {
        friendlyUnitHealth.OnHealthChanged -= FriendlyUnitHeaelth_OnHealthChanged;
    }


    private void FriendlyUnitHeaelth_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = friendlyUnitHealth.GetHealth() / friendlyUnitHealth.GetMaxHealth();
    }


    private void Initialize()
    {
        friendlyUnitHealth = GetComponent<FriendlyUnitHealth>();
    }
}
