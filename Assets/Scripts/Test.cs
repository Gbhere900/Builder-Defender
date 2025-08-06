using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private AttackBuilding attackBuilding;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            WaveManager.Instance().BeginNextWave();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            WaveManager.Instance().BeginWave(1);
        }


        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            attackBuilding.LevelUP_L2(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            attackBuilding.LevelUP_L2(2);
        }
    }
}
