using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuddleType
{
    Slow,
    Slippery,
    Flammable
}

[Serializable]
public class Puddle : MonoBehaviour
{
    [HideInInspector] public bool isSlowing;
    [HideInInspector] public float speedPercent;

    [HideInInspector] public bool isSlippery;
    [HideInInspector] public float slippingForce;

    [HideInInspector] public bool isFlammable;
    [HideInInspector] public float totalFlameDamage;
    [HideInInspector] public int numTicks;
    [HideInInspector] public float timeBetweenTicks = 0.5f;

    [HideInInspector] public float timeAlive;

    [HideInInspector] public Material puddleMat;

    private float _spawnTime;
    private Transform _transform;
    private Rigidbody _affectedRb;
    private ZombieAI _affectedZombieAI;
    private Health _affectedHealth;


    public void Start()
    {
        _spawnTime = Time.fixedTime;
        _transform = transform;
    }

    public void Update()
    {
        if (Time.fixedTime >= _spawnTime + timeAlive)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _affectedRb = other.attachedRigidbody;
        if (isSlowing)
        {
            _affectedRb.velocity *= speedPercent;
        }

        if (isSlippery)
        {
        }
        /*
         * if(other.gameObject.TryGetComponent<NavMeshAgent>(out var triggerNMAgent))
         * {
         *      triggerNMAgent.velocity *= speedPercent
         * }
         */

        if (isSlippery && other.TryGetComponent<ZombieAI>(out _affectedZombieAI))
        {
            _affectedZombieAI.StartRagdoll();
            _affectedRb.AddForceAtPosition(_affectedRb.transform.forward * 10f,
                _affectedRb.ClosestPointOnBounds(_transform.position),
                ForceMode.Impulse);
        }

        if (isFlammable && other.TryGetComponent<Health>(out _affectedHealth))
        {
            Burn(_affectedHealth);
        }
    }

    private float damagePerTick;

    public IEnumerator Burn(Health burningHealth)
    {
        damagePerTick = (totalFlameDamage / numTicks);
        while (numTicks > 0)
        {
            burningHealth.Hurt(damagePerTick);
            numTicks--;
            yield return new WaitForSeconds(timeBetweenTicks);
        }
    }

    public void CopyProperties(Puddle other)
    {
        if (other.isSlowing)
        {
            isSlowing = other.isSlowing;
            speedPercent = other.speedPercent;
        }

        if (other.isSlippery)
        {
            isSlippery = other.isSlippery;
            slippingForce = other.slippingForce;
        }

        if (other.isFlammable)
        {
            isFlammable = other.isFlammable;
            totalFlameDamage = other.totalFlameDamage;
        }

        puddleMat = other.puddleMat;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out _affectedRb))
        {
            if (isSlowing)
            {
                _affectedRb.velocity /= speedPercent;

                /*
                 * if(other.gameObject.TryGetComponent<NavMeshAgent>(out var triggerNMAgent))
                 * {
                 *      triggerNMAgent.velocity /= speedPercent
                 * }
                 */
            }
        }
    }
}