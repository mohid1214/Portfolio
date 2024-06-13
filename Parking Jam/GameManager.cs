using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Levels,bossLevels;
    public TextMeshProUGUI levelText,coinsText;
    public GameObject winParticle;

    public List<GameObject> levels;


    public int storingIndexLevel;

    public int currentLevelCars = 4;
    public int activeLevelIndex = 0;
    public int remainingMoves = 0;

    public bool isBossLevel = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void OnEnable()
    {
        RewardedADMOB.SkipLevel += skipLevel;
    }

    private void Start()
    {
        if (GameConstants.restartLevel)
        {
            GameConstants.restartLevel = false;
            LoadNewLevel();
        }
        else
            LoadGame();
        Time.timeScale = 1;
    }


    public void UpdateScoreText()
    {
        coinsText.text = GameConstants.Instance.totalScore.ToString();
    }
    public void LoadGame() 
    {
        GameConstants.isLevelLoaded = true;
        GameConstants.isGameStarted = false;
        GameConstants.isGameFinished = false;
        winParticle.SetActive(false);
        UIManager.Instance.ShowStartPanel();
        coinsText.text = GameConstants.Instance.totalScore.ToString();
        SaveDataProgress.LoadData();
    }
    public void LoadNewLevel()
    {
        
        GameConstants.isLevelLoaded = true;
        GameConstants.isGameStarted = false;
        GameConstants.isGameFinished = false;
        winParticle.SetActive(false);
        UIManager.Instance.ShowGamePanel();
        coinsText.text = GameConstants.Instance.totalScore.ToString();
        LoadNextLevel();
        //StartMode1();
    }

    public void LoadNextLevel()
    {
        SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.buttonclick);
        GameConstants.isGameStarted = true;
        GameConstants.isGameFinished = false;
        GameConstants.isGamePaused = false;
        GameConstants.isLevelFailed = false;
        GameConstants.noOfClearedCars = 0;
        GameConstants.currentScore = 0;
        isBossLevel = false;

        Debug.Log("GameStarted");
        if (GameConstants.Instance.currentLevel == 7)
        {
            isBossLevel = true;
            InterstatialInstance.instance.LoadInterstatialAdNow();
            InterstatialInstance.instance.ShowInterstatialAdNow();
        }
        UIManager.Instance.ShowGamePanel();
        for (int i = 0; i < Levels.transform.childCount; i++)
            Levels.transform.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < bossLevels.transform.childCount; i++)
            bossLevels.transform.GetChild(i).gameObject.SetActive(false);
        if (isBossLevel)
        {
            if (GameConstants.Instance.currentBossLevel < bossLevels.transform.childCount)
            {
                bossLevels.transform.GetChild(GameConstants.Instance.currentBossLevel).gameObject.SetActive(true);
                activeLevelIndex = GameConstants.Instance.currentBossLevel;
            }
            else
            {
                bossLevels.transform.GetChild(bossLevels.transform.childCount - 1).gameObject.SetActive(true);
                activeLevelIndex = bossLevels.transform.childCount - 1;
            }

            currentLevelCars = GameConstants.Instance.bossLevelCars[activeLevelIndex];
            remainingMoves = GameConstants.Instance.bossLevelMoves[activeLevelIndex];
            levelText.text = "Moves Left " + remainingMoves;
        }
        else
        {

            if ((GameConstants.Instance.currentLevel) < Levels.transform.childCount)
            {
                Levels.transform.GetChild((GameConstants.Instance.currentLevel)).gameObject.SetActive(true);
                
            }
            else
            {
                Levels.transform.GetChild(Levels.transform.childCount - 1).gameObject.SetActive(true);
                activeLevelIndex = Levels.transform.childCount - 1;
            }


            levelText.text = "Level " + ((GameConstants.Instance.currentLevel) + 1);
            if (GameConstants.Instance.carsInLevel.Count > (GameConstants.Instance.currentLevel))
                currentLevelCars = GameConstants.Instance.carsInLevel[(GameConstants.Instance.currentLevel)];
            else
                currentLevelCars = GameConstants.Instance.carsInLevel[activeLevelIndex];
        }
        if (GameConstants.Instance.currentLevel > 0)
        {
            UIManager.Instance.ShowBarrierButton();
        }

        if (GameConstants.Instance.currentLevel > 4)
        {
            UIManager.Instance.ShowGrannyButton();
        }
    }

    public void StartMode1(int x) 
    {
        storingIndexLevel = x;
        GameConstants.Instance.currentLevel = x;
        SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.buttonclick);
        GameConstants.isGameStarted = true;
        GameConstants.isGameFinished = false;
        GameConstants.isGamePaused = false;
        GameConstants.isLevelFailed = false;
        GameConstants.noOfClearedCars = 0;
        GameConstants.currentScore = 0;
        isBossLevel = false;

        if (GameConstants.Instance.currentLevel == 7)
            isBossLevel = true;
        UIManager.Instance.ShowGamePanel();
        for(int i=0;i<Levels.transform.childCount;i++)
            Levels.transform.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < bossLevels.transform.childCount; i++)
            bossLevels.transform.GetChild(i).gameObject.SetActive(false);
        if (isBossLevel)
        {
            if (GameConstants.Instance.currentBossLevel < bossLevels.transform.childCount)
            {
                bossLevels.transform.GetChild(GameConstants.Instance.currentBossLevel).gameObject.SetActive(true);
                activeLevelIndex = GameConstants.Instance.currentBossLevel;
            }
            else
            {
                bossLevels.transform.GetChild(bossLevels.transform.childCount - 1).gameObject.SetActive(true);
                activeLevelIndex = bossLevels.transform.childCount - 1;
            }
          
            currentLevelCars = GameConstants.Instance.bossLevelCars[activeLevelIndex];
            remainingMoves = GameConstants.Instance.bossLevelMoves[activeLevelIndex];
            levelText.text = "Moves Left " + remainingMoves;
        }
        else
        {
            if ((x) < Levels.transform.childCount)
            {
                Levels.transform.GetChild((x)).gameObject.SetActive(true);
                activeLevelIndex = x;
            }
            else
            {
                Levels.transform.GetChild(Levels.transform.childCount - 1).gameObject.SetActive(true);
                activeLevelIndex = Levels.transform.childCount - 1;
            }
         

            levelText.text = "Level " + ((x)+1);
            if (GameConstants.Instance.carsInLevel.Count > (x))
                currentLevelCars = GameConstants.Instance.carsInLevel[(x)];
            else
                currentLevelCars = GameConstants.Instance.carsInLevel[activeLevelIndex];
        }
        if (x > 0)
        {
            UIManager.Instance.ShowBarrierButton();
        }

        if (x > 4)
        {
            UIManager.Instance.ShowGrannyButton();
        }
    }

    public void GameFinished() 
    {
        GameConstants.noOfClearedCars += 1;
        if (GameConstants.noOfClearedCars >= currentLevelCars)
        {
            GameConstants.isGameFinished = true;
            winParticle.SetActive(true );
            SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.LevelWin);
            Invoke("FinishUI",3f);
            Debug.Log("GameFinish");
        }

    }
    public void StartHexaScene()
    {
        SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.buttonclick);
        SceneLoader.instance.LoadScene(1);
        //SceneManager.LoadScene(1);
    }
    public void StartLotScene()
    {
        SceneLoader.instance.LoadScene(2);
        SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.buttonclick);
        //SceneManager.LoadScene(2);
    }
    public void CheckLevelFail() 
    {
        if (isBossLevel)
        {
            remainingMoves--;
            levelText.text = "Moves Left " + remainingMoves;
            if (remainingMoves <= 0)
            {
                GameConstants.isLevelFailed = true;
                UIManager.Instance.ShowFailPanel();
                SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.LevelFail);
            }

        }
    }
    private void FinishUI()
    {
        winParticle.SetActive(false);
        UIManager.Instance.ShowCompletePanel();
        if (GameConstants.Instance.currentLevel > 1)
        {
            InterstatialInstance.instance.LoadInterstatialAdNow();
            InterstatialInstance.instance.ShowInterstatialAdNow();
        }
    }
    public void GamePaused()
    {
        GameConstants.isGamePaused =true;
        UIManager.Instance.ShowPausePanel();
    }
    public void GameResumed()
    {
        GameConstants.isGamePaused = false;
        UIManager.Instance.HidePausePanel();
    }
    public void Restart()
    {
        SoundManagerJam.instance.PlayTheAudio(SoundManagerJam.instance.buttonclick);
        SceneLoader.instance.LoadScene(0);
        GameConstants.restartLevel = true;
        //SceneManager.LoadScene(0);
    }
    public void Resume()
    {

        GameConstants.isGamePaused = false;
    }



    public void SkipButtonCall()
    {
        RewardAdInstance.instance.LoadRewardedNow();
        RewardAdInstance.instance.ShowRewardedNow3();
    }

    public void skipLevel()
    {
        if (isBossLevel)
            GameConstants.Instance.currentBossLevel++;

        if (GameConstants.Instance.currentLevel >= Levels.transform.childCount)
        {
            SceneLoader.instance.LoadScene(0);
        }

        GameObject yy = levels[GameConstants.Instance.currentLevel];
        PlayerPrefs.SetInt(yy.gameObject.name, 1);
        GameConstants.Instance.currentLevel++;
        if (GameConstants.Instance.currentLevel < levels.Count)
        {
            GameObject zy = levels[GameConstants.Instance.currentLevel];
            PlayerPrefs.SetInt(zy.gameObject.name, 1);
        }


        GameConstants.Instance.totalScore += GameConstants.currentScore;
        if (GameConstants.Instance.currentLevel < Levels.transform.childCount)
            LoadNewLevel();



    }
    public void NextLevel()
    {
        if (isBossLevel)
            GameConstants.Instance.currentBossLevel++;

        if (GameConstants.Instance.currentLevel >= Levels.transform.childCount)
        {
            SceneLoader.instance.LoadScene(0);
        }

        GameObject yy = levels[GameConstants.Instance.currentLevel];
        PlayerPrefs.SetInt(yy.gameObject.name, 1);
        GameConstants.Instance.currentLevel++;
        if (GameConstants.Instance.currentLevel < levels.Count)
        {
            GameObject zy = levels[GameConstants.Instance.currentLevel];
            PlayerPrefs.SetInt(zy.gameObject.name, 1);
        }
       

        GameConstants.Instance.totalScore += GameConstants.currentScore;
        if (GameConstants.Instance.currentLevel < Levels.transform.childCount)
            LoadNewLevel();



    }
    public void GotoHome()
    {
        SceneLoader.instance.LoadScene(0);
        //SceneManager.LoadScene(0);

    }
    public void UpdateCars(int index) 
    {
        
        {
            GameConstants.Instance.CarTypeIndex = index;
        }
    }

    public void updateSmoke(int index)
    {
        GameConstants.Instance.SmokeTypeIndex = index;
    }


    private void OnDisable()
    {
        RewardedADMOB.SkipLevel -= skipLevel;
    }
}
