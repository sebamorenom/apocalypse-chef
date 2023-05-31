using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : WeaponTest, IWeapon
{
    [Header("Projectile Parameters")] [SerializeField]
    public bool stickToThings;

    [SerializeField] public float damageModifier;
    [SerializeField] public int maxHits;
    [SerializeField] public int framesBeforeHitDetection;

    [Header("Animation Curves")] [SerializeField]
    public AnimationCurve boomerangX;

    public AnimationCurve boomerangZ;

    [Header("Utilities")] [SerializeField] private GameObject vfx;

    [Header("Boomerang Related")] [SerializeField]
    public float _maxTimeFlying;

    public float _xModifier, _zModifier;

    //
    private Transform _transform;
    private Rigidbody _rb;
    private bool _thrown;

    private float _collisionForce;
    private float _currentFlyingTime;
    private float _xModifierVelocity, _zModifierVelocity;

    private bool _isFlying;
    private Vector3 _startFlyingPos, _flyingOffset;
    private Vector3 _rbDirection;
    private Coroutine _boomerangCoroutine;

    private Collision _lastCollision;
    private Collider _hitCollider;
    private Health _hitZombieHealth;

    private Vector3 _lastPos, _newPos;


    private void Start()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        _lastPos = _newPos;
        _newPos = _transform.position;
    }

    public void OnHit()
    {
        Debug.Log("OnHit");
        onHit.Invoke();
    }

    public void OnThrow()
    {
        _thrown = true;
        onThrow.Invoke();
    }

    public IEnumerator OnHitTimer()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator OnThrowTimer()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_thrown && _currentFlyingTime > Time.deltaTime * framesBeforeHitDetection)
        {
            _thrown = false;
        }

        if (CheckHit(collision))
        {
            if (_boomerangCoroutine != null)
            {
                StopCoroutine(_boomerangCoroutine);
                _rb.velocity = Vector3.zero;
                _rb.useGravity = false;
            }

            OnHit();
            maxHits--;
        }
    }

    public void Throw()
    {
        if (CheckThrow())
        {
            OnThrow();
        }
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
            return true;
        }

        return false;
    }

    public void Damage()
    {
        _hitCollider = _lastCollision.collider;
        if (_hitCollider.TryGetComponent(out _hitZombieHealth))
        {
            _hitZombieHealth.Hurt(_collisionForce > 2 * hitForceThreshold ? damageModifier * damage : damage);
        }
    }

    public void PlayVFX()
    {
        Instantiate(vfx, _transform.position, Quaternion.identity, null);
    }

    public void Boomerang()
    {
        _maxTimeFlying = _rb.velocity.magnitude - thrownVelocityThreshold;
        _boomerangCoroutine = StartCoroutine(BoomerangFlying());
    }

    private IEnumerator BoomerangFlying()
    {
        _rb.useGravity = false;
        _currentFlyingTime = 0;
        _rbDirection = _rb.velocity.normalized;
        _xModifierVelocity = Mathf.Max(Vector3.Dot(_rbDirection, _transform.right) * _rb.velocity.magnitude /
            thrownVelocityThreshold * _rb.velocity.magnitude % thrownVelocityThreshold, 1);
        _zModifierVelocity = Mathf.Max(Vector3.Dot(_rbDirection, _transform.forward) * _rb.velocity.magnitude /
            thrownVelocityThreshold * _rb.velocity.magnitude % thrownVelocityThreshold, 1);
        _startFlyingPos = _transform.position;
        _rb.velocity = Vector3.zero;
        _isFlying = true;
        var initialForward = _transform.forward;
        var initialRight = _transform.right;
        while (_currentFlyingTime < _maxTimeFlying && maxHits > 0 && _thrown)
        {
            _flyingOffset = boomerangX.Evaluate(_currentFlyingTime / _maxTimeFlying) * _xModifierVelocity *
                            initialRight +
                            boomerangZ.Evaluate(_currentFlyingTime / _maxTimeFlying) * _zModifierVelocity *
                            initialForward;
            _transform.position = _startFlyingPos + _flyingOffset;
            _currentFlyingTime += Time.deltaTime;
            yield return null;
        }

        _isFlying = false;
        if (stickToThings)
        {
            _rb.useGravity = false;
        }
        else
        {
            _rb.useGravity = true;
        }
    }

    public void Stick()
    {
        _transform.parent = _lastCollision.transform;
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void Destroy()
    {
        StopAllCoroutines();
        Destroy(gameObject, destructionTimer);
    }
}