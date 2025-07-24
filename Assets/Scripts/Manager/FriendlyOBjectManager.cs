using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyOBjectManager : MonoBehaviour
{
    static FriendlyOBjectManager instance;

    private List<FriendlyOBject> FriendlyObjectList = new List<FriendlyOBject>();

    
    private void Awake()
    {
        instance = this;
    }


    static public FriendlyOBjectManager Instance()
    {
        return instance; 
    }

    public void AddFriendlyObject(FriendlyOBject friendlyOBject)
    {
        FriendlyObjectList.Add(friendlyOBject);
    }

    public void RemoveFriendlyObject(FriendlyOBject friendlyOBject)
    {
        FriendlyObjectList.Remove(friendlyOBject);
    }


    public List<FriendlyOBject> GetFriendlyObjetcList()
    {
        return FriendlyObjectList;
    }
}
