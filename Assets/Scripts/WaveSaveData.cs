using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class BuildingSiteData
    { 
        public string ID;
        public bool hasBuilt;
        public BuildingData buildingData;
    }

    public class BuildingData
    {
        public int currentLevel;
        public List<UpgradeChoice> upgradeChoices;

        public BuildingData(int currentLevel, List<UpgradeChoice> upgradeChoices)
        {
            this.currentLevel = currentLevel;
            this.upgradeChoices = upgradeChoices;
        }
    }

    public class UpgradeChoice
    {
        public int levelFrom;
        public int choice;

        public UpgradeChoice(int levelFrom, int choice)
        {
            this.levelFrom = levelFrom;
            this.choice = choice;
        }
    }

[Serializable]
    public class UpgradeOption
    {
        public UpgradeType upgradeType;
        public string name;
        public string description;
        public float healthBoost_flat;
        public float damageBoost_percent;
        public float attackRangeBoost_percent;
        public float arrowSpeedBoost_percent;
        public float attackCDBoost_percent;
}

public enum UpgradeType
{
    ShooterTower_L2,
    SniperTower_L2,
    ArmorTower_L2,
    FortTower_L2,
    ShooterTower_L3,
    SniperTower_L3,
    FireTower_L3,
    HealTower_L3
}

[Serializable]
public class UpgradeOptions
{
    public List<UpgradeOption> upgradeOptions;
}

public class WaveData
{
    public int waveCount;
    public List<BuildingSiteData> buildingSitesData;
}







