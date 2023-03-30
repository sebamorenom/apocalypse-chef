using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class Director : MonoBehaviour, ISaveable
{
    public static Director director;
    private GameInfo[] gameInfos;
    public GameInfo currentGameInfo;

    public UIManager uiManager;

    public ZombieSpawnerManager zSpawnManager;

    public bool forceDayEnd;

    public int currentFileIndex;

    public delegate void StartDay();

    public delegate void EndDay();

    public event StartDay startDay;
    public event EndDay endDay;

    private void Awake()
    {
        if (director == null)
        {
            director = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (uiManager.dayNeedsToEnd && endDay != null)
        {
            endDay.Invoke();
        }*/

        if (forceDayEnd)
        {
            endDay.Invoke();
            forceDayEnd = false;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            if (SaveSystem.Load(currentFileIndex))
                return;
            else
            {
                SaveSystem.Save(currentFileIndex);
            }

            zSpawnManager.FindZombieSpawners();
        }
        else
        {
            if (scene.buildIndex == 2)
            {
                SaveSystem.Save(currentFileIndex);
            }

            zSpawnManager.RemoveZombieSpawners();
        }
    }

    public void Save(int fileSaveIndex)
    {
        currentFileIndex = fileSaveIndex;
    }

    public void SetGameInfo()
    {
        currentGameInfo = gameInfos[currentFileIndex];
    }

    public void PrepareZombieSpawnManager()
    {
        zSpawnManager = FindObjectOfType<ZombieSpawnerManager>();
        zSpawnManager.totalPoints = Mathf.CeilToInt(zSpawnManager.startingPoints *
                                                    (zSpawnManager.pointScalingModifier *
                                                     currentGameInfo.currentDay));
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

    [System.Serializable]
    struct DirectorSaveInfo
    {
        public int currentDay;
        public int currentMoney;
        public int totalScore;
    }
}