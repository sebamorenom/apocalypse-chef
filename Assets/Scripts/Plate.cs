using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private float separationAcrossIngredients;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            var otherTransform = other.transform;
            var otherRigidbody = other.attachedRigidbody;
            var placementTransform = transform.GetChild(0);
            if (placementTransform.childCount == 0)
            {
                otherTransform.parent = placementTransform;
            }
            else
            {
                var lastChildTransform = otherTransform.GetChild(otherTransform.childCount - 1);
                otherTransform.parent = lastChildTransform;
            }

            otherTransform.localPosition = Vector3.zero + otherTransform.up;
            otherTransform.rotation = placementTransform.rotation;
            otherRigidbody.velocity = Vector3.zero;
            DisableIngredient(otherRigidbody);
        }
    }

    private static void DisableIngredient(Rigidbody otherRigidbody)
    {
        otherRigidbody.isKinematic = true;
        otherRigidbody.useGravity = false;
        otherRigidbody.detectCollisions = false;
    }
}