using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Autohand;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SaveableObject))]
public class Spawner : Upgradable
{
    [SerializeField] public string spawnerName;
    [SerializeField] private GameObject spawnableItem;
    [SerializeField] private float[] spawnerTimers = new float[3];
    private Hand tryHand;
    private bool itemInside;
    private float halfHeight;

    private Transform _transform;

    private TemporalUpgradeStorage _temporalUpgradeStorage;

    private float lastSpawnTime;
    private Director _director;

    private void Start()
    {
        lastSpawnTime = Time.fixedTime;
        _transform = transform;
        var boxBounds = GetComponent<Collider>().bounds;
        halfHeight = (boxBounds.max.y - boxBounds.min.y) / 2f;
        _director = Director.Instance;
        _temporalUpgradeStorage = FindObjectOfType<TemporalUpgradeStorage>();
        CheckForUpgrades();
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
                lastSpawnTime + spawnerTimers[currentUpgradeLevel])
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
        if (currentUpgradeLevel < 2 && _director.currentGameInfo.currentMoney > upgradeCosts[currentUpgradeLevel])
        {
            currentUpgradeLevel++;
            return true;
        }

        return false;
    }

    public void Spawn()
    {
        Instantiate(spawnableItem, _transform.position + _transform.up * halfHeight, Quaternion.identity);
        itemInside = true;
    }

    public void CheckForUpgrades()
    {
        for (int i = 0; i < _temporalUpgradeStorage.spawnersUpgradeInfo.Count; i++)
        {
            if (_temporalUpgradeStorage.spawnersUpgradeInfo[i].upgradedObjectName == name)
            {
                currentUpgradeLevel = _temporalUpgradeStorage.spawnersUpgradeInfo[i].currentUpgradeLevel;
                upgradeCosts = _temporalUpgradeStorage.spawnersUpgradeInfo[i].upgradeCosts;
                _temporalUpgradeStorage.spawnersUpgradeInfo.Remove(_temporalUpgradeStorage.spawnersUpgradeInfo[i]);
            }
        }
    }


    public new object CaptureState()
    {
        UpgradeInfo upgradeInfo = new UpgradeInfo
        {
            upgradedObjectName = name = this.name,
            currentUpgradeLevel = this.currentUpgradeLevel,
            upgradeCosts = this.upgradeCosts
        };
        if (_temporalUpgradeStorage != null)
        {
            _temporalUpgradeStorage.spawnersUpgradeInfo.Add(upgradeInfo);
        }

        return upgradeInfo;
    }

    public new void LoadState(object state)
    {
        UpgradeInfo upgradeInfo = (UpgradeInfo)state;
        currentUpgradeLevel = upgradeInfo.currentUpgradeLevel;
        upgradeCosts = upgradeInfo.upgradeCosts;
    }
}