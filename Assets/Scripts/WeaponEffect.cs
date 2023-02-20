using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [HideInInspector] public bool destroyOnHit;


    [HideInInspector] public bool isBoomerang;
    [HideInInspector] public float minDotYToLaunch = 0.5f;
    [HideInInspector] public float timeToReturn;
    [HideInInspector] public AnimationCurve parabolaZ;
    [HideInInspector] public AnimationCurve parabolaX;

    [Header("AudioClips")] [HideInInspector]
    public AudioClip onThrowClip;

    [HideInInspector] public AudioClip onDestroyClip;
    [HideInInspector] public AudioClip onHandClip;

    [HideInInspector] public Transform fTransform;
    [HideInInspector] public Collider fCollider;
    [HideInInspector] public MonoBehaviour mono;

    public OnHit onHit;
    public OnTravel onTravel;


    public void Fill(Transform foodTransform, Collider foodCollider, MonoBehaviour monoBehaviour)
    {
        fTransform = foodTransform;
        fCollider = foodCollider;
        mono = monoBehaviour;
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
                Debug.Log("Pushed");
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
        mono.StartCoroutine(StartNoise());
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

    public void ReturnToHand()
    {
        mono.StartCoroutine(StartReturningToHand());
    }

    private IEnumerator StartReturningToHand()
    {
        Vector3 basePosition = fTransform.position;
        Quaternion baseRotation = fTransform.rotation;
        Vector3 baseRotationEuler = baseRotation.eulerAngles;
        Quaternion finalRotation = Quaternion.Euler(baseRotationEuler + (Vector3.up * 180f));
        Rigidbody fRb = fTransform.GetComponent<Rigidbody>();
        Vector3 onThrowLocalVelocity = fTransform.InverseTransformDirection(fRb.velocity);
        float initTime = Time.fixedTime;
        if (Mathf.Abs(Vector3.Dot(onThrowLocalVelocity, Vector3.up)) > minDotYToLaunch)
        {
            while (Time.fixedTime <= initTime + timeToReturn)
            {
                Vector3 positionOffset = new Vector3(
                    parabolaX.Evaluate((Time.fixedTime - initTime) / timeToReturn) * onThrowLocalVelocity.x, 0f,
                    parabolaZ.Evaluate((Time.fixedTime - initTime) / timeToReturn) * onThrowLocalVelocity.z);
                fTransform.rotation = Quaternion.Slerp(fTransform.rotation, finalRotation,
                    (Time.fixedTime - initTime) / timeToReturn);
                fTransform.position = new Vector3(basePosition.x + positionOffset.x, basePosition.y + positionOffset.y,
                    basePosition.z + positionOffset.z);
                yield return null;
            }
        }
    }

    public void DestroyOnHit()
    {
        mono.StopAllCoroutines();
        Destroy(fCollider.gameObject);
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

        if (destroyOnHit)
        {
            onHit += DestroyOnHit;
        }

        var invocationListLength = 0;
        if (onHit != null)
        {
            invocationListLength = onHit.GetInvocationList().Length;
        }

        Debug.Log("OnHitEffectsCount: " + invocationListLength);

        if (isBoomerang)
        {
            onTravel += ReturnToHand;
        }
    }
}