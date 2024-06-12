using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using GameAnalyticsSDK;

public class UIManager : MonoBehaviour
{

    public static Action DeletePref;


    [SerializeField] private Button achievementsCloseButton;
    [SerializeField] private Button achievementsOpenButton;
    [SerializeField] private GameObject achievements;

    [SerializeField] private Button powerUpsCloseButton, powerUpsOpenButton;
    [SerializeField] private GameObject upgradeMenu;
    public static UIManager Instance { get; private set; }

    public Color realColor, greyColor;
    [SerializeField] private Button nextLevelClose, upgradeToNextLevel, nextLevelOpen;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText, upgradeAllMachines;
    [SerializeField] private GameObject stallUpgradeMenu;
    [SerializeField] private SO_LVL levelSo;
    public long nextStallPice;
    public  Image nextLevelArrow;

    [Header("offline Panel")]
    public GameObject offlinePanel;
    public Slider offlineSlider;
    public Button offlineCloseButton;
    public TextMeshProUGUI rewardsAmount;
    public TextMeshProUGUI timebytime;

    [Header("settingsButton")]
    public Button soundOnButton,soundOffButton,musicOnButton,musicOffButton,privacyPolicyButton,vibrationButtonOn,VibrationButtonOff, settingsButton,settingsclosebutton;
    public GameObject settingsMenu;
    
    private void Awake()
    {
        Instance = this;
        settingsButton.onClick.AddListener(OnSettingsButtonDown);
        soundOnButton.onClick.AddListener(OnSoundOnButtonDown);
        soundOffButton.onClick.AddListener(OnSoundOffButtonDown);
        musicOnButton.onClick.AddListener(OnMusicOnButtonDown);
        musicOffButton.onClick.AddListener(OnMusicOffButtonDown);
        vibrationButtonOn.onClick.AddListener(OnVibrationOnButtonDown);
        VibrationButtonOff.onClick.AddListener(OnVibrationOffButtonDown);
        settingsclosebutton.onClick.AddListener(OnSettingsCloseButtonDown);
        privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonDown);

        offlineCloseButton.onClick.AddListener(OnOfflineCloseButtonDown);
        powerUpsCloseButton.onClick.AddListener(OnCloseButtonDown);
        powerUpsOpenButton.onClick.AddListener(OnOpenButtonDown);
        nextLevelClose.onClick.AddListener(OnStallUpgradeCloseButtonDown);
        nextLevelOpen.onClick.AddListener(OnStallUIOpenButtonDown);


