using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            var otherRb = other.GetComponent<Rigidbody>();
            otherRb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            var otherRb = other.GetComponent<Rigidbody>();
            otherRb.useGravity = true;
        }
    }
}