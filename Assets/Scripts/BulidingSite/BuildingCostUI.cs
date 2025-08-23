using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCostUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image coinImage;

    private void Awake()
    {
        canvas.enabled = false;
    }
    public void ShowUI(int cost)
    {
        for(int i = 0;i<canvas.transform.childCount;i++)
        {
            GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
        }
        
        for(int i = 0;i< cost;i++)
        {
            GameObject.Instantiate(coinImage, canvas.transform);
        }
        canvas.enabled = true;

    }

    public void HideUI()
    {
        canvas.enabled = false;
    }
}
