using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyFeature
{
   public EnemyType enemyType;
   public bool canFly = false;
   public bool isRangedEnemy;
}

public enum EnemyType
{
    Monster,
    Siege,
    Human
}


