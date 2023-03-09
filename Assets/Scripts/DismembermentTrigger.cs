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
    private float parentWeight;
    private Transform limbPiece;
    private Rigidbody limbPieceRb;

    private Transform _transform;
    // Start is called before the first frame update

    private void Start()
    {
        _transform = transform;
        _transform.SetAsLastSibling();
        limbPiece = _transform.parent.GetChild(0);
        parentWeight = transform.parent.GetComponent<Rigidbody>().mass;
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
        limbPiece.parent = null;
        limbPiece.AddComponent<Rigidbody>().mass = parentWeight * weightPercent;
        parentWeight -= parentWeight * weightPercent;
        Destroy(gameObject);
    }
}