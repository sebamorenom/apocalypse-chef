using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Autohand;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[RequireComponent(typeof(SaveableObject))]
public class Spawner : Upgradable
{
    [SerializeField] public string spawnerName;
    [SerializeField] private IngredientsList spawnableList;
    [SerializeField] private float[] spawnerTimers = new float[3];
    [SerializeField] private TextMeshProUGUI spawnableName;
    [SerializeField] private GameObject _vfx;
    [SerializeField] private Vector3 spawningOffset;
    private int _currentArrayIndex = 0;
    private Hand tryHand;
    private bool itemInside;
    private float halfHeight;

    private Transform _transform;

    private TemporalUpgradeStorage _temporalUpgradeStorage;

    private float _lastSpawnTime = float.MinValue;
    private Director _director;

    private void Start()
    {
        _transform = transform;
        var boxBounds = GetComponent<Collider>().bounds;
        halfHeight = (boxBounds.max.y - boxBounds.min.y) / 2f;
        _director = Director.Instance;
        _temporalUpgradeStorage = FindObjectOfType<TemporalUpgradeStorage>();
        CheckForUpgrades();
        ChangeText();
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
                _lastSpawnTime + spawnerTimers[currentUpgradeLevel])
            {
                Spawn();
                _lastSpawnTime = Time.fixedTime;
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

    public void MoveNextPrevious(bool dir)
    {
        if (dir)
        {
            _currentArrayIndex = (_currentArrayIndex + 1) % spawnableList.ingredientList.Length;
        }
        else
        {
            _currentArrayIndex = _currentArrayIndex - 1 >= 0
                ? _currentArrayIndex - 1
                : spawnableList.ingredientList.Length - 1;
        }

        ChangeText();
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
        Instantiate(_vfx, _transform.position + spawningOffset, Quaternion.identity);
        Instantiate(spawnableList.ingredientList[_currentArrayIndex], 
            _transform.position + _transform.up * halfHeight + spawningOffset,
        Quaternion.identity);
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

    private void ChangeText()
    {
        spawnableName.text = spawnableList.ingredientList[_currentArrayIndex].GetFoodIdentifier();
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