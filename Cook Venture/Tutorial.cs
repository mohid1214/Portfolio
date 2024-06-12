using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private RectTransform upgradesRectTransform;
    [SerializeField] private RectTransform nextStageRectTransform;

    [Header("GmaeObjects")]
    [SerializeField] private GameObject lemonadeMachine;
    [SerializeField] private GameObject cashier;
    [SerializeField] private GameObject firstMachine;


    [Header("Panles")]
    [SerializeField] private GameObject upgradeMachinePane;
    [SerializeField] private GameObject upgradesPanel;
    [SerializeField] private GameObject nextStagePanel;

    [SerializeField] private TextMeshProUGUI text;

    public GameObject achievementsButton, settingsButtons;

    private Tween newtween;

    private void Awake()
    {
        Instance = this;
        achievementsButton.SetActive(false);
        settingsButtons.SetActive(false);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("tutorial") == 1)
        {
            achievementsButton.SetActive(true);
            settingsButtons.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            if (!PlayerPrefs.HasKey("sceneName"))
            {
                IntroductionLemonadeUpgrade(firstMachine);
            }
        }
        
            
    }

    public void IntroductionLemonadeUpgrade(GameObject targetObject)
    {
        arrowRectTransform.gameObject.SetActive(true);
        TurnOnText();
        text.text = "tap here to unlock Machine!";
        Vector3 worldPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position);
        arrowRectTransform.DOMove(worldPosition + new Vector3(0f, 100f, 0f), 0.5f).OnComplete(() => 
        {
            newtween = arrowRectTransform.DOPunchPosition(new Vector3(0f, 20f, 0f), 0.5f, 0, 0f).SetLoops(10).SetEase(Ease.Linear);
        });
        PlayerPrefs.SetInt("tutorial", 1);
    }

    public void TurnArrowOff()
    {
        newtween.Kill();
        arrowRectTransform.gameObject.SetActive(false);
    }


    public void MakeTextNull()
    {
        TurnOffText();
        text.text = "";
    }

    public GameObject GetMachineUpgrade() => lemonadeMachine;

    public void ShowLemonadeUpgrade()
    {
        arrowRectTransform.gameObject.SetActive(true);
        TurnOnText();
        text.text = "tap here to increase profits!";
        Vector3 worldPosition = Camera.main.WorldToScreenPoint(lemonadeMachine.transform.position);
        arrowRectTransform.DOMove(worldPosition + new Vector3(-50f, 100f, 0f), 0.5f).OnComplete(() =>
        {
            newtween = arrowRectTransform.DOPunchPosition(new Vector3(0f, 20f, 0f), 0.5f, 0, 0f).SetLoops(10).SetEase(Ease.Linear);
        });
    }

    public void ShowUpgrades()
    {
        arrowRectTransform.gameObject.SetActive(true);
        TurnOnText();
        text.text = "Tap Here To Upgrade Machines and Cashiers!";
        arrowRectTransform.DOMove(upgradesRectTransform.position + new Vector3(-30f, 150f, 0f), 0.5f).OnComplete(() =>
        {
            newtween = arrowRectTransform.DOPunchPosition(new Vector3(0f, 20f, 0f), 0.5f, 0, 0f).SetLoops(10).SetEase(Ease.Linear);
        });
    }

    public void ShowNextLevel()
    {
        arrowRectTransform.gameObject.SetActive(true);
        TurnOnText();
        text.text = "Tap Here To Go To Next Stage";
        arrowRectTransform.DOMove(nextStageRectTransform.position + new Vector3(40f, 150f, 0f), 0.5f).OnComplete(() =>
        {
            newtween = arrowRectTransform.DOPunchPosition(new Vector3(0f, 20f, 0f), 0.5f, 0, 0f).SetLoops(10).SetEase(Ease.Linear);
        });
    }

    public void TurnOnText()
    {
        text.enabled = true;
    }

    public void TurnOffText()
    {
        text.enabled=false;
    }

    public void TurnOnAchivementAndSettingsButton()
    {
        achievementsButton.SetActive(true);
        settingsButtons.SetActive(true);
    }

}
