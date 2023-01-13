﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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
            GameObject firstHalf = new GameObject();
            firstHalf.name = "FirstHalf";
            GameObject secondHalf = new GameObject();
            secondHalf.name = "SecondHalf";
            firstHalf.transform.parent = secondHalf.transform.parent = foodTransform.parent;
            Transform[] children = new Transform[transform.parent.childCount];
            //Fill the array of children
            for (int i = 0; i < parent.childCount; i++)
            {
                children[i] = transform.parent.GetChild(i);
            }

            //Get this trigger's sibling index
            int triggerIndex = transform.GetSiblingIndex();
            //Get the number of children for both halves
            int firstHalfChildrenCount = triggerIndex - (triggerIndex / 2);
            int secondHalfChildrenCount = (children.Length - triggerIndex) - (children.Length - triggerIndex) / 2;
            //Fill firstHalf's children
            for (int i = 0; i < triggerIndex; i++)
            {
                children[i].parent = firstHalf.transform;
            }

            //Fill secondHalf's children
            for (int i = triggerIndex + 1; i < children.Length; i++)
            {
                children[i].parent = secondHalf.transform;
            }

            //Distributing masses
            float parentMass = parent.GetComponent<Rigidbody>().mass;
            float massPerPiece = parentMass / (children.Length - (children.Length / 2));
            firstHalf.AddComponent<Rigidbody>().mass = massPerPiece * firstHalfChildrenCount;
            secondHalf.AddComponent<Rigidbody>().mass = massPerPiece * secondHalfChildrenCount;
            Destroy(parent.gameObject);
        }
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
}