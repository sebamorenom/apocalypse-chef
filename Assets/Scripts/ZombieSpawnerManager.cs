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
    [SerializeField] public AnimationCurve pointsTotal;
    [SerializeField] public AnimationCurve timeBetweenSpawns;
    [SerializeField] private Vector2 timeVariation = new Vector2(0, 1);

    private float _timeLastSpawn;
    private bool _spawningAllowed;

    private int _currentTimeBetweenSpawns;

    private float _timeBeforeNextSpawn;

    public int currentAvailablePoints;

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

    private void FixedUpdate()
    {
        if (_spawningAllowed)
        {
            if (Time.fixedTime >= _timeLastSpawn + _timeBeforeNextSpawn)
            {
                SpawnZombie();
            }
        }
        else
        {
            _timeBeforeNextSpawn = 0f;
        }
    }

    public void InitializeForDay(int day)
    {
        currentAvailablePoints = Mathf.RoundToInt(pointsTotal.Evaluate(Math.Clamp(day, 0, 10) / 10f) * 10);
        _currentTimeBetweenSpawns = Mathf.RoundToInt(timeBetweenSpawns.Evaluate(Math.Clamp(day, 0, 10) / 10f) * 10);
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
        if (currentAvailablePoints >= zCosts.First().points)
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

            _timeBeforeNextSpawn = _currentTimeBetweenSpawns + Random.Range(timeVariation.x, timeVariation.y);
        }
    }
}