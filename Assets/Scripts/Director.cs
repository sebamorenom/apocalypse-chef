using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director director;
    private GameInfo gameInfo;

    private string defaultFile = "/GameInfo/GameInfo1";

    public UIManager uiManager;

    public ZombieSpawnerManager zSpawnManager;

    public bool forceDayEnd;

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

        defaultFile = Application.dataPath + defaultFile;
        gameInfo = AssetDatabase.LoadAssetAtPath<GameInfo>(defaultFile);
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
    }


    public void SetGameInfo(GameInfo gInfo)
    {
        gameInfo = gInfo;
    }

    public void PrepareZombieSpawnManager()
    {
        zSpawnManager.totalPoints = Mathf.CeilToInt(gameInfo.difficultySettings.startingPoints *
                                                    (gameInfo.difficultySettings.pointScalingModifier *
                                                     gameInfo.currentDay));
    }
}