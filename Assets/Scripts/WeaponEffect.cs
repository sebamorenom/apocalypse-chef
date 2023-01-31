using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "WeaponEffect")]
public class WeaponEffect : ScriptableObject
{
    [SerializeField] private string effectName;
    [HideInInspector] public bool isExplosive;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public float explosionForce;
    [HideInInspector] public float explosionDamage;
    [HideInInspector] public VisualEffect explosionVFX;

    [HideInInspector] public bool isSticky;
    [HideInInspector] public float slownessPercent;
    [HideInInspector] public float slownessRadius;
    [HideInInspector] public VisualEffect stickyVFX;

    [HideInInspector] public bool isNoisy;
    [HideInInspector] public float noiseDuration;
    [HideInInspector] public float noiseRadius;
    [HideInInspector] public VisualEffect noiseVFX;

    [HideInInspector] public bool isFlammable;
    [HideInInspector] public float flammableRadius;
    [HideInInspector] public VisualEffect flammableVFX;

    public bool testing;

    [HideInInspector] public Transform fTransform;
    [HideInInspector] public Collider fCollider;

    public OnHit onHit;


    public void Fill(Transform foodTransform, Collider foodCollider)
    {
        fTransform = foodTransform;
        fCollider = foodCollider;
    }

    public void Explode()
    {
        var affected = Physics.OverlapSphere(fTransform.position, explosionRadius);
        IDamageable tryEnemy;
        Rigidbody tryRb;
        foreach (var entity in affected)
        {
            if (entity.TryGetComponent<IDamageable>(out tryEnemy))
            {
                tryEnemy.Hurt(explosionDamage);
            }

            if (entity.TryGetComponent<Rigidbody>(out tryRb))
            {
                tryRb.AddExplosionForce(explosionForce, fTransform.position, explosionRadius);
            }
        }
    }

    public void Slow()
    {
        Collider[] affected = Physics.OverlapSphere(fTransform.position, slownessRadius);
        NavMeshAgent tryNavMeshAgent;
        Rigidbody tryRb;
        foreach (Collider coll in affected)
        {
            if (coll.TryGetComponent<NavMeshAgent>(out tryNavMeshAgent))
            {
                tryNavMeshAgent.velocity *= 1 - slownessPercent;
                continue;
            }

            if (coll.TryGetComponent<Rigidbody>(out tryRb))
            {
                tryRb.velocity = Vector3.zero;
            }
        }
    }

    public void Noise()
    {
        StartNoise();
    }

    public IEnumerator StartNoise()
    {
        NavMeshObstacle obstacle = fTransform.GetComponent<NavMeshObstacle>();
        var carving = obstacle.carving;
        carving = true;
        yield return new WaitForSeconds(noiseDuration);
        carving = false;
        Destroy(obstacle);
    }

    public void Flammable()
    {
        Collider[] affected = Physics.OverlapSphere(fTransform.position, flammableRadius);
        foreach (var coll in affected)
        {
            //coll.GetComponent<Zombie>().canBeOnFire;
        }
    }

    public void Testing()
    {
        Debug.Log("Testing");
    }

    public void ApplyEffects()
    {
        onHit = null;
        if (isExplosive)
        {
            onHit += Explode;
        }

        if (isSticky)
        {
            onHit += Slow;
        }

        if (isNoisy)
        {
            onHit += Noise;
        }


        if (isFlammable)
        {
            onHit += Flammable;
        }


        var invocationListLength = 0;
        if (onHit != null)
        {
            invocationListLength = onHit.GetInvocationList().Length;
        }

        Debug.Log(invocationListLength);
    }
}