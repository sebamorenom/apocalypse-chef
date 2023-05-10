using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponTest : MonoBehaviour
{
    [Header("Basic Weapon Parameters")]
    //
    [SerializeField] public string weaponName;
    [SerializeField] public float damage;
    [SerializeField] public float hitForceThreshold;
    [SerializeField] public float thrownVelocityThreshold;

    [SerializeField] public float destructionTimer;
    //
    public UnityEvent onHit, onThrow;

}