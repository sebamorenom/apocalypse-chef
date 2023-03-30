using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Autohand;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnableItem;
    [SerializeField] private float[] spawnerTimers = new float[3];
    [SerializeField] private int[] upgradeCost = new int[2];
    private int _currentUpgradeLevel = 0;
    private Hand tryHand;
    private bool itemInside;
    private float halfHeight;

    private Transform _transform;

    private float lastSpawnTime;
    private Director _director;

    private void Start()
    {
        lastSpawnTime = Time.fixedTime;
        _transform = transform;
        var boxBounds = GetComponent<Collider>().bounds;
        halfHeight = (boxBounds.max.y - boxBounds.min.y) / 2f;
        _director = FindObjectOfType<Director>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hand"))
        {
            itemInside = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!itemInside && other.TryGetComponent<Hand>(out tryHand))
        {
            if (Time.fixedTime >=
                lastSpawnTime + spawnerTimers[_currentUpgradeLevel])
            {
                Spawn();
                lastSpawnTime = Time.fixedTime;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Hand"))
        {
            itemInside = false;
        }
    }

    public bool Upgrade()
    {
        if (_currentUpgradeLevel < 2 && _director.currentGameInfo.currentMoney > upgradeCost[_currentUpgradeLevel])
        {
            _currentUpgradeLevel++;
            return true;
        }

        return false;
    }

    public void Spawn()
    {
        Instantiate(spawnableItem, _transform.position + _transform.up * halfHeight, Quaternion.identity);
        itemInside = true;
    }
}