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


    public GameObject GetObject(GameObject prefab)
    {
        string name = NormalizeName(prefab.name);
        if(ObjectPoolDictionary.ContainsKey(name))
        {
            return ObjectPoolDictionary[name].Get();
        }

        CreateObjectPoolClass(name,prefab);

        return ObjectPoolDictionary[name].Get();
    }

    public void ReleaseObject(GameObject prefab)
    {
        string name = NormalizeName(prefab.name);
        if (ObjectPoolDictionary.ContainsKey(name))
        {
            ObjectPoolDictionary[name].Release(prefab);

           // Debug.Log("����" + prefab.name);
            return;
        }
      //  Debug.Log(prefab.name);
        Debug.LogWarning("δ�ҵ�"+prefab.gameObject.name+"�Ķ����,ֱ�ӽ���������");
        GameObject.Destroy(prefab);
    }

    

    private ObjectPoolClass CreateObjectPoolClass(string name,GameObject prefab)
    {
        ObjectPoolClass tempObjectPool = new ObjectPoolClass(prefab);
        ObjectPoolDictionary[name] = tempObjectPool;
        Debug.LogWarning("δ�ҵ�" + name + "��Ӧ�Ķ���أ��Ѵ���KeyΪ"+ name+"�Ķ����");
        return tempObjectPool;
    }

    private string NormalizeName(string name)
    {
        int cloneIndex = name.IndexOf("(Clone)");
        return cloneIndex >= 0 ? name.Substring(0, cloneIndex) : name;
    }

}
