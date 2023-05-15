using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    [SerializeField] private AutoHandPlayer autoHandPlayer;
    [SerializeField] private Health playerHealth;

    private bool _hasBeenDisabled;

    void Update()
    {
        if (playerHealth.dead && !_hasBeenDisabled)
        {
            _hasBeenDisabled = true;
            autoHandPlayer.enabled = false;
        }
    }
}