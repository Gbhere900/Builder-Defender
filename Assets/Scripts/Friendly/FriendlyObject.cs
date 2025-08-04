using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FriendlyObject : MonoBehaviour
{
    [SerializeField] private FriendlyUnitType friendlyUnitType;
    public Action<FriendlyObject> OnDestroyed;
    private void OnEnable()
    {
        Debug.Log("�������FriendlyObject�����б�");
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