        achievementsCloseButton.onClick.AddListener(OnAchivementsCloseButtonDown);
        achievementsOpenButton.onClick.AddListener(OnAchievementsOpenButtonDown);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("exitTime"))
        {
            offlineSlider.maxValue = 3 * 3600;
            offlinePanel.SetActive(true);
            DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString("exitTime"));
            DateTime currentTime = DateTime.Now;

            TimeSpan timeAway = currentTime - lastTime;

           
            long timeInSeconds = TimeDifferenceInSeconds(lastTime, currentTime);

            if(timeInSeconds >= offlineSlider.maxValue)
            {
                offlineSlider.value = offlineSlider.maxValue;
                long maxValue = (long)offlineSlider.maxValue;
                EconomyManager.Instance.AddCoins(GetRewardAmount(maxValue));
                rewardsAmount.text = FormatNumber(GetRewardAmount(maxValue));
                timebytime.text = SecondsToHours(maxValue).ToString() + "/3H";
            }
            else
            {
                offlineSlider.value = timeInSeconds;
                EconomyManager.Instance.AddCoins(GetRewardAmount(timeInSeconds));
                rewardsAmount.text = FormatNumber(GetRewardAmount(timeInSeconds));
                timebytime.text = SecondsToHours(timeInSeconds).ToString() + "/3H";
            }

           
            PlayerPrefs.DeleteKey("exitTime");
        }
        else
        {
            offlinePanel.SetActive(false);
        }
    }

    public void OnAchivementsCloseButtonDown()
    {
        achievements.transform.DOScale(Vector3.zero,1);
    }
    public void OnAchievementsOpenButtonDown()
    {
        for(int i=0; i<GameManager.Instance.GetMainMachineList().Count; i++)
        {
            GameManager.Instance.GetMainMachineList()[i].CloseButtonDownSeperate();
        }
        achievements.transform.DOScale(Vector3.one, 1);
    }
    public float SecondsToHours(long seconds)
    {
        float hours = (float)seconds / 3600f;
        string formattedHours = hours.ToString("0.0"); // Format to one decimal place
        return float.Parse(formattedHours); // Convert the formatted string back to a float
    }



    public long GetRewardAmount(long seconds)
    {
        int machineLevel = GameManager.Instance.GetMainMachineList()[0].machineLevel;
        if(machineLevel < 10)
        {
            return 1 * seconds;
        }
        if(machineLevel >= 10 && machineLevel < 25)
        {
            return 4 * seconds;
        }
        if(machineLevel >= 25 && machineLevel < 50)
        {
            return 15 * seconds;
        }
        if(machineLevel>=50  && machineLevel < 75)
        {
            return 40 * seconds;
        }

        return 0;
    }



    public  int TimeDifferenceInSeconds(DateTime startTime, DateTime endTime)
    {
        TimeSpan timeDifference = endTime - startTime;
        return (int)timeDifference.TotalSeconds;
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

    public void OnOfflineCloseButtonDown()
    {
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
        offlinePanel.SetActive(false);
    }

    public void OnCloseButtonDown()
    {
        if (Tutorial.Instance != null)
        {
            Tutorial.Instance.ShowNextLevel();
        }
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
        upgradeMenu.SetActive(false);
    }

    public void OnOpenButtonDown()
    {
        if(Tutorial.Instance != null)
        {
            Tutorial.Instance.TurnArrowOff();
            Tutorial.Instance.MakeTextNull();
        }
        for (int i = 0; i < GameManager.Instance.GetMainMachineList().Count; i++)
        {
            GameManager.Instance.GetMainMachineList()[i].CloseButtonDownSeperate();
        }
        SoundManager.Instance.PlaySelectionVibration();
        RectTransform rectTransform = upgradeMenu.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        upgradeMenu.SetActive(true);
        rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.15f).OnComplete(() => rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
        settingsMenu.SetActive(false);
        stallUpgradeMenu.SetActive(false);
        SoundManager.Instance.OnButtonDownSound();
    }

    public void OnStallUIOpenButtonDown()
    {
        if(Tutorial.Instance != null)
        {
            Tutorial.Instance.TurnArrowOff();
            Tutorial.Instance.MakeTextNull();
            
        }
        for (int i = 0; i < GameManager.Instance.GetMainMachineList().Count; i++)
        {
            GameManager.Instance.GetMainMachineList()[i].CloseButtonDownSeperate();
        }
        SoundManager.Instance.PlaySelectionVibration();
        RectTransform rectTransform = stallUpgradeMenu.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.15f).OnComplete(() => rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
        stallUpgradeMenu.SetActive(true);
        upgradeMenu.SetActive(false);
        settingsMenu.SetActive(false);
        SoundManager.Instance.OnButtonDownSound();
        int counter = 0;
        for (int i = 0; i < GameManager.Instance.GetMainMachineList().Count; i++)
        {
            if (GameManager.Instance.GetMainMachineList()[i].GetMachineLevel() == levelSo.machineMaxLevel)
            {
                counter++;
            }
        }

        if (counter == GameManager.Instance.GetMainMachineList().Count)
        {
            upgradeAllMachines.enabled = false;
            upgradeToNextLevel.interactable = true;
            buttonText.text = nextStallPice.ToString();
            if (EconomyManager.Instance.GetTotalCoins() > nextStallPice)
            {
                buttonImage.color = realColor;
            }
            else
            {
                buttonImage.color = greyColor;
            }

        }
        else
        {
            upgradeAllMachines.enabled = true;
            upgradeAllMachines.text = "upgrade all machine to level  " + levelSo.machineMaxLevel;
            upgradeToNextLevel.interactable = false;
            buttonImage.color = greyColor;
        }

        
    }

    public void OnStallUpgradeCloseButtonDown()
    {
       
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
        stallUpgradeMenu.SetActive(false);
        Tutorial.Instance.TurnOnAchivementAndSettingsButton();
        Tutorial.Instance.gameObject.SetActive(false);
    }

    


    public void loadLevel2()
    {
        if (EconomyManager.Instance.GetTotalCoins() > nextStallPice)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete , SceneManager.GetActiveScene().name.ToString());
            PlayerPrefs.DeleteAll();
            DeletePrefKeys();
            PlayerPrefs.SetInt("policy", 1);
            PlayerPrefs.SetString("sceneName", "SumyalGameScene 2");
            SceneManager.LoadScene("SumyalGameScene 2");
        }
    }


    public void loadlevel3()
    {
        if (EconomyManager.Instance.GetTotalCoins() > nextStallPice)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, SceneManager.GetActiveScene().name.ToString());
            PlayerPrefs.DeleteAll();
            DeletePrefKeys();
            PlayerPrefs.SetInt("policy", 1);
            string sceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("sceneName", "SumyalGameScene 3");
            SceneManager.LoadScene("SumyalGameScene 3");
        }
    }
    public void loadlevel4()
    {
        if (EconomyManager.Instance.GetTotalCoins() > nextStallPice)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, SceneManager.GetActiveScene().name.ToString());
            PlayerPrefs.DeleteAll();
            DeletePrefKeys();
            PlayerPrefs.SetInt("policy", 1);
            string sceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("sceneName", "SumyalGameScene 4");
            SceneManager.LoadScene("SumyalGameScene 4");
        }
    }

    public void DeletePrefKeys()
    {
        UIManager.DeletePref?.Invoke();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
    }

    public void OnSettingsButtonDown()
    {
        for (int i = 0; i < GameManager.Instance.GetMainMachineList().Count; i++)
        {
            GameManager.Instance.GetMainMachineList()[i].CloseButtonDownSeperate();
        }
        RectTransform rectTransform = settingsMenu.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.15f).OnComplete(() => rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
        settingsMenu.SetActive(true);
        upgradeMenu.SetActive(false);
        stallUpgradeMenu.SetActive(false);
    }

    public void OnMusicOffButtonDown()
    {
        SoundManager.Instance.PlayBgSound();
        musicOnButton.image.enabled = true;
        musicOffButton.image.enabled = false;
        musicOnButton.interactable = true;
        musicOffButton.interactable = false;
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();

    }

    public void OnMusicOnButtonDown()
    {
        SoundManager.Instance.StopBgSound();
        musicOnButton.image.enabled = false;
        musicOffButton.image.enabled = true;
        musicOnButton.interactable = false;
        musicOffButton.interactable = true;
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
    }

    public void OnSoundOnButtonDown()
    {
        SoundManager.Instance.stopSound = true;
        soundOnButton.image.enabled = false;
        soundOffButton.image.enabled = true;
        soundOnButton.interactable = false;
        soundOffButton.interactable = true;
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
    }

    public void OnSoundOffButtonDown()
    {
        SoundManager.Instance.stopSound = false;
        soundOnButton.image.enabled = true;
        soundOffButton.image.enabled = false;
        soundOnButton.interactable = true;
        soundOffButton.interactable = false;
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();

    }

    public void OnVibrationOnButtonDown()
    {
        SoundManager.Instance.stopVibration = true;
        vibrationButtonOn.image.enabled = false;
        VibrationButtonOff.image.enabled = true;
        vibrationButtonOn.interactable = false;
        VibrationButtonOff.interactable = true;
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
    }

    public void OnVibrationOffButtonDown()
    {
        SoundManager.Instance.stopVibration = false;
        vibrationButtonOn.image.enabled = true;
        VibrationButtonOff.image.enabled = false;
        vibrationButtonOn.interactable = true;
        VibrationButtonOff.interactable = false;
        
    }

    public void OnSettingsCloseButtonDown() 
    {
        settingsMenu.SetActive(false);
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
    } 

    public void PrivacyPolicyButtonDown()
    {
        SoundManager.Instance.PlaySelectionVibration();
        SoundManager.Instance.OnButtonDownSound();
        Application.OpenURL("https://sites.google.com/view/cookventure-privacy-policy/home");
    }

    public void TurnOffAllMenus()
    {
        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        stallUpgradeMenu.SetActive(false);
    }

}
