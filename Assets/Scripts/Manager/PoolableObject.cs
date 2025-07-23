using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public string id = "";
    private void Awake()
    {
        if (id == "") 
        id = gameObject.name;
    }
}
