using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public string playerName;

    [SerializeField] public Transform playerTransform;

    [SerializeField] public int currentLevel;

    [SerializeField] public DifficultySettings difficultySettings;

    [SerializeField] public float currentDayScore;

    [SerializeField] public float totalScore;

    [SerializeField] public ZombieKills zombieKills = new ZombieKills();
    

    public void EndDay()
    {
        totalScore += currentDayScore;
    }
}