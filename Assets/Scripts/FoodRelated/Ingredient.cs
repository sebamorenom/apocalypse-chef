using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;


[Serializable]
public class Ingredient : MonoBehaviour, ICook
{
    [Header("Ingredient parameters")] public string foodIdentifier;
    public int foodValue;
    [Header("Cut parameters")] public bool canBeCut;
    public GameObject cutIngredient;
    [ReadOnly] Health cuttingHealth;
    public GameObject cutVFX;
    public float minCuttingThreshold;

    [Header("Cooking Type")] public bool canBeFried;
    public bool canBeMicrowaved;
    public bool canBeRoasted;
    private SkinnedMeshRenderer skMRenderer;
    public string cookingState;
    public float cookingTime;

    [Header("Cooking Time")] [Range(0, 10f)]
    public float fryingTime;

    [Range(0, 10f)] public float microwavingTime;
    [Range(0, 10f)] public float roastingTime;
    [HideInInspector] public bool isCooked;

    public IEnumerator cookingCoroutine;

    private Transform _transform;
    public Rigidbody rb;
    private Collider[] _colliders;
    public bool cuttingMode;

    private void Start()
    {
        //skMRenderer = GetComponent<SkinnedMeshRenderer>()

        _transform = transform;
        rb = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
        cuttingHealth = GetComponent<Health>();
    }

    public void Cook(string toolIdentifier)
    {
        if (cookingCoroutine == null)
        {
            cookingCoroutine = StartCooking(toolIdentifier);
        }

        StartCoroutine(cookingCoroutine);
    }

    public IEnumerator StartCooking(string toolIdentifier)
    {
        if (toolIdentifier == "Pan")
        {
            if ((cookingState == "" ^ cookingState == "Pan") && canBeFried)
            {
                //float cookingPercent = skMRenderer.GetBlendShapeWeight(0);
                if (cookingTime == 0)
                {
                    cookingState = "Pan";
                }

                while (cookingTime < fryingTime)
                {
                    //skMRenderer.SetBlendShapeWeight(0, cookingPercent + 1 / fryingTime);
                    cookingTime += Time.fixedDeltaTime;
                    yield return null;
                }

                isCooked = true;
                print("Fried");
            }
        }

        if (toolIdentifier == "Microwave")
        {
            if ((cookingState == "" ^ cookingState == "Microwave") && canBeMicrowaved)
            {
                //float cookingPercent = skMRenderer.GetBlendShapeWeight(1);
                if (cookingTime == 0)
                {
                    cookingState = "Microwave";
                }

                while (cookingTime < microwavingTime)
                {
                    //skMRenderer.SetBlendShapeWeight(0, cookingPercent + 1 / microwavingTime);
                    cookingTime += Time.fixedDeltaTime;
                    yield return null;
                }

                isCooked = true;
            }
        }

        if (toolIdentifier == "Oven")
        {
            if ((cookingState == "" ^ cookingState == "Oven") && canBeRoasted)
            {
                //float cookingPercent = skMRenderer.GetBlendShapeWeight(0);
                if (cookingTime == 0)
                {
                    cookingState = "Oven";
                }

                while (cookingTime < roastingTime)
                {
                    //skMRenderer.SetBlendShapeWeight(0, cookingPercent + 1 / roastingTime);
                    cookingTime += Time.fixedDeltaTime;
                    yield return null;
                }

                isCooked = true;
            }
        }

        isCooked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cuttingMode)
        {
            if (other.CompareTag("Blade") && other.attachedRigidbody.velocity.magnitude > minCuttingThreshold)
            {
                Debug.Log("Corte");
                cuttingHealth.Hurt(20);
                if (cuttingHealth.currentHealth == 0)
                {
                    Cut();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CuttingHelper tryCHelper;
        if (other.TryGetComponent(out tryCHelper))
        {
            DeactivateCuttingMode();
        }
    }

    public void StopCooking()
    {
        StopCoroutine(cookingCoroutine);
    }

    public string GetFoodIdentifier()
    {
        if (isCooked)
        {
            return cookingState + " " + foodIdentifier;
        }

        return foodIdentifier;
    }

    public void Cut()
    {
        Instantiate(cutVFX, _transform.position, Quaternion.identity, null);
        Instantiate(cutIngredient, transform.position, Quaternion.identity);
        //yield return WaitUntil();
        Destroy(gameObject);
    }

    public void ActivateCuttingMode()
    {
        if (cuttingMode == false&&canBeCut)
        {
            foreach (var coll in _colliders)
            {
                coll.isTrigger = true;
            }

            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            cuttingMode = true;
        }
    }

    public void DeactivateCuttingMode()
    {
        if (cuttingMode && canBeCut)
        {
            foreach (var coll in _colliders)
            {
                coll.isTrigger = false;
            }

            rb.isKinematic = false;
            cuttingMode = false;
        }
    }
}