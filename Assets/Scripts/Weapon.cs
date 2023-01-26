using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    [SerializeField] protected float minSpeedOnHitThreshold;
    [SerializeField] protected float minSpeedOnTravelThreshold;
    public bool thrown;

    public UnityEvent onHit;

    public UnityEvent onTravel;

    // Start is called before the first frame update
    public void ChangeDamage(float newDamage)
    {
        damage = newDamage;
    }
}