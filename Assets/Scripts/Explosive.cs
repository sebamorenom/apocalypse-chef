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
    [SerializeField] public bool sticky;
    [SerializeField] public bool stun;
    [SerializeField] public bool incendiary;
    [SerializeField] public bool toxic;

    [Header("Timers")] [SerializeField] public float explosionHitTimer;
    [SerializeField] public float explosionThrowTimer;

    [SerializeField] public float stuckTime;
    [SerializeField] public float stunTime;
    [SerializeField] public float burningTime;

    [Header("LayerMask")] [SerializeField] private LayerMask affectedLayerMask;

    [Header("Utilities")] [SerializeField] private int maxNumAffected;


    private Transform _transform;
    private Rigidbody _rb;
    private Health _ownHealth;

    private float _startingTimer;
    private IEnumerator _onHitTimer, _onThrowTimer;
    private bool _hitTimerActive, _throwTimerActive;
    private bool _thrown;

    private Collider[] _hitColliders;
    private ZombieAI _hitZombieAI;
    private Health _hitZombieHealth;
    private Rigidbody _hitRb;
    private Explosive _hitExplosive;

    private Collision _lastCollision;
    private Vector3 _lastPos, _newPos;

    private Objective _asObjective;

    private void Start()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _onHitTimer = OnHitTimer();
        _onThrowTimer = OnThrowTimer();
        _hitColliders = new Collider[maxNumAffected];
        _asObjective = new Objective
        {
            objectiveHealth = _ownHealth,
            objectiveTransform = _transform
        };
    }

    private void LateUpdate()
    {
        _lastPos = _newPos;
        _newPos = _transform.position;
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


    private bool CheckHit(Collision collisionToCheck)
    {
        if (_rb.velocity.magnitude > hitForceThreshold)
        {
            _lastCollision = collisionToCheck;
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

        if (CheckHit(collision))
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
        Physics.OverlapSphereNonAlloc(_transform.position, explosionRadius, _hitColliders, affectedLayerMask);
        for (int i = 0; i < _hitColliders.Length; i++)
        {
            if (_hitColliders[i].TryGetComponent<Health>(out _hitZombieHealth) &&
                _hitColliders[i].CompareTag($"Zombie"))
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

    public void Sticky()
    {
        _hitColliders = Physics.OverlapSphere(_transform.position, explosionRadius, affectedLayerMask);
        for (int i = 0; i < _hitColliders.Length; i++)
        {
            if (_hitColliders[i].TryGetComponent<ZombieAI>(out _hitZombieAI))
            {
                _hitZombieAI.stuckTime = stuckTime;
                _hitZombieAI.stuck = true;
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

    private void Distract()
    {
        StartCoroutine(DistractingCoroutine());
    }

    private IEnumerator DistractingCoroutine()
    {
        while (!_ownHealth.dead)
        {
            Physics.OverlapSphereNonAlloc(_transform.position, explosionRadius, _hitColliders, affectedLayerMask);
            for (int i = 0; i < _hitColliders.Length; i++)
            {
                _hitColliders[i].GetComponent<ZombieAI>().Distract(this._asObjective);
            }

            yield return new WaitForSeconds(1);
        }
    }


    public void Destroy()
    {
        Destroy(gameObject, destructionTimer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}