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

    private ObjectivesManager _objectivesManager;

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
        Director.Instance.zSpawnManager = this;
        _objectivesManager = GetComponent<ObjectivesManager>();
        SetObjectivesManagerToZombie();
        InitializeForDay(Director.Instance.currentGameInfo.currentDay);
        zSpawnPoints = new List<SpawnPoint>();
        zSpawnPoints.AddRange(GetComponentsInChildren<SpawnPoint>().ToArray());
    }

    public void SetObjectivesManagerToZombie()
    {
        foreach (var zombieCost in zCosts)
        {
            zombieCost.zombie.GetComponent<ZombieAI>().objectivesManager = _objectivesManager;
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
            _chosenSpawnPoint = zSpawnPoints[Random.Range(0, zSpawnPoints.Count)];

            Instantiate(_zombieToGive, _chosenSpawnPoint.transform.position,
                _chosenSpawnPoint.transform.rotation);

            _timeLastSpawn = Time.fixedTime;
            currentAvailablePoints -= zCosts[zombieIndex].points;

            _timeBeforeNextSpawn = currentTimeBetweenSpawns + Random.Range(timeVariation.x, timeVariation.y);
            //Debug.Log(_timeBeforeNextSpawn + _timeLastSpawn);
        }
    }
}