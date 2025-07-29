using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance;

    private int waveCount = 4;
    private int currentWaveIndex = 1;

    private EnemySpawnArea[] enemySpawnAreas;

    private void Awake()
    {
        instance = this;
        enemySpawnAreas = FindObjectsOfType<EnemySpawnArea>();
    }

    public static WaveManager Instance()
    {
        return instance; 
    }

    public  void BeginWave(int waveIndex)
    {
        foreach(EnemySpawnArea enemySpawnArea in enemySpawnAreas)
        {
            enemySpawnArea.TryBeginWave(waveIndex);
        }
    }

    public void BeginNextWave()
    {
        Debug.Log("���ι�����ִ����һ������");

        if (currentWaveIndex > waveCount)
        {
            Debug.LogWarning("currentWaveIndex > Waves.Count���߼������");
            return;
        }
        currentWaveIndex++;
        BeginWave(currentWaveIndex);

    }
}
