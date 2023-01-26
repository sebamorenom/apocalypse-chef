using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private float separationAcrossIngredients;
    [SerializeField] private Collider plateCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            var otherTransform = other.transform;
            var otherRigidbody = other.attachedRigidbody;
            otherRigidbody.isKinematic = true;
            var placementTransform = transform.GetChild(0);
            Vector3 placementPosition;
            float previousObjectHalfHeight;
            if (placementTransform.childCount == 0)
            {
                previousObjectHalfHeight = (plateCollider.bounds.max.y - plateCollider.bounds.min.y) / 2;
                placementPosition = new Vector3(placementTransform.position.x,
                    previousObjectHalfHeight + plateCollider.transform.position.y,
                    placementTransform.position.z);
            }
            else
            {
                var previousObjectCollider =
                    placementTransform.GetChild(placementTransform.childCount - 1).GetComponent<Collider>();
                previousObjectHalfHeight =
                    (previousObjectCollider.bounds.max.y - previousObjectCollider.bounds.min.y) / 2;
                placementPosition = new Vector3(placementTransform.position.x,
                    previousObjectHalfHeight + previousObjectCollider.transform.position.y,
                    placementTransform.position.z);
            }

            Vector3 yOffset = new Vector3(0f, (other.bounds.max.y - other.bounds.min.y) / 2, 0f);
            placementPosition += yOffset;
            otherTransform.position = placementPosition;
            otherTransform.rotation = placementTransform.rotation;
            otherTransform.parent = placementTransform;
            DisableAndMakeGrabbable(otherRigidbody);
        }
    }

    private void DisableAndMakeGrabbable(Rigidbody otherRigidbody)
    {
        Destroy(otherRigidbody);
        otherRigidbody.GetComponent<GrabbableChild>().grabParent = GetComponent<Grabbable>();
        /*otherRigidbody.isKinematic = false;
        otherRigidbody.useGravity = false;
        otherRigidbody.detectCollisions = false;*/
    }
}