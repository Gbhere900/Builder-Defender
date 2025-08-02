using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FriendlyOBject : MonoBehaviour
{
    [SerializeField] private FriendlyUnitType friendlyUnitType;
    public Action<FriendlyOBject> OnDestroyed;
    private void OnEnable()
    {
        Debug.Log("将激活的FriendlyObject放入列表");
        FriendlyOBjectManager.Instance().AddFriendlyObject(this);
    }

    private void OnDisable()
    {
        FriendlyOBjectManager.Instance().RemoveFriendlyObject(this);
        OnDestroyed?.Invoke(this);
    }


    public FriendlyUnitType GetFriendlyUnitType()
    { 
        return friendlyUnitType; 
    }



}
