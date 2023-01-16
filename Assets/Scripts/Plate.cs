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
            var placementTransform = transform.GetChild(0);
            if (placementTransform.childCount == 0)
            {
                otherTransform.position = placementTransform.position + placementTransform.up * 0.25f;
            }
            else
            {
                var lastChildTransform = otherTransform.GetChild(otherTransform.childCount - 1);
                otherTransform.position = lastChildTransform.position + placementTransform.up * 0.25f;
            }

            otherTransform.rotation = placementTransform.rotation;
            otherTransform.parent = placementTransform;
            other.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}