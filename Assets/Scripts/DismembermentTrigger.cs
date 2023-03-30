using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DismembermentTrigger : MonoBehaviour
{
    public enum Limb
    {
        Head,
        Arm,
        Leg
    }

    [HideInInspector] public Limb dismemberedLimb;
    [HideInInspector] public float weightPercent;
    [HideInInspector] public bool forceDetach;
    [SerializeField] private GameObject _dismemberedCopySibling;

    private Rigidbody _rb;

    private Transform _transform;
    // Start is called before the first frame update

    private void Start()
    {
        _transform = transform;
        _transform.SetAsLastSibling();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (forceDetach)
        {
            BreakJoint();
            forceDetach = false;
        }
    }

    public void UpdateLimb()
    {
        switch (dismemberedLimb)
        {
            case Limb.Head:
                weightPercent = 0.08f;
                break;
            case Limb.Arm:
                weightPercent = 0.05f;
                break;
            case Limb.Leg:
                weightPercent = 0.17f;
                break;
        }
    }

    private void BreakJoint()
    {
        _transform.localScale = Vector3.one * .000001f;
        _dismemberedCopySibling.transform.parent = null;
        _dismemberedCopySibling.SetActive(true);
    }
}