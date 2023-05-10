using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class Explosive : WeaponTest, IWeapon
{
    [Header("Explosive Parameters")] [SerializeField]
    public float explosionRadius;

    [SerializeField] public float explosionForce;
    [SerializeField] public int numExplosions;
    [SerializeField] public float timeBetweenExplosions;

    [Header("Type")] [SerializeField] public bool explosive;
    [SerializeField] public bool stun;
    [SerializeField] public bool incendiary;
    [SerializeField] public bool toxic;

    [Header("Timers")] [SerializeField] public float explosionHitTimer;
    [SerializeField] public float explosionThrowTimer;

    [SerializeField] public float stunTime;
    [SerializeField] public float burningTime;

    [Header("LayerMask")] [SerializeField] private LayerMask affectedLayerMask;

    private Transform _transform;
    private Rigidbody _rb;

    private float _startingTimer;
    private IEnumerator _onHitTimer, _onThrowTimer;
    private bool _hitTimerActive, _throwTimerActive;
    private bool _thrown;

    private Collider[] _hitColliders;
    private ZombieAI _hitZombieAI;
    private Health _hitZombieHealth;
    private Rigidbody _hitRb;
    private Explosive _hitExplosive;

    private void Start()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _onHitTimer = OnHitTimer();
        _onThrowTimer = OnThrowTimer();
    }


    public void OnHit()
    {
        onHit.Invoke();
    }

    public void OnThrow()
    {
        onThrow.Invoke();
    }

    public IEnumerator OnHitTimer()
    {
        _hitTimerActive = true;
        yield return new WaitForSeconds(explosionHitTimer);
        _hitTimerActive = false;
        onHit.Invoke();
    }

    public IEnumerator OnThrowTimer()
    {
        _throwTimerActive = true;
        yield return new WaitForSeconds(explosionThrowTimer);
        _throwTimerActive = false;
        onThrow.Invoke();
    }


    private bool CheckHit(Vector3 impulse)
    {
        if ((impulse / Time.fixedDeltaTime).magnitude > hitForceThreshold)
        {
            return true;
        }

        return false;
    }

    public bool CheckThrow()
    {
        if (_rb.velocity.magnitude >= thrownVelocityThreshold)
        {
            _thrown = true;
            return true;
        }

        return false;
    }


    public void OnHitTest()
    {
        Debug.Log("Invoked after " + explosionHitTimer + " seconds");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_thrown)
        {
            _thrown = false;
        }

        if (CheckHit(collision.impulse))
        {
            if (explosionHitTimer == 0)
            {
                OnHit();
            }
            else
            {
                StartCoroutine(_onHitTimer);
            }
        }
    }

    public void Throw()
    {
        if (CheckThrow())
        {
            if (explosionThrowTimer == 0)
            {
                OnThrow();
            }
            else
            {
                StartCoroutine(_onThrowTimer);
            }
        }
    }

    public void Explode()
    {
        if (numExplosions > 1)
        {
            StartCoroutine(ExplodeMultiple());
        }
        else
        {
            ExplodeSingle();
        }
    }

    private void ExplodeSingle()
    {
        _hitColliders = Physics.OverlapSphere(_transform.position, explosionRadius, affectedLayerMask);
        for (int i = 0; i < _hitColliders.Length; i++)
        {
            if (_hitColliders[i].TryGetComponent<Health>(out _hitZombieHealth) && _hitColliders[i].CompareTag($"Zombie"))
            {
                _hitZombieHealth.Hurt(damage);
            }
            else
            {
                if (_hitColliders[i].TryGetComponent<Explosive>(out _hitExplosive) && _hitExplosive.explosive)
                {
                    _hitExplosive.OnHit();
                }
                else if (_hitColliders[i].TryGetComponent<Rigidbody>(out _hitRb))
                {
                    _hitRb.AddExplosionForce(explosionForce, _transform.position, explosionRadius);
                }
            }
        }
    }

    private IEnumerator ExplodeMultiple()
    {
        while (numExplosions > 0)
        {
            ExplodeSingle();
            yield return new WaitForSeconds(timeBetweenExplosions);
            numExplosions--;
        }
    }

    public void Stun()
    {
        _hitColliders = Physics.OverlapSphere(_transform.position, explosionRadius, affectedLayerMask);
        for (int i = 0; i < _hitColliders.Length; i++)
        {
            if (_hitColliders[i].TryGetComponent<ZombieAI>(out _hitZombieAI))
            {
                _hitZombieAI.stunnedTime = stunTime;
                _hitZombieAI.stunned = true;
            }
            else
            {
                if (_hitColliders[i].TryGetComponent<Explosive>(out _hitExplosive) && _hitExplosive.stun)
                {
                    _hitExplosive.OnHit();
                }
            }
        }
    }

    public void Burn()
    {
        _hitColliders = Physics.OverlapSphere(_transform.position, explosionRadius, affectedLayerMask);
        for (int i = 0; i < _hitColliders.Length; i++)
        {
            if (_hitColliders[i].TryGetComponent<Health>(out _hitZombieHealth))
            {
                _hitZombieHealth.Burn(damage / burningTime, burningTime, 1);
            }
            else
            {
                if (_hitColliders[i].TryGetComponent<Explosive>(out _hitExplosive) && _hitExplosive.toxic)
                {
                    _hitExplosive.OnHit();
                }
            }
        }
    }


    public void Destroy()
    {
        Destroy(gameObject, destructionTimer);
    }
}