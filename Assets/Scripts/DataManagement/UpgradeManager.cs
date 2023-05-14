using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private GameInfo _gameInfo;
    [SerializeField] private TextMeshProUGUI spawnerNameTMPro;
    [SerializeField] private TextMeshProUGUI trapNameTMPro;

    private int _currentSpawnerIndex, _currentTrapIndex;

    private Spawner[] _spawners;

    private Trap[] _traps;

    void SetGameInfo(GameInfo gameInfo)
    {
        _gameInfo = gameInfo;
    }

    public void UpgradeSpawner()
    {
        if (_spawners[_currentSpawnerIndex].currentUpgradeLevel < 2)
            _spawners[_currentSpawnerIndex].currentUpgradeLevel++;
    }

    public void UpgradeTrap()
    {
        if (_traps[_currentTrapIndex].currentUpgradeLevel < 2)
            _traps[_currentTrapIndex].currentUpgradeLevel++;
    }

    // Start is called before the first frame update

    void MoveSlideSpawner(int spawnerIndexOffset)
    {
        _currentSpawnerIndex = (_currentSpawnerIndex + spawnerIndexOffset) % _spawners.Length;
        spawnerNameTMPro.text = _spawners[_currentSpawnerIndex].spawnerName;
    }

    void MoveSlideTrap(int trapIndexOffset)
    {
        _currentTrapIndex = (_currentTrapIndex + trapIndexOffset) % _traps.Length;
        trapNameTMPro.text = _traps[_currentTrapIndex].trapName;
    }
}