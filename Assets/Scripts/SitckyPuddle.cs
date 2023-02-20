using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitckyPuddle : MonoBehaviour
{
    [Range(0f, 1f)] public float speedPercent;

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