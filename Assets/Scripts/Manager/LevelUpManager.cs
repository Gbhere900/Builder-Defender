using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    private static LevelUpManager instance;

    public static LevelUpManager Instance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

   public void UpgradeAttackBuilding(AttackBuilding attackBuilding,UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.ShooterTower_L2:
                break;
            case UpgradeType.SniperTower_L2:
                attackBuilding.AddComponent<SniperTower_L2>();
                break;
            case UpgradeType.ArmorTower_L2:
                attackBuilding.AddComponent<ArmorTower_L2>();
                break;
            case UpgradeType.FortTower_L2:
                attackBuilding.AddComponent<FortTower_L2>();
                break;
            case UpgradeType.ShooterTower_L3:
                attackBuilding.AddComponent<ShooterTower_L3>();
                break;
            case UpgradeType.SniperTower_L3:
                break;
            case UpgradeType.FireTower_L3:
                attackBuilding.AddComponent<FireTower_L3>();
                break;
            case UpgradeType.HealTower_L3:
                attackBuilding.AddComponent<HealTower_L3>();
                break;
        }

    }

}
