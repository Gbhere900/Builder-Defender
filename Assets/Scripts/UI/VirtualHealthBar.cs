using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualHealthBar : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private Image healthBar;
    private Image virtualHealthBar;

    private void OnEnable()
    {
        virtualHealthBar = GetComponent<Image>();
    }
    private void Update()
    {
        if(virtualHealthBar.fillAmount > healthBar.fillAmount)
        virtualHealthBar.fillAmount -= speed * Time.deltaTime;
        if(virtualHealthBar.fillAmount < healthBar.fillAmount)
        {
            virtualHealthBar.fillAmount = healthBar.fillAmount;
        }
    }
}
