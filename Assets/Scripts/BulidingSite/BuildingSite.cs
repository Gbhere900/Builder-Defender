using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackBuilding;

public class BuildingSite : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] protected List<Building> buildings;
    [SerializeField] protected Building currentBuilding;
    private bool hasBuilt = false;
    [SerializeField] protected int maxLevel = 3;
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int firstBuildCost = 3;
    [SerializeField] protected List<int> LevelCost;
    public Action OnInterace;

    [Header("升级")]
    //[SerializeField] protected List<UpgradeOptions> upgradeOptions;
    [SerializeField] protected List<UpgradeChoice> upgradeChoices ;

    private void Awake()
    {
        upgradeChoices = new List<UpgradeChoice>();
    }

    public void Interact()
    {
        if (!hasBuilt)
            Build();
        else
        {
           LevelUpBuilding();
        }
        OnInterace.Invoke();        //更新金币UI显示
    }
    public void Build()
    {
        currentBuilding = GameObject.Instantiate(buildings[0],transform);
        hasBuilt = true;
    }

    public void ApplyBuildingSiteData(BuildingSiteData buildingSiteData)
    {
        GameObject.Destroy(currentBuilding);
        hasBuilt = buildingSiteData.hasBuilt;
        if (!buildingSiteData.hasBuilt)
        {
            return;
        }
            

        if(buildingSiteData.hasBuilt)
        {
            currentBuilding = GameObject.Instantiate(buildings[buildingSiteData.buildingData.currentLevel - 1], transform).GetComponent<Building>();
            currentBuilding.ApplyAllBuildingUpgrades(buildingSiteData.buildingData);
            //经济类完成时补充
        }
    }

    public BuildingSiteData GetBuildingSiteData()
    {
        BuildingSiteData buildingSiteData = new BuildingSiteData();
        buildingSiteData.ID = ID;
        buildingSiteData.hasBuilt = hasBuilt;

        BuildingData buildingData = new BuildingData(currentLevel,upgradeChoices);
        buildingSiteData.buildingData = buildingData;
        return buildingSiteData;
    }

    public void SetID(string ID)
    {
        this.ID = ID;
    }

    public int GetFirstBuildCost()
    {

        return this.firstBuildCost; 
    }

    public bool GetHasBuilt()
    {
        return hasBuilt;
    }

    public Building GetBuilding()
    {
        return currentBuilding;
    }

    public int GetInteraceCoinCost()
    {
        if (!hasBuilt)
            return firstBuildCost;
        else
            return GetLevelUpCost();
    }


    //更新upgradeChoice ,currentLevel,创建新预制体，计算并且应用升级
    public void LevelUpBuilding()       
    {
        currentLevel++;
        upgradeChoices.Add(new UpgradeChoice(1, 1));
        GameObject.Destroy(currentBuilding.gameObject);
        currentBuilding = GameObject.Instantiate(buildings[currentLevel - 1], transform).GetComponent<Building>();
        BuildingData buildingData = new BuildingData(currentLevel, upgradeChoices);
        currentBuilding.ApplyAllBuildingUpgrades(buildingData);
        //switch (currentBuilding)
        //{
        //    case (AttackBuilding attackBuilding):
        //        attackBuilding.ApplyBuidingUpgradeChoice(new UpgradeChoice(1, 1)); //因为不同类型建筑应用升级不一样所以要switch;
                
        //        break;
        //        //添加经济类建筑时增加
        //}
    }

    public int GetLevelUpCost()
    {
        return LevelCost[currentLevel - 1];
    }
}
