using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnlockMachines : MonoBehaviour
{
    public List<MachineDetails> machineDetalsList;
    public static UnlockMachines Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI notEnoughCoins;
    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < machineDetalsList.Count; i++)
        {
            int currentIndex = i;
            machineDetalsList[currentIndex].unlockButton.onClick.AddListener(() => OnUnlockButtonDown(machineDetalsList[currentIndex].mainMachine, machineDetalsList[currentIndex].UnlockGameObject, machineDetalsList[currentIndex].displayTable));
            if (machineDetalsList[i].mainMachine.machineUnlocked)
            {
                machineDetalsList[i].mainMachine.enabled = true;
                machineDetalsList[i].displayTable.SetActive(true);
                machineDetalsList[i].UnlockGameObject.SetActive(false);
            }
            else
            {
                machineDetalsList[i].mainMachine.enabled = false;
                machineDetalsList[i].displayTable.SetActive(false);
                machineDetalsList[i].UnlockGameObject.SetActive(true);
            }
        }
    }


    private void Start()
    {
        UnlockMachineArrow();
    }
    public void UnlockMachineArrow()
    {
        for(int k=0; k<machineDetalsList.Count; k++)
        {
            if(machineDetalsList[k].mainMachine.name == PlayerPrefs.GetString(machineDetalsList[k].mainMachine.name + "isunlock"))
            {
                machineDetalsList[k].mainMachine.enabled = true;
                machineDetalsList[k].mainMachine.machineUnlocked = true;
                machineDetalsList[k].displayTable.SetActive(true);
                machineDetalsList[k].UnlockGameObject.SetActive(false);
            }
        }

        for(int i=0; i<machineDetalsList.Count; i++)
        {
            if (!machineDetalsList[i].mainMachine.enabled)
            {
                if(machineDetalsList[i].mainMachine.UnlockPrice < EconomyManager.Instance.GetTotalCoins())
                {
                    machineDetalsList[i].uparrow.gameObject.SetActive(true);
                }
                else
                {
                    machineDetalsList[i].uparrow.gameObject.SetActive(false);
                }
            }
            else
            {
                machineDetalsList[i].uparrow.gameObject.SetActive(false);
            }
        }

    }



    public void OnUnlockButtonDown(MainMachine mainMachine, GameObject unlockCanvas, GameObject table)
    {
        if(EconomyManager.Instance.GetTotalCoins() >= mainMachine.UnlockPrice)
        {
            EconomyManager.Instance.DecreaseCoins(mainMachine.UnlockPrice);
            PlayerPrefs.SetString(mainMachine.name + "isunlock", mainMachine.name);
            table.SetActive(true);
            mainMachine.enabled = true;
            unlockCanvas.SetActive(false);
            mainMachine.machineUnlocked = true;
        }
        else
        {
            SetTextNotEnoughCoins();
        }
    }

    public void SetTextNotEnoughCoins()
    {
        notEnoughCoins.gameObject.SetActive(true);
        notEnoughCoins.text = "Not Enough Coins!";
        notEnoughCoins.transform.localScale = Vector3.zero;
        notEnoughCoins.transform.DOScale(Vector3.one, 1f).OnComplete(() => 
        {
            notEnoughCoins.transform.DOScale(Vector3.zero, 1f).OnComplete(() => {
                notEnoughCoins.gameObject.SetActive(false);
            });
        });

    }

}

[System.Serializable]
public class MachineDetails
{
    public MainMachine mainMachine;
    public GameObject displayTable;
    public GameObject UnlockGameObject;
    public Button unlockButton;
    public Image uparrow;
}
