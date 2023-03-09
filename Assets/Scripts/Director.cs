using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Director : MonoBehaviour
{
    private GameInfo gameInfo = AssetDatabase.LoadAssetAtPath<GameInfo>(Application.dataPath + "/GameInfo/GameInfo1");

    public UIManager uiManager;

    public ZombieSpawnerManager zSpawnManager;

    public delegate void EndDay();

    public event EndDay endDay;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.dayNeedsToEnd && endDay != null)
        {
            endDay();
        }
    }

    private void StartDay()
    {
        endDay += gameInfo.EndDay;
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