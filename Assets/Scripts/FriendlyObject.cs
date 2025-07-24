using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FriendlyOBject : MonoBehaviour
{
    [SerializeField] private FriendlyUnitType friendlyUnitType;
    public Action OnDestroyed;
    private void OnEnable()
    {
        FriendlyOBjectManager.Instance().AddFriendlyObject(this);
    }

    private void OnDisable()
    {
        FriendlyOBjectManager.Instance().RemoveFriendlyObject(this);
        OnDestroyed.Invoke();
    }


    public FriendlyUnitType GetFriendlyUnitType()
    { 
        return friendlyUnitType; 
    }



}
