using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Timeline;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CuttingHelper : MonoBehaviour
{
    [SerializeField] private float cuttingBoxThreshold;
    private Ingredient foodToCut;

    private void OnTriggerEnter(Collider other)
    {
        Ingredient tryIng;
        if (other.TryGetComponent<Ingredient>(out tryIng))
        {
            foodToCut = tryIng;
            PrepareToCut(other);
        }
    }

    private void PrepareToCut(Collider other)
    {
        other.transform.position = transform.position;
        other.attachedRigidbody.velocity = Vector3.zero;
        other.attachedRigidbody.isKinematic = true;
        other.enabled = false;
    }

    private void TakeOut(Collider other)
    {
        other.attachedRigidbody.isKinematic = false;
        other.enabled = true;
        foodToCut = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Blade") && foodToCut != null)
        {
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody.velocity.magnitude > cuttingBoxThreshold &&
                Vector3.Dot(other.transform.forward, transform.up) < -0.6f)
            {
                foodToCut.cuttingHealth = Mathf.Max(foodToCut.cuttingHealth - 20f, 0f);
                if (foodToCut.cuttingHealth == 0)
                {
                    Instantiate(foodToCut.cutIngredient, transform.position, Quaternion.identity);
                    Destroy(foodToCut.gameObject);
                    foodToCut = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient tryIng;
        if (other.TryGetComponent<Ingredient>(out tryIng))
        {
            if (foodToCut == tryIng)
            {
                TakeOut(other);
            }
        }
    }
}