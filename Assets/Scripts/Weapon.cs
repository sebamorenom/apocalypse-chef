using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public delegate void OnHit();

public delegate void OnTravel();

public class Weapon : MonoBehaviour
{
    public string weaponIdentifier;
    public float damage;
    [SerializeField] protected float minSpeedOnHitThreshold;
    [SerializeField] protected float minSpeedOnTravelThreshold;
    public bool thrown;
    public WeaponEffect wEffect;


    private Rigidbody _rb;

    public OnHit onHit;

    public OnTravel onTravel;

    private void Start()
    {
        if (wEffect != null) onHit += wEffect.onHit;
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    public void ChangeDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb.velocity.magnitude >= minSpeedOnHitThreshold)
        {
            if (onHit != null) onHit.Invoke();
        }
    }
}