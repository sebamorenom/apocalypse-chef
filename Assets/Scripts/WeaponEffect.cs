using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "WeaponEffect")]
public class WeaponEffect : ScriptableObject
{
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
    [HideInInspector] public float minStrengthToThrow = 1f;
    [HideInInspector] public float minDotYToLaunch = 0.5f;
    [HideInInspector] public AnimationCurve parabolaZ = new AnimationCurve();
    [HideInInspector] public AnimationCurve parabolaX = new AnimationCurve();
    [HideInInspector] public AnimationCurve projectileVelocity = new AnimationCurve();

    [HideInInspector] public bool spawnPuddle;
    [HideInInspector] public GameObject puddleGameObject;
    [HideInInspector] public Puddle puddleProperties;

    [Header("AudioClips")] [HideInInspector]
    public AudioClip onThrowClip;

    [HideInInspector] public AudioClip onDestroyClip;
    [HideInInspector] public AudioClip onHandClip;

    [HideInInspector] public Transform fTransform;
    [HideInInspector] private Rigidbody fRb;
    [HideInInspector] public Collider fCollider;
    [HideInInspector] public MonoBehaviour mono;

    public OnHit onHit;
    public OnTravel onTravel;


    public void Fill(Transform foodTransform, Collider foodCollider, MonoBehaviour monoBehaviour)
    {
        fTransform = foodTransform;
        fCollider = foodCollider;
        fRb = fCollider.attachedRigidbody;
        if (!puddleGameObject.IsUnityNull())
        {
            puddleGameObject.GetComponent<Puddle>().CopyProperties(puddleProperties);
        }

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


    public void ReturnToHand()
    {
        mono.StartCoroutine(StartReturningToHand());
    }


    private Vector3 basePosition;
    private float initTime;
    private Vector3 posOffset;
    private Vector3 dir;
    private Vector3 zDirLocal;
    private Vector3 xDirLocal;
    private Vector3 zPosOffset;
    private Vector3 xPosOffset;
    private Vector3 localVelocity;
    private Vector3 localDir;
    private float timeSinceLaunch;

    private IEnumerator StartReturningToHand()
    {
        //float timeToReturn = fRbVelocityMag;
        initTime = Time.fixedTime;
        basePosition = fTransform.position;
        localVelocity = fTransform.InverseTransformVector(fRb.velocity);
        localDir = localVelocity.normalized;
        xDirLocal = (fTransform.right);
        zDirLocal = (fTransform.forward);
        timeSinceLaunch = 0f;
        if (Mathf.Abs(Vector3.Dot(fTransform.up, Vector3.up)) > minDotYToLaunch &&
            fRb.velocity.magnitude >= minStrengthToThrow && fRb.angularVelocity.y >= 9f)
        {
            while (timeSinceLaunch <= 4f)
            {
                xPosOffset = xDirLocal * (localVelocity.x * parabolaX.Evaluate(timeSinceLaunch / 4f));

                zPosOffset = zDirLocal * (localVelocity.z * 4 * parabolaZ.Evaluate(timeSinceLaunch / 4f));
                posOffset = (xPosOffset + zPosOffset) * projectileVelocity.Evaluate(timeSinceLaunch / 4f);
                Debug.Log(timeSinceLaunch / 4f);
                fRb.velocity = posOffset;
                timeSinceLaunch += Time.fixedDeltaTime;
                yield return null;
            }
        }
    }

    public void SpawnPuddle()
    {
        puddleGameObject.transform.position = fTransform.position;
        puddleGameObject.transform.rotation = fTransform.rotation;
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

        if (isNoisy)
        {
            onHit += Noise;
        }

        if (spawnPuddle)
        {
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