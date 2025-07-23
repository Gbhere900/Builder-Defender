using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolClass
{
    private GameObject prefab;
    private ObjectPool<GameObject> objectPool;
    public ObjectPoolClass(GameObject prefab)
    {

        this.prefab = prefab;
        objectPool = new ObjectPool<GameObject>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }


    public GameObject Get()
    {
        return objectPool.Get();
    }


    public void Release(GameObject gameObject)
    {
        objectPool.Release(gameObject);
    }
    private GameObject CreateFunction()
    {
        return GameObject.Instantiate(prefab);
    }

    private void ActionOnGet(GameObject prefabs)
    {
        prefabs.gameObject.SetActive(true);
    }

    private void ActionOnRelease(GameObject prefabs)
    {
        prefabs.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(GameObject prefabs)
    {
        GameObject.Destroy(prefabs.gameObject);
    }

}
