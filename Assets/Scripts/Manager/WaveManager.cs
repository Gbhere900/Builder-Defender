using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance;

    [SerializeField] private int waveCount = 4;
    private int currentWaveNum = 1;

    private EnemySpawnArea[] enemySpawnAreas;

    private List<WaveData> waveDatas;
    private List<BuildingSite> buildingSites;

    private void Awake()
    {
        instance = this;
        enemySpawnAreas = FindObjectsOfType<EnemySpawnArea>();
        waveDatas = new List<WaveData>(waveCount);
        buildingSites = FindObjectsOfType<BuildingSite>().ToList<BuildingSite>();
        for(int i = 0;i<buildingSites.Count;i++)
        {
            buildingSites[i].SetID(i.ToString());
        }
    }

    public static WaveManager Instance()
    {
        return instance; 
    }

    public  void BeginWave(int waveNum)       //waveIndexΪʵ�ʵĲ������������һ��Ϊ1
    {
        foreach(EnemySpawnArea enemySpawnArea in enemySpawnAreas)
        {
            enemySpawnArea.TryBeginWave(waveNum);
        }

        waveDatas[currentWaveNum-1].buildingSitesData.Clear();
        for(int i=0;i<buildingSites.Count;i++)
        {
            waveDatas[currentWaveNum - 1].buildingSitesData.Add(buildingSites[i].GetBuildingSiteData());
        }
        
    }

    public void BeginNextWave()
    {
        Debug.Log("���ι�����ִ����һ������");

        if (currentWaveNum > waveCount)
        {
            Debug.LogWarning("currentWaveIndex > Waves.Count���߼������");
            return;
        }
        currentWaveNum++;
        BeginWave(currentWaveNum);

    }
}
