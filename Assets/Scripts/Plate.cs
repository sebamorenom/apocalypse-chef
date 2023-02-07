using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using Unity.VisualScripting;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private float separationAcrossIngredients;
    [SerializeField] private Collider plateCollider;
    [SerializeField] private Collider placementTrigger;

    [SerializeField] private List<string> foodStacked;

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
            Vector3 newPlacementMaxBound = new Vector3(placementTrigger.bounds.max.x,
                other.bounds.max.y + 0.5f,
                placementTrigger.bounds.max.z);
            placementTrigger.bounds.SetMinMax(placementTrigger.bounds.min, newPlacementMaxBound);
            DisableAndMakeGrabbable(otherRigidbody);
            foodStacked.Insert(0, other.GetComponent<Ingredient>().foodIdentifier);
        }
    }

    private void DisableAndMakeGrabbable(Rigidbody otherRigidbody)
    {
        Destroy(otherRigidbody);
        GrabbableChild otherGrabbChild = otherRigidbody.AddComponent<GrabbableChild>();
        otherGrabbChild.grabParent = GetComponent<Grabbable>();
        Destroy(otherRigidbody.GetComponent<Ingredient>());
        /*otherRigidbody.isKinematic = false;
        otherRigidbody.useGravity = false;
        otherRigidbody.detectCollisions = false;*/
    }

    public string[] GetFoodOnPlate()
    {
        return foodStacked.ToArray();
    }
}