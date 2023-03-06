using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] public Transform[] objectivesTransform;
    [SerializeField] public Health[] objectivesHealth;

    [SerializeField] public int currentLevel;

    [SerializeField] public DifficultySettings difficultySettings;

    [SerializeField] public int currentDayScore = 0;

    [SerializeField] public int totalScore = 0;

    [SerializeField] public ZombieKills zombieKills = new ZombieKills();


    public void EndDay()
    {
        totalScore += currentDayScore;
    }

    public void CreateSave()
    {
        ResetFile();
    }

    public void DeleteSave(GameInfo gInfo)
    {
        var path = AssetDatabase.GetAssetPath(gInfo);
        AssetDatabase.DeleteAsset(path);
    }

    private void ResetFile()
    {
        currentLevel = 0;
        zombieKills.normalZombies = 0;
        zombieKills.specialZombies = 0;
        zombieKills.totalZombieKills = 0;
    }

    public void ResizeObjectivesArray()
    {
        objectivesTransform = objectivesTransform.Where(x => !x.IsUnityNull()).ToArray();
    }

    private void OnValidate()
    {
        for (int i = 0; i < objectivesTransform.Length; i++)
        {
            objectivesHealth[i] = objectivesTransform[i].GetComponent<Health>();
        }
    }
}