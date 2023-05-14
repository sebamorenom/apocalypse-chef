using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct ZombieKills
{
    public int normalZombies;
    public int specialZombies;
    public int totalZombieKills;
}

[CreateAssetMenu(menuName = "GameInfo")]
[Serializable]
public class GameInfo : ScriptableObject
{
    [SerializeField] public int id;

    [Header("AI Related")] [DoNotSerialize] [ReadOnly]
    public List<Transform> objectivesTransform;

    [DoNotSerialize] [ReadOnly] public List<Health> objectivesHealth;

    [Header("Playtime Related")] [SerializeField]
    public int currentDay;

    [SerializeField] public int currentMoney;

    [SerializeField] public int currentDayScore;

    [SerializeField] public int totalScore;

    [Header("Miscellaneous Info")] [SerializeField]
    public ZombieKills zombieKills = new ZombieKills();

    [SerializeField] public int[] scoreThresholds = new int[5];


    public void EndDay()
    {
        totalScore += currentDayScore;
        currentDayScore = 0;
    }


    public void CreateArrays()
    {
        objectivesTransform = new List<Transform>();
        objectivesHealth = new List<Health>();
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