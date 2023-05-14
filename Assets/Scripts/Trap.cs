using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Upgradable
{
    private struct TrapStates
    {
        public static int Activate = Animator.StringToHash("Activate");
        public static int Deactivate = Animator.StringToHash("Deactivate");
    }

    [SerializeField] public string trapName;
    [SerializeField] public float[] timer = new float[3];
    public int currentUpgradeLevel;
    private Animator _animator;
    private AnimationClip[] _animationClips;


    private float _activatedAnimationTime;
    private float _deactivatedAnimationTime;

    private float _activatedTime;
    private float _deactivatedTime;

    private TemporalUpgradeStorage _temporalUpgradeStorage;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        GetAnimationClips();
        _temporalUpgradeStorage = FindObjectOfType<TemporalUpgradeStorage>();
        CheckForUpgrades();
    }

    public void SwitchState()
    {
        if (!isActive && Time.fixedTime > _activatedTime + _activatedAnimationTime)
        {
            _animator.Play(TrapStates.Activate);
            isActive = true;
        }

        if (isActive && Time.fixedTime > _deactivatedTime + _deactivatedAnimationTime)
        {
            _animator.Play(TrapStates.Deactivate);
            isActive = false;
        }
    }

    public void GetAnimationClips()
    {
        _animationClips = _animator.runtimeAnimatorController.animationClips;
        foreach (var animClip in _animationClips)
        {
            switch (animClip.name)
            {
                case "Activated":
                    _activatedAnimationTime = animClip.length;
                    break;
                case "Deactivated":
                    _deactivatedAnimationTime = animClip.length;
                    break;
            }
        }
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
            _temporalUpgradeStorage.trapsUpgradeInfo.Add(upgradeInfo);
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