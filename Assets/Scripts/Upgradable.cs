using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgradable : MonoBehaviour, ISaveable
{
    [SerializeField] public int[] upgradeCosts = new int[2];
    public int currentUpgradeLevel;

    [Serializable]
    public struct UpgradeInfo
    {
        public string upgradedObjectName;
        public int currentUpgradeLevel;
        public int[] upgradeCosts;
    }

    public object CaptureState()
    {
        UpgradeInfo upgradeInfo = new UpgradeInfo
        {
            currentUpgradeLevel = currentUpgradeLevel,
            upgradeCosts = upgradeCosts
        };
        return upgradeInfo;
    }

    public void LoadState(object state)
    {
        UpgradeInfo upgradeInfo = (UpgradeInfo)state;
        currentUpgradeLevel = upgradeInfo.currentUpgradeLevel;
        upgradeCosts = upgradeInfo.upgradeCosts;
    }
}