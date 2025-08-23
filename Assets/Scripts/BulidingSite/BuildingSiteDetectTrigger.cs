using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSiteDetectTrigger : MonoBehaviour
{
    [SerializeField] private BuildingSite buildingSite;
    [SerializeField] private BuildignChoiceUI buildingChoiceUI;  
    [SerializeField] private BuildingCostUI buildingCostUI;  
    private void Awake()
    {
        buildingSite = GetComponentInParent<BuildingSite>();
    }

    private void OnEnable()
    {
        PlayerBuild.OnChangeBuildingSite += HideBuildCostUI;
        buildingSite.OnInterace += BuildingSite_OnInteract;
    }

    private void OnDisable()
    {
        PlayerBuild.OnChangeBuildingSite -= HideBuildCostUI;
        buildingSite.OnInterace -= BuildingSite_OnInteract;

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.TryGetComponent<PlayerResource>(out PlayerResource playerResource))
    //    {
    //        playerResource.ChangeBuildingSite(buildingSite);
    //        ShowBuildCostUI();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent<PlayerResource>(out PlayerResource playerResource))
    //    {
    //        playerResource.ClearBuildingSite();
    //        HideBuildCostUI();
    //    }
    //}

    public void ShowBuildCostUI()
    {
        if (!buildingSite.GetHasBuilt())
            buildingCostUI.ShowUI(buildingSite.GetFirstBuildCost());
        else
        {
            Building building = buildingSite.GetBuilding();
            int cost = buildingSite.GetLevelUpCost();
            buildingCostUI.ShowUI(cost);
        }
    }

    public void HideBuildCostUI()
    {
        buildingCostUI.HideUI();
    }

    public BuildingSite GetBuildingSite()
    {
        return buildingSite;
    }

    private void BuildingSite_OnInteract()
    {
        ShowBuildCostUI();
    }
}
