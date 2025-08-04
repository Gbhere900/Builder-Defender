using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyOBjectManager : MonoBehaviour
{
    static FriendlyOBjectManager instance;

    private List<FriendlyObject> FriendlyObjectList = new List<FriendlyObject>();

    
    private void Awake()
    {
        instance = this;
    }


    static public FriendlyOBjectManager Instance()
    {
        return instance; 
    }

    public void AddFriendlyObject(FriendlyObject friendlyOBject)
    {
        FriendlyObjectList.Add(friendlyOBject);
    }

    public void RemoveFriendlyObject(FriendlyObject friendlyOBject)
    {
        FriendlyObjectList.Remove(friendlyOBject);
    }


    public List<FriendlyObject> GetFriendlyObjetcList()
    {
        return FriendlyObjectList;
    }
}
