using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradePackingCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Image fillImage;
    [SerializeField] private int currentLevel;

    [SerializeField] private Image imageOne, imageTwo;

    [SerializeField] private List<UpgradeLevel> upgradeLevelsList;

    private Coroutine deductMoneyCoroutine;
    [SerializeField] private int totalAmount;
    [SerializeField] private int remaingAmount;
    

    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private int machineMaxLevel;
    [SerializeField] private PackFood packFood;

    [SerializeField] private PlayerController playerController;

    private void Start()
    {

        GetMachineLevelDetails();
        displayText.color = Color.white;
        displayText.fontSize = 1.5f;
        UpgradeFromPref();
    }

    public void GetMachineLevelDetails()
    {
        for (int i = 0; i < upgradeLevelsList.Count; i++)
        {
            if (i == currentLevel && upgradeLevelsList[i].dueAmount == 0)
            {
                totalAmount = upgradeLevelsList[i].upgradeCost;
                remaingAmount = upgradeLevelsList[i].upgradeCost;
                upgradeLevelsList[i].dueAmount = 1;
                SetDisplayText();
                if (upgradeLevelsList[i].upgradeImage1 != null)
                {
                    imageOne.enabled = true;
                    imageOne.sprite = upgradeLevelsList[i].upgradeImage1;
                }
                else
                {
                    imageOne.enabled = false;
                }

                if (upgradeLevelsList[i].upgradeImage2 != null)
                {
                    imageTwo.enabled = true;
                    imageTwo.sprite = upgradeLevelsList[i].upgradeImage2;
                }
                else
                {
                    imageTwo.enabled = false;
                }


                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null) 
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (playerController == null) return;
        if (!other.gameObject.CompareTag("Player")) return;
        if (deductMoneyCoroutine == null) deductMoneyCoroutine = StartCoroutine(DeductMoney(other));
    }

    IEnumerator DeductMoney(Collider other)
    {
       
        upgradeCanvas.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1f);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < upgradeLevelsList.Count; i++)
        {
            if (i == currentLevel && upgradeLevelsList[i].dueAmount == 0)
            {
                totalAmount = upgradeLevelsList[i].upgradeCost;
                remaingAmount = upgradeLevelsList[i].upgradeCost;
                upgradeLevelsList[i].dueAmount = 1;
                SetDisplayText();
                if (upgradeLevelsList[i].upgradeImage1 != null)
                {
                    imageOne.enabled = true;
                    imageOne.sprite = upgradeLevelsList[i].upgradeImage1;
                }
                else
                {
                    imageOne.enabled = false;
                }

                if (upgradeLevelsList[i].upgradeImage2 != null)
                {
                    imageTwo.enabled = true;
                    imageTwo.sprite = upgradeLevelsList[i].upgradeImage2;
                }
                else
                {
                    imageTwo.enabled = false;
                }
                break;
            }
        }

        if (EconomyManager.Instance.GetTotalCoins() > remaingAmount)
        {

            while (true)
            {
                float calculateFifty = (float)totalAmount / 20f;
                yield return new WaitForSeconds(0.06f);
                int roundedPrice = Mathf.RoundToInt(calculateFifty) + 2;
                if (remaingAmount <= roundedPrice)
                {
                    EconomyManager.Instance.ReduceTotalCoins(remaingAmount);
                    remaingAmount -= remaingAmount;
                    SetDisplayText();
                    FillImageAccToPayment();
                    UpgradeComplete();
                    StopCoroutineNow();
                    yield break;
                }
                else
                {
                    EconomyManager.Instance.ReduceTotalCoins(roundedPrice);
                    remaingAmount -= roundedPrice;
                    SetDisplayText();
                    FillImageAccToPayment();
                }
            }
        }

        StopCoroutineNow();

    }

    private void StopCoroutineNow()
    {
        if (deductMoneyCoroutine != null)
        {
            StopCoroutine(deductMoneyCoroutine);
            deductMoneyCoroutine = null;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        playerController = null;
        StopCoroutineNow();
        upgradeCanvas.transform.DOScale(new Vector3(1f, 1f, 1f), 1.25f);
    }

    private void SetDisplayText()
    {
        displayText.text = remaingAmount.ToString();
    }

    public void FillImageAccToPayment()
    {
        float amountToFill = (float)remaingAmount / totalAmount;

        fillImage.fillAmount = (1 - amountToFill);
    }


    public void UpgradeFromPref()
    {
        int x = GetPref();
        for (int i = 0; i < x; i++) UpgradeComplete();
    }

    public void UpgradeComplete()
    {
        if (currentLevel == 0)
        {
            packFood.ReducePackingTime();
            packFood.IncreaseMaxStackHeight(3);
            fillImage.fillAmount = 0;
            upgradeCanvas.transform.DOScale(Vector3.one, 0.2f);
            SetPref(1);
        }

        if (currentLevel == 1)
        {
            packFood.IncreaseMaxStackHeight(2);
            packFood.ReducePackingTime();
            fillImage.fillAmount = 0;
            upgradeCanvas.transform.DOScale(Vector3.one, 0.2f);
            SetPref(2);
        }

        if (currentLevel == 2)
        {
            packFood.IncreaseMaxStackHeight(3);
            packFood.ReducePackingTime();
            fillImage.fillAmount = 0;
            upgradeCanvas.transform.DOScale(Vector3.one, 0.2f);
            SetPref(3);
        }

        if (currentLevel == 3)
        {
            packFood.IncreaseMaxStackHeight(2);
            packFood.ReducePackingTime();
            fillImage.fillAmount = 0;
            upgradeCanvas.transform.DOScale(Vector3.one, 0.2f);
            SetPref(4);
        }
        if (currentLevel == machineMaxLevel)
        {
            upgradeCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
        currentLevel++;
        GetMachineLevelDetails();
    }

    private void SetPref(int x)
    {
        PlayerPrefs.SetInt(gameObject.name + SceneManager.GetActiveScene().name,x);
    }

    private int GetPref()
    {
        return PlayerPrefs.GetInt(gameObject.name + SceneManager.GetActiveScene().name);
    }

}

