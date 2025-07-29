using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
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
    }
}
