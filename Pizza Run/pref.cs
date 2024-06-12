using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class pref 
{
    public static string tableUnlocked = "unlockedTable";
    public static string pizzaMachine = "pizzaMachine";
    public static string secondPizzaMachine = "pizzaMachine2";
    public static string pizzaServeCustomer = "servecustomerspizza";
    public static string pizzaPackingCounter = "pizzaPackingCounter";
    public static string pizzaCarCounter = "PizzaCarCounter";
    public static string pizzaDoorUnlock = "pizzaDoorunlocked";
    public static string pizzaInitialAmount = "initialPizzaAmount";
    public static string foodaiPref = "foodAiPref";
    public static string trashaiPref = "trashAiPref";
    public static string boxaiPref = "boxaiPref";
    public static string playerspeed = "playerspeed";
    public static string playerCapacity = "playercapacity";
    public static string profits = "profits";
    public static string helperspeedPref = "helperspeedPref";
    public static string helperCapacityPref = "helperCapacityPref";
    public static string totalCoins = "coins";
    public static string numberOfCustomersServed = "numberofcustomersServed";
    public static string customerServedLevel = "CustomerServedLevel";

    public static string numberofCarsServed = "numberOfCarsServed";
    public static string carServedLevel = "CarservedLevel";

    public static string numberoftablesCleaned = "numberoftablescleaned";
    public static string tablesCleanedLevel = "tablecleanedlevel";

    public static void SetUnlockedTablePrf(int numberOfTablesUnlokced)
    {
        PlayerPrefs.SetInt(tableUnlocked, numberOfTablesUnlokced);
    }

    public static int GetNumberOfUnlockedTables()
    {
        return PlayerPrefs.GetInt(tableUnlocked);
    }

    public static void SetPizzaMachinePref()
    {
        PlayerPrefs.SetInt(pizzaMachine, 1);
    }

    public static bool IsPizzaMachineUnlocked()
    {
        if (PlayerPrefs.GetInt(pizzaMachine) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetSecondPizzaMachinePref()
    {
        PlayerPrefs.SetInt(secondPizzaMachine, 1);
    }

    public static bool IsSecondPizzaMachineUnlocked()
    {
        if (PlayerPrefs.GetInt(secondPizzaMachine) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetPizzaServingCounterPref()
    {
        PlayerPrefs.SetInt(pizzaServeCustomer, 1);
    }

    public static bool IsCustomerCounterUnlocked()
    {
        if(PlayerPrefs.GetInt(pizzaServeCustomer) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetPrefPizzaPacking()
    {
        PlayerPrefs.SetInt(pizzaPackingCounter, 1);
    }
    public static bool IsPackingCounterUnlocked()
    {
        if(PlayerPrefs.GetInt(pizzaPackingCounter) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetPrefCarCounter()
    {
        PlayerPrefs.SetInt(pizzaCarCounter, 1);
    }

    public static bool IsCarCounterUnlocked()
    {
        if(PlayerPrefs.GetInt(pizzaCarCounter) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void SetPizzaDoorPref()
    {
        PlayerPrefs.SetInt(pizzaDoorUnlock, 1);
    }

    public static bool IsPizzaDoorUnlocked()
    {
        if(PlayerPrefs.GetInt(pizzaDoorUnlock) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void SetInitialPizzaMoneyPref()
    {
        PlayerPrefs.SetInt(pizzaInitialAmount, 1);
    }

    public static bool IsInitialAmountTaken()
    {
        if(PlayerPrefs.GetInt(pizzaInitialAmount) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetPlayerUpgradePref(string x,int count)
    {
        PlayerPrefs.SetInt(x, count);
    }

    public static int GetPlayerUpgradePrefs(string x)
    {
        return PlayerPrefs.GetInt(x);
    }

    public static void SetHelperUpgrades(string x, int count)
    {
        PlayerPrefs.SetInt(x, count);
    }

    public static int GetHelperUpgrades(string x)
    {
        return PlayerPrefs.GetInt(x);   
    }
    
    public static void SetTotalCoins(int qty)
    {
        PlayerPrefs.SetInt(totalCoins, qty);
    }

    public static int GetTotalCoins()
    {
        return PlayerPrefs.GetInt(totalCoins);
    }


    //customers

    public static void SetNumberOfCustomersServed(int x)
    {
        PlayerPrefs.SetInt(numberOfCustomersServed + SceneManager.GetActiveScene().name, x);
    }

    public static int GetNumberOfCustomersServed()
    {
        return PlayerPrefs.GetInt(numberOfCustomersServed + SceneManager.GetActiveScene().name);
    }

    public static void SetCustomerServeLevel(int x)
    {
        PlayerPrefs.SetInt(customerServedLevel + SceneManager.GetActiveScene().name, x);
    }

    public static int GetCustomerServeLevel()
    {
        return PlayerPrefs.GetInt(customerServedLevel + SceneManager.GetActiveScene().name);
    }

    //cars

    public static void SetNumberOfCarsServed(int x)
    {
        PlayerPrefs.SetInt(numberofCarsServed + SceneManager.GetActiveScene().name, x);
    }

    public static int GetNumberofCarsServed()
    {
        return PlayerPrefs.GetInt(numberofCarsServed + SceneManager.GetActiveScene().name);
    }

    public static void SetCarCustomerLevel(int x)
    {
        PlayerPrefs.SetInt(carServedLevel + SceneManager.GetActiveScene().name, x);
    }

    public static int GetCarCustomerLevel()
    {
        return PlayerPrefs.GetInt(carServedLevel + SceneManager.GetActiveScene().name);
    }

    //tables cleaned
    public static void SetNumberOfTablesCleaned(int x)
    {
        PlayerPrefs.SetInt(numberoftablesCleaned + SceneManager.GetActiveScene().name, x);
    }

    public static int GetNumberOfTablesCleaned()
    {
        return PlayerPrefs.GetInt(numberoftablesCleaned + SceneManager.GetActiveScene().name);
    }

    public static void SetTableCleaningLevel(int x)
    {
        PlayerPrefs.SetInt(tablesCleanedLevel + SceneManager.GetActiveScene().name, x);
    }

    public static int GetTableCleaningLevel()
    {
        return PlayerPrefs.GetInt(tablesCleanedLevel + SceneManager.GetActiveScene().name);
    }
}
