using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public delegate void OnHit();

public delegate void OnTravel();

public class Weapon : MonoBehaviour
{
    [Header("General parameters")] [SerializeField]
    protected float minSpeedOnHitThreshold;

    [SerializeField] protected float minSpeedOnTravelThreshold;
    [SerializeField] public float gunDamage = 10f;

    [Header("Gun parameters")] [SerializeField]
    public bool canShoot;

    [SerializeField] public float minDistToUpdatePointer = 1f;
    [SerializeField] public float bulletsPerSecond;
    [SerializeField] public int bulletCount = 10;
    public bool thrown;
    public WeaponEffect wEffect;

    private LineRenderer _shootingPointer;
    private Ray _shootingRay;
    private RaycastHit _shootingHit;
    private Vector3 _oldPointerPos = Vector3.zero;
    private Health _shotHealth;
    private float _lastTimeShot;

    private Transform _transform;
    private Rigidbody _rb;
    private Collider _collider;

    public bool shootingMode;


    public OnHit onHit;
    public OnTravel onTravel;

    private void Start()
    {
        _transform = transform;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();

        if (canShoot)
        {
            _shootingPointer = GetComponent<LineRenderer>();
            _shootingPointer.enabled = false;
            _shootingPointer.positionCount = 2;
            _shootingPointer.startWidth = _shootingPointer.endWidth = 0.015f;
            _shootingPointer.startColor = _shootingPointer.endColor = Color.red;
            _shootingPointer.SetPosition(0, _transform.position);
        }

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

    private void Update()
    {
        if (shootingMode)
        {
            UpdatePointer();
        }
    }

    public void ActivateShootingMode()
    {
        _shootingPointer.enabled = true;
        shootingMode = !shootingMode;
        StartCoroutine(UpdatePointer());
    }

    private IEnumerator UpdatePointer()
    {
        while (shootingMode)
        {
            Physics.Raycast(_transform.position + _collider.bounds.extents.z * _transform.forward, _transform.forward,
                out _shootingHit);
            if ((_shootingHit.point - _oldPointerPos).magnitude > minDistToUpdatePointer)
            {
                _shootingPointer.SetPosition(1, _shootingHit.point);
                _oldPointerPos = _shootingHit.point;
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    public void Shoot()
    {
        if (Time.fixedTime >= _lastTimeShot + 1 / bulletsPerSecond)
        {
            if (bulletCount > 0)
            {
                bulletCount--;
                Destroy(gameObject, 5);
                return;
            }

            _lastTimeShot = Time.fixedTime;
            if (shootingMode && _shootingHit.transform.TryGetComponent<Health>(out _shotHealth))
            {
                _shotHealth.Hurt(gunDamage);
            }
        }
    }

    // Start is called before the first frame update
    public void ChangeDamage(float newDamage)
    {
        gunDamage = newDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb.velocity.magnitude >= minSpeedOnHitThreshold)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var tryIDamageable))
            {
                tryIDamageable.Hurt(gunDamage);
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