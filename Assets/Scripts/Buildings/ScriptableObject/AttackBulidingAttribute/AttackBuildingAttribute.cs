using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackBuildingAttribute", menuName = "AttackBuildingAttribute", order = 0)]
public class AttackBuildingAttribute : ScriptableObject
{
    public float basicMaxHealth;
    public float basicDamage;
    public float basicAttackRange;
    public float basicArrowSpeed;
    public float basicAttackCD;
}
