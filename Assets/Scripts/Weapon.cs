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
    public string identifier;
    public float damage;
    [SerializeField] protected float minSpeedOnHitThreshold;
    [SerializeField] protected float minSpeedOnTravelThreshold;
    public bool thrown;
    
    public OnHit onHit;

    public OnTravel onTravel;

    // Start is called before the first frame update
    public void ChangeDamage(float newDamage)
    {
        damage = newDamage;
    }
}