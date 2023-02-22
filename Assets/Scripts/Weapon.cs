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
    public float damage;
    [SerializeField] protected float minSpeedOnHitThreshold;
    [SerializeField] protected float minSpeedOnTravelThreshold;
    public bool thrown;
    public WeaponEffect wEffect;

    private Transform _transform;
    private Rigidbody _rb;
    private Collider _collider;


    public OnHit onHit;
    public OnTravel onTravel;

    private void Start()
    {
        _transform = transform;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();

        if (wEffect != null)
        {
            wEffect = Instantiate(wEffect);
            wEffect.Fill(_transform, _collider, this);
            wEffect.ApplyEffects();
            onHit += wEffect.onHit;
            onTravel += wEffect.onTravel;
            if (wEffect != null) Debug.Log(onHit.GetInvocationList().Length);
        }
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
            if (collision.gameObject.TryGetComponent<IDamageable>(out var tryIDamageable))
            {
                tryIDamageable.Hurt(damage);
            }

            if (onHit != null)
            {
                //Debug.Log("OnHit Invocation Size: " + onHit.GetInvocationList().Length);
                onHit.Invoke();
                //Debug.Log("OnHit");
            }
        }

        thrown = false;
    }

    public void CheckThrow()
    {
        if (_rb.velocity.magnitude >= minSpeedOnTravelThreshold)
        {
            thrown = true;
            onTravel.Invoke();
        }
    }
}