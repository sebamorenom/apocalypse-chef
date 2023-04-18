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
    private GameInfo[] gameInfos;
    public GameInfo currentGameInfo;


    private SceneChanger _sceneChanger;
    private Fader _fader;

    public UIManager uiManager;
    public ZombieSpawnerManager zSpawnManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager != null)
        {
            zSpawnManager.CanSpawn(uiManager.canSpawnZombies);
            if (uiManager.dayNeedsToEnd && endDay != null)
            {
                ToUpgradeScene();
            }

            if (forceDayEnd)
            {
                forceDayEnd = false;
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            if (scene.buildIndex == 1)
            {
                SaveSystem.Load(currentFileIndex);
                onLoad.Invoke();
                StartCoroutine(PrepareZSpawnerManager());
                LoadFields();
            }
            else
            {
                if (scene.buildIndex == 2)
                {
                    onLoad.Invoke();
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
        uiManager = FindObjectOfType<UIManager>();
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

    private IEnumerator PrepareZSpawnerManager()
    {
        yield return new WaitUntil(() => zSpawnManager != null);
    }

    private void ToUpgradeScene()
    {
        _sceneChanger.ChangeScene(2);
    }


    [System.Serializable]
    struct DirectorSaveInfo
    {
        public int currentDay;
        public int currentMoney;
        public int totalScore;
    }
}