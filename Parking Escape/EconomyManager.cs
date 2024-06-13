using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int coins;

    public static EconomyManager instace;

    private void Awake()
    {
       if(instace == null)
        {
            instace = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(PlayerPrefManager.TotalConins))
        {
            coins = GetCoinPlayerPref();
        }

        UpdateCoinsInUI();
    }

    public void IncreaseFromDailyRewards(int coin)
    {
        coins += coin;
        SetCoinPlayerPref();
        UpdateCoinsInUI();
    }

    public void UpdateCoinsInUI()
    {
        UIManager.instance.UpdateCoins(coins);
    }

    public void SetCoinPlayerPref()
    {
        PlayerPrefs.SetInt(PlayerPrefManager.TotalConins, coins);
    }

    public int GetCoinPlayerPref()
    {
        return PlayerPrefs.GetInt(PlayerPrefManager.TotalConins);
    }
}
