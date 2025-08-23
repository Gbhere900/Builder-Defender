using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int coinCount;         //´ýÖØ¹¹
    private static CoinManager instance;

    private void Awake()
    {
        instance = this;
    }
    public static CoinManager Instance()
    {
        return instance;
    }
    public bool TrySpendCoins(int cost)
    {
        if (coinCount < cost)
            return false;

        coinCount -= cost;
        return true;
    }

    public int GetCoinCount()
    {

        return coinCount; 
    }

    public void SetCoinCount(int coin)
    {
        coinCount = coin;
    }
}
