using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEnhance : ScriptableObject
{
    [SerializeField] public Mesh newMesh;
    [SerializeField] public string description;
    [SerializeField] public float maxHealthEnhance;
    [SerializeField] public float damageEnhance;
    [SerializeField] public float attackRangeEnhance;
    [SerializeField] public float attackSpeedEnhance;
    [SerializeField] public float arrowSpeedEnhance;
    [SerializeField] public MonoBehaviour newScript;
}
