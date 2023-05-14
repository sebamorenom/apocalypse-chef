using Autohand;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliceTrigger : MonoBehaviour
{
    [SerializeField] private float minCuttingSpeedThreshold;
    private Transform parent;


    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Blade") && GetVelocitySum(other.attachedRigidbody) > minCuttingSpeedThreshold)
        {
            parent = transform.parent;
            Transform foodTransform = transform.parent;

            Transform[] children = new Transform[transform.parent.childCount];
            //Fill the array of children
            for (int i = 0; i < parent.childCount; i++)
            {
                children[i] = transform.parent.GetChild(i);
            }

            float parentMass = parent.GetComponent<Rigidbody>().mass;
            float massPerPiece = parentMass / (children.Length - (children.Length / 2));

            //Get this trigger's sibling index
            int triggerIndex = transform.GetSiblingIndex();
            //Get the number of children for both halves
            int firstHalfChildrenCount = triggerIndex - (triggerIndex / 2);
            int secondHalfChildrenCount = (children.Length - triggerIndex) - (children.Length - triggerIndex) / 2;
            //Create halves
            if (firstHalfChildrenCount > 1)
            {
                GameObject firstHalf = new GameObject("FirstHalf");
                firstHalf.transform.parent = foodTransform.parent;
                firstHalf.transform.position = GetMiddleOfPieces(children, 0, triggerIndex);
                //Fill firstHalf's children
                for (int i = 0; i < triggerIndex; i++)
                {
                    children[i].parent = firstHalf.transform;
                }

                firstHalf.AddComponent<Rigidbody>();
                firstHalf.AddComponent<Grabbable>();
                var halfGrabbable = firstHalf.GetComponent<Grabbable>();
                halfGrabbable.enabled = true;
                halfGrabbable.body = firstHalf.GetComponent<Rigidbody>();
                firstHalf.GetComponent<Rigidbody>().mass = massPerPiece * firstHalfChildrenCount;
            }
            else
            {
                var piece = children[0];
                piece.parent = parent.parent;
                MakeGrabbable(massPerPiece, piece);
            }

            if (secondHalfChildrenCount > 1)
            {
                GameObject secondHalf = new GameObject("SecondHalf");
                secondHalf.transform.parent = foodTransform.parent;
                secondHalf.transform.position = GetMiddleOfPieces(children, triggerIndex + 1, children.Length);
                //Fill secondHalf's children
                for (int i = triggerIndex + 1; i < children.Length; i++)
                {
                    children[i].parent = secondHalf.transform;
                }

                secondHalf.AddComponent<Rigidbody>();
                secondHalf.AddComponent<Grabbable>();
                var halfGrabbable = secondHalf.GetComponent<Grabbable>();
                halfGrabbable.enabled = true;
                halfGrabbable.body = secondHalf.GetComponent<Rigidbody>();
                secondHalf.GetComponent<Rigidbody>().velocity = Vector3.zero;
                secondHalf.GetComponent<Rigidbody>().mass = massPerPiece * secondHalfChildrenCount;
            }
            else
            {
                var piece = children[triggerIndex + 1];
                piece.parent = parent.parent;
                MakeGrabbable(massPerPiece, piece);
            }

            Destroy(parent.gameObject);
        }
    }

    private static void MakeGrabbable(float massPerPiece, Transform piece)
    {
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = massPerPiece;
        piece.AddComponent<Grabbable>();
        piece.GetComponent<Grabbable>().enabled = true;
        piece.GetComponent<Grabbable>().body = piece.GetComponent<Rigidbody>();
    }

    private float GetVelocitySum(Rigidbody bladeRigidbody)
    {
        Rigidbody parentRigidbody = transform.parent.GetComponent<Rigidbody>();
        Vector3 bladeLocalSpeed = bladeRigidbody.transform.InverseTransformVector(bladeRigidbody.velocity);
        //Calculate the food velocity in the blade local space
        Vector3 foodVelocityInBladeLocal = bladeRigidbody.transform.InverseTransformVector(parentRigidbody.velocity);
        float bladeForce = bladeRigidbody.velocity.z * bladeRigidbody.mass;
        float foodForce = -foodVelocityInBladeLocal.z * parentRigidbody.mass;
        float impactForce = Mathf.Abs(bladeForce + foodForce);
        print(impactForce);
        return impactForce;
    }

    private Vector3 GetMiddleOfPieces(Transform[] pieces, int fromInclusiveIndex, int toExclusiveIndex)
    {
        Vector3 middle = Vector3.zero;
        int numPieces = toExclusiveIndex - fromInclusiveIndex;
        for (int i = fromInclusiveIndex; i < toExclusiveIndex; i++)
        {
            middle += pieces[i].position;
        }

        return middle / numPieces;
    }
}