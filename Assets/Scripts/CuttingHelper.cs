using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingHelper : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float cuttingBoxThreshold;
    [SerializeField] private GameObject cutFood;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Blade"))
        {
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody.velocity.magnitude > cuttingBoxThreshold &&
                Vector3.Dot(other.transform.forward, transform.up) < -0.6f)
            {
                health = Mathf.Max(health - 20f, 0f);
            }

            if (health == 0)
            {
                Instantiate(cutFood);
                Destroy(gameObject);
            }
        }
    }
}