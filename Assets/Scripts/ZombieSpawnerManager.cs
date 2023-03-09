using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct ZombieCost
{
    public int points;
    public GameObject zombie;
}

public class ZombieSpawnerManager : MonoBehaviour
{
    [SerializeField] public ZombieCost[] zCosts;
    [SerializeField] public ZombieSpawner[] zSpawners;
    public int totalPoints;

    public int availablePoints;
    // Start is called before the first frame update

    private int _pointsForZombie;

    [SerializeField] public float timeBeforeZombies;

    private float _startingTime;

    private GameObject _zombieToGive;


    private void Start()
    {
        _startingTime = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime >= _startingTime + timeBeforeZombies)
        {
            foreach (var spawner in zSpawners)
            {
                spawner.StartSpawning();
            }
        }
    }

    public void SortZCosts()
    {
        zCosts = zCosts.OrderBy(zombieCost => zombieCost.points).ToArray();
    }

    public GameObject GetZombie(int points)
    {
        int i = 0;
        while (points > zCosts[i].points)
        {
            _zombieToGive = zCosts[i].zombie;
            i++;
        }

        return zCosts[i].zombie;
    }
}