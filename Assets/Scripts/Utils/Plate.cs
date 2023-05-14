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
    [SerializeField] [Range(0, 10f)] private float triggerHeight;
    [SerializeField] private List<string> foodStacked;
    public int plateValue = 0;
    private Bounds triggerBounds;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        triggerBounds = placementTrigger.bounds;
        placementTrigger.bounds.SetMinMax(triggerBounds.min,
            new Vector3(triggerBounds.max.x, triggerBounds.min.y + triggerHeight, triggerBounds.max.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            other.GetComponent<Grabbable>().isGrabbable = false;
            var otherTransform = other.transform;
            var otherRigidbody = other.attachedRigidbody;
            otherRigidbody.isKinematic = true;
            var placementTransform = _transform;
            Vector3 placementPosition;
            float previousObjectHalfHeight;

            if (placementTransform.childCount == 1)
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

            /*Vector3 newPlacementMaxBound = new Vector3(placementTrigger.bounds.max.x,
                other.bounds.max.y + 0.5f,
                placementTrigger.bounds.max.z);
                */
            var placementTriggerHeight =
                new Vector3(0, placementTrigger.bounds.max.y - placementTrigger.bounds.min.y, 0);
            placementTrigger.transform.position = placementPosition + yOffset;
            //MoveTriggerBounds(other);
            //placementTrigger.bounds.SetMinMax(placementTrigger.bounds.min, newPlacementMaxBound);
            DisableAndMakeGrabbable(otherRigidbody);
            var auxIngredient = other.GetComponent<Ingredient>();
            plateValue += auxIngredient.foodValue;
            foodStacked.Insert(0, auxIngredient.foodIdentifier);
        }
    }

    private void DisableAndMakeGrabbable(Rigidbody otherRigidbody)
    {
        Destroy(otherRigidbody.GetComponent<Grabbable>());
        Destroy(otherRigidbody);
        GrabbableChild otherGrabbChild = otherRigidbody.AddComponent<GrabbableChild>();
        otherGrabbChild.grabParent = GetComponent<Grabbable>();
        Destroy(otherRigidbody.GetComponent<Ingredient>());
        /*otherRigidbody.isKinematic = false;
        otherRigidbody.useGravity = false;
        otherRigidbody.detectCollisions = false;*/
    }

    private void MoveTriggerBounds(Collider foodColl)
    {
        var newMin = new Vector3(triggerBounds.min.x, foodColl.bounds.max.y, triggerBounds.min.z);
        placementTrigger.bounds.SetMinMax(newMin,
            new Vector3(triggerBounds.max.x, newMin.y + triggerHeight, triggerBounds.max.z));
    }

    public string[] GetFoodOnPlate()
    {
        return foodStacked.ToArray();
    }
}