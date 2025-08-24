using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorTower_L2 : MonoBehaviour
{
    bool isSkillTriggered = false;
    bool isInvinciable = false;
    private AttackBuilding attackBuilding;
    [SerializeField] private float InvincibilityTime = 5;

    protected void OnEnable()
    {
        attackBuilding = GetComponent<AttackBuilding>();

        attackBuilding.OnHealthChanged += TryTriggerSkill ;
    }

    private void OnDisable()
    {
        attackBuilding.OnHealthChanged -= TryTriggerSkill;
    }


    private void TryTriggerSkill(float formerHealth,float currentHealth)
    {
        if(isInvinciable)       //如果无敌状态这返还掉的血
            attackBuilding.SetHealth(formerHealth) ;
        
        if(!isSkillTriggered)
        StartCoroutine(WaitForInvincibilityTimer());    

    }

    IEnumerator WaitForInvincibilityTimer()
    {
        isInvinciable = true;
        yield return new WaitForSeconds(InvincibilityTime);
        isInvinciable = false;
    }
}
