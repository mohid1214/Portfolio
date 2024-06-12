using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [SerializeField] private Transform ExitPointCustomer;

    [SerializeField] private List<MainMachine> mainMachinesList;
    public SO_LVL levelSo;

    private void Awake()
    {
        Instance = this;
    }

    public MainMachine GetMainMachine(string itemName)
    {
        for (int i = 0; i < mainMachinesList.Count; i++)
        {
            if (mainMachinesList[i].GetItemName().ToString() == itemName)
            {
                return mainMachinesList[i];
            }
        }
        return null;
    }

    public List<MainMachine> GetMainMachineList() => mainMachinesList;

    public Transform GetCustomerExitpoint()
    {
        return ExitPointCustomer;
    }

    public void NextLevelPossible()
    {
        if (UIManager.Instance.nextLevelArrow == null) return;
        int counter = 0;
        for(int i=0; i<mainMachinesList.Count; i++)
        {
            if(mainMachinesList[i].machineLevel == levelSo.machineMaxLevel)
            {
                counter++;
            }
        }

        if(counter == mainMachinesList.Count)
        {
            if(UIManager.Instance.nextStallPice < EconomyManager.Instance.GetTotalCoins())
            {
                UIManager.Instance.nextLevelArrow.gameObject.SetActive(true);
            }
        }
        else
        {
            UIManager.Instance.nextLevelArrow.gameObject.SetActive(false);
        }
    }
}
