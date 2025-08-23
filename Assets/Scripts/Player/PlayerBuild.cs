using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{

    [SerializeField] private BuildingSite currentBuildingSite;
    public static Action OnChangeBuildingSite;



    public void ChangeBuildingSite(BuildingSite buildingSite)
    {
        OnChangeBuildingSite.Invoke();
        this.currentBuildingSite = buildingSite;     
    }

    public void ClearBuildingSite()
    {
        currentBuildingSite = null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            if (CoinManager.Instance().TrySpendCoins(currentBuildingSite.GetInteraceCoinCost()))
            {
                currentBuildingSite.Interact();
            }
            else
            {
                Debug.LogWarning("金币不足无法建造/升级");
            }
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BuildingSiteDetectTrigger>(out BuildingSiteDetectTrigger buildingSiteDetectTrigger))
        {
            Debug.LogWarning("找到建筑点");
            ChangeBuildingSite(buildingSiteDetectTrigger.GetBuildingSite());
            buildingSiteDetectTrigger.ShowBuildCostUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BuildingSiteDetectTrigger>(out BuildingSiteDetectTrigger buildingSiteDetectTrigger))
        {
            if (buildingSiteDetectTrigger.GetBuildingSite() == currentBuildingSite)
                ClearBuildingSite();
            buildingSiteDetectTrigger.HideBuildCostUI();
        }
    }

    private void BuildCurrentBuildSpawnPoint()
    {
        currentBuildingSite.Build();
    }



}
