using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public struct ZombieKills
{
    public int normalZombies;
    public int specialZombies;
    public int totalZombieKills;
}

[CreateAssetMenu(menuName = "GameInfo")]
public class GameInfo : ScriptableObject
{
    [SerializeField] public int id;

    [DoNotSerialize] [ReadOnly] public List<Transform> objectivesTransform;
    [DoNotSerialize] [ReadOnly] public List<Health> objectivesHealth;

    [SerializeField] public int currentDay;

    [SerializeField] public DifficultySettings difficultySettings;

    [SerializeField] public int currentDayScore = 0;

    [SerializeField] public int totalScore = 0;

    [SerializeField] public ZombieKills zombieKills = new ZombieKills();
    [SerializeField] public FoodValues foodValues;


    public void EndDay()
    {
        totalScore += currentDayScore;
    }

    public void CreateSave()
    {
        ResetFile();
    }

    public void CreateArrays()
    {
        objectivesTransform = new List<Transform>();
        objectivesHealth = new List<Health>();
    }

    public void DeleteSave(GameInfo gInfo)
    {
        var path = AssetDatabase.GetAssetPath(gInfo);
        AssetDatabase.DeleteAsset(path);
    }

    private void ResetFile()
    {
        currentDay = 0;
        zombieKills.normalZombies = 0;
        zombieKills.specialZombies = 0;
        zombieKills.totalZombieKills = 0;
    }

    public void RemoveFromArrays(Transform objTransform, Health objHealth)
    {
        objectivesTransform.Remove(objTransform);
        objectivesHealth.Remove(objHealth);
    }
}