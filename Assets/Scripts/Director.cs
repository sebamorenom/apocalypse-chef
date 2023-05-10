using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


[RequireComponent(typeof(SceneChanger))]
public class Director : MonoBehaviour, ISaveable
{
    [HideInInspector] public static Director director;
    [SerializeField] private GameInfo[] gameInfos;
    public GameInfo currentGameInfo;

    [SerializeField] public float scoreToMoneyModifier;

    private SceneChanger _sceneChanger;
    private Fader _fader;

    public UIManager uiManager;
    public ZombieSpawnerManager zSpawnManager;
    public OrderShower orderShower;

    public UpgradeManager upgradeManager;

    public bool forceDayEnd;

    public int currentFileIndex;

    public delegate void OnLoad();

    public delegate void StartDay();

    public delegate void EndDay();

    public event OnLoad onLoad;
    public event StartDay startDay;
    public event EndDay endDay;

    private void Awake()
    {
        if (director == null)
        {
            director = this;
            _fader = GetComponent<Fader>();
            _sceneChanger = GetComponent<SceneChanger>();
            DontDestroyOnLoad(gameObject);
            onLoad += Save;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager != null)
        {
            zSpawnManager.CanSpawn(uiManager.canSpawnZombies);
            if (uiManager.dayNeedsToEnd)
            {
                ToUpgradeScene();
            }

            if (forceDayEnd)
            {
                forceDayEnd = false;
                ToUpgradeScene();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            if (scene.buildIndex == 1)
            {
                SaveSystem.Load(currentFileIndex);
                onLoad.Invoke();
                StartCoroutine(PrepareSceneManagers());
            }
            else
            {
                if (scene.buildIndex == 2)
                {
                    SaveSystem.Load(currentFileIndex);
                    onLoad.Invoke();
                    StartCoroutine(PrepareUpgradeSceneManagers());
                }

                ClearFields();
            }
        }
        else
        {
            ClearFields();
        }
    }


    private void Save()
    {
        SaveSystem.Save(currentFileIndex);
    }


    private void LoadFields()
    {
        zSpawnManager.InitializeForDay(currentGameInfo.currentDay);
        uiManager.gameInfo = currentGameInfo;
        uiManager.scoreThresholds = currentGameInfo.scoreThresholds;
    }

    private void ClearFields()
    {
        zSpawnManager.CanSpawn(false);
        uiManager = null;
    }

    public object CaptureState()
    {
        DirectorSaveInfo dirSaveInfo = new DirectorSaveInfo();
        dirSaveInfo.currentMoney = currentGameInfo.currentMoney;
        dirSaveInfo.currentDay = currentGameInfo.currentDay;
        dirSaveInfo.totalScore = currentGameInfo.totalScore;
        return dirSaveInfo;
    }

    public void LoadState(object state)
    {
        DirectorSaveInfo dirLoadInfo = (DirectorSaveInfo)state;
        currentGameInfo.currentMoney = dirLoadInfo.currentMoney;
        currentGameInfo.currentDay = dirLoadInfo.currentDay;
        currentGameInfo.totalScore = dirLoadInfo.totalScore;
    }

    private IEnumerator PrepareSceneManagers()
    {
        while (zSpawnManager == null && orderShower == null && uiManager == null)
        {
            yield return null;
        }

        zSpawnManager.currentGameInfo = currentGameInfo;
        zSpawnManager.SetGameInfoToZombies();
        LoadFields();
    }

    private IEnumerator PrepareUpgradeSceneManagers()
    {
        while (upgradeManager != null)
        {
            yield return null;
        }
    }

    private void ToUpgradeScene()
    {
        ScoreToMoney();
        _sceneChanger.ChangeScene(2);
    }

    private void ScoreToMoney()
    {
        currentGameInfo.currentMoney += Mathf.RoundToInt(currentGameInfo.currentDayScore * scoreToMoneyModifier);
        currentGameInfo.currentDayScore = 0;
    }

    public void SelectSaveFile(int saveIndex)
    {
        currentGameInfo = gameInfos[saveIndex];
        currentFileIndex = saveIndex;
    }


    [System.Serializable]
    struct DirectorSaveInfo
    {
        public int currentDay;
        public int currentMoney;
        public int totalScore;
    }
}