using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI totalCoinsText;
    [SerializeField] private long  totalCoins;
    public bool reward2X;

    private void OnEnable()
    {
        UIManager.DeletePref += DeletePrefsNow;
    }

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        totalCoins = LoadCoinPref();
        SetCoinsText();
    }

    public void AddCoins(long coinsGained)
    {
        if (reward2X)
        {
            totalCoins += (coinsGained *2);
            SaveCoinPref(totalCoins);
            SetCoinsText();
        }
        else
        {
            totalCoins += coinsGained;
            SaveCoinPref(totalCoins);
            SetCoinsText();
        }
        
    }

    public void DecreaseCoins(long coinsLost)
    {
        totalCoins -= coinsLost;
        SaveCoinPref(totalCoins);
        SetCoinsText();
    }

    public void SetCoinsText()
    {
        totalCoinsText.text = FormatNumber(totalCoins);
    }

    public string FormatNumber(long number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }
        else if (number < 1000000)
        {
            return (number / 1000.0).ToString("0.0") + "k";
        }
        else if (number < 1000000000)
        {
            return (number / 1000000.0).ToString("0.0") + "M";
        }
        else if (number < 1000000000000)
        {
            return (number / 1000000000.0).ToString("0.0") + "B";
        }
        else if (number < 1000000000000000)
        {
            return (number / 1000000000000.0).ToString("0.0") + "T";
        }
        else
        {
            // You can add more conditions for larger numbers if needed
            return "Too large";
        }
    }

    public void SaveCoinPref(long coinQty)
    {
        int highBits = (int)(coinQty >> 32);
        int lowBits = (int)(coinQty & 0xFFFFFFFF);

        PlayerPrefs.SetInt(prefs.totalcoinPref + "_high", highBits);
        PlayerPrefs.SetInt(prefs.totalcoinPref + "_low", lowBits);
    }

    public long LoadCoinPref()
    {
        int highBits = PlayerPrefs.GetInt(prefs.totalcoinPref + "_high");
        int lowBits = PlayerPrefs.GetInt(prefs.totalcoinPref + "_low");

        return ((long)highBits << 32) | (uint)lowBits;
    }


    public long GetTotalCoins() => totalCoins;

    public void DeletePrefsNow()
    {
        PlayerPrefs.DeleteKey(prefs.totalcoinPref + "_high");
        PlayerPrefs.DeleteKey(prefs.totalcoinPref + "_low");
    }

    private void OnDisable()
    {
        UIManager.DeletePref -= DeletePrefsNow;
    }

}
