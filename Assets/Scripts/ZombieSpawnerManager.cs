using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct ZombieCost
{
    public int points;
    public GameObject zombie;
}

public class ZombieSpawnerManager : MonoBehaviour
{
    [SerializeField] public ZombieCost[] zCosts;
    [SerializeField] public List<SpawnPoint> zSpawnPoints;
    [SerializeField] private Vector2 timeVariation = new Vector2(0, 1);

    [HideInInspector] public GameInfo currentGameInfo;

    private float _timeLastSpawn;
    private bool _spawningAllowed;

    public int currentAvailablePoints;
    public float currentTimeBetweenSpawns;

    private float _timeBeforeNextSpawn;


    private int _pointsForZombie;


    private GameObject _zombieToGive;


    private SpawnPoint _chosenSpawnPoint;

    private void Start()
    {
        var dir = FindObjectOfType<Director>();
        dir.zSpawnManager = this;
        InitializeForDay(dir.currentGameInfo.currentDay);
        zSpawnPoints = new List<SpawnPoint>();
        zSpawnPoints.AddRange(GetComponentsInChildren<SpawnPoint>().ToArray());
    }

    public void SetGameInfoToZombies()
    {
        foreach (var zombieCost in zCosts)
        {
            zombieCost.zombie.GetComponent<ZombieAI>().gameInfo = currentGameInfo;
        }
    }

    private void FixedUpdate()
    {
        if (_spawningAllowed)
        {
            if (Time.fixedTime > _timeLastSpawn + _timeBeforeNextSpawn)
            {
                SpawnZombie();
            }
        }
    }

    public void InitializeForDay(int day)
    {
    }

    public void CanSpawn(bool spawningStatus)
    {
        _spawningAllowed = spawningStatus;
    }

    public void SortZCosts()
    {
        zCosts = zCosts.OrderBy(zombieCost => zombieCost.points).ToArray();
    }

    public void SpawnZombie()
    {
        int zombieIndex = 0;
        if (currentAvailablePoints > zCosts.First().points)
        {
            while (zCosts[zombieIndex].points >= currentAvailablePoints)
            {
                zombieIndex = Random.Range(0, zCosts.Length);
            }

            _zombieToGive = zCosts[zombieIndex].zombie;
            _chosenSpawnPoint = zSpawnPoints[Random.Range(0, zSpawnPoints.Count - 1)];

            Instantiate(_zombieToGive, _chosenSpawnPoint.transform.position,
                _chosenSpawnPoint.transform.rotation);

            _timeLastSpawn = Time.fixedTime;
            currentAvailablePoints -= zCosts[zombieIndex].points;

            _timeBeforeNextSpawn = currentTimeBetweenSpawns + Random.Range(timeVariation.x, timeVariation.y);
            Debug.Log(_timeBeforeNextSpawn + _timeLastSpawn);
        }
    }
}