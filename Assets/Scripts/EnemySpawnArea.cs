using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
    [SerializeField] private Transform areaEdge1;

    [SerializeField] private List<Wave> Waves;


    private bool[] hasBegun = new bool[20];
    Wave currentWave;
    float timer = 0f;

    [Serializable]
    public struct Enemies
    {
        public Enemy enemy;
        public int count;
        public float spawnCD;
        public float beginTime;
    }

    [Serializable]
    public struct Wave
    {
        public int waveIndex ;
        public bool isValid;
        public List<Enemies> wave;
    }




    private void OnEnable()
    {
        //BeginWave(1);  //等待重构
    }

    public void TryBeginWave(int waveIndex)       //每次开始当前波次都要调用
    {
        if (waveIndex > Waves.Count)
        {
            Debug.LogWarning("指定开始的波次不存在或波次索引有误");
            return;
        }

        //if(!Waves.ContainsKey(waveIndex))
        //{
        //    return;
          
        //}
        BeginWave(waveIndex);
    }

    public void BeginWave(int waveIndex)       //每次开始当前波次都要调用
    {

        currentWave = Waves[waveIndex - 1];
        for(int i=0;i<hasBegun.Length;i++)
        {
            hasBegun[i] = false;
        }

        timer = 0f;
    }




    private void Update()
    {
        timer += Time.deltaTime;
        if (!currentWave.isValid)
            return;
        
        for (int i = 0; i < currentWave.wave.Count;i++)
        {
            if(timer > currentWave.wave[i].beginTime)
            {
                if (!hasBegun[i])
                {
                    StartCoroutine(WaitForSpawnCDAndSpawn(currentWave.wave[i]));
                    hasBegun[i] = true;
                }
                
            }
        }
        
    }


    IEnumerator WaitForSpawnCDAndSpawn(Enemies enemies)
    {
        for(int i = 0;i<enemies.count;i++)
        {
            GameObject enemy = ObjectPoolManager.Instance().GetObject(enemies.enemy.gameObject);
            enemy.transform.position = GetRandomPositionInArea();
            yield return new WaitForSeconds(enemies.spawnCD);
        }

    }

    public Vector3 GetRandomPositionInArea()
    {
        float deltaX = Math.Abs(areaEdge1.position.x - transform.position.x);
        float deltaZ = Math.Abs(areaEdge1.position.z - transform.position.z);
        Vector3 position = new Vector3(UnityEngine.Random.Range(-deltaX,deltaX),
            0, UnityEngine.Random.Range(-deltaZ,deltaZ));

        return position + transform.position;
    }


}
