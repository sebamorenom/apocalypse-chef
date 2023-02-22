using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    [HideInInspector] public bool isSlowing;
    [HideInInspector] public float speedPercent;

    [HideInInspector] public bool isSlippery;
    [HideInInspector] public float slippingForce;

    [HideInInspector] public bool isFlammable;
    [HideInInspector] public float flameDamage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out var triggerRb))
        {
            triggerRb.velocity *= speedPercent;
        }
        /*
         * if(other.gameObject.TryGetComponent<NavMeshAgent>(out var triggerNMAgent))
         * {
         *      triggerNMAgent.velocity *= speedPercent
         * }
         */
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out var collidedRb))
        {
            collidedRb.velocity /= speedPercent;
        }
        /*
         * if(other.gameObject.TryGetComponent<NavMeshAgent>(out var triggerNMAgent))
         * {
         *      triggerNMAgent.velocity /= speedPercent
         * }
         */
    }
}