using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class KnifeVFX : MonoBehaviour
{
    [SerializeField] private float minSpeedToVFX;


    private Transform _transform;
    private Rigidbody _rb;
    private VisualEffect _vfx;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _vfx = GetComponentInChildren<VisualEffect>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rb.GetPointVelocity(_transform.position).magnitude > minSpeedToVFX)
        {
            PlayVFX();
        }
    }

    private void PlayVFX()
    {
        _vfx.Play();
    }
}