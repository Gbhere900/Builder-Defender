using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    [SerializeField] private Dictionary<string, ObjectPoolClass> ObjectPoolDictionary = new Dictionary<string, ObjectPoolClass>();

    private void Awake()
    {
        instance = this;
    }


    static public ObjectPoolManager Instance()
    {
        return instance;
    }


    public GameObject GetObject(GameObject prefab )
    {
        string name = prefab.name;
        if(ObjectPoolDictionary.ContainsKey(name))
        {
            return ObjectPoolDictionary[name].Get();
        }

        CreateObjectPoolClass(name,prefab);

        return ObjectPoolDictionary[name].Get();
    }

    private ObjectPoolClass CreateObjectPoolClass(string name,GameObject prefab)
    {
        ObjectPoolClass tempObjectPool = new ObjectPoolClass(prefab);
        ObjectPoolDictionary[name] = tempObjectPool;
        Debug.LogWarning("未找到" + name + "对应的对象池，已创建");
        return tempObjectPool;
    }



}
