using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public class Ingredient : Weapon, ICook
{
    [Header("Cut parameters")] public GameObject cutIngredient;

    public VisualEffect cutVFX;

    [Header("Cooking Type")] public bool canBeFried;
    public bool canBeMicrowaved;
    public bool canBeRoasted;
    private SkinnedMeshRenderer skMRenderer;
    public string cookingState;
    public float cookingTime;
    public bool isCut;

    [Header("Cooking Time")] [Range(0, 10f)]
    public float fryingTime;

    [Range(0, 10f)] public float microwavingTime;
    [Range(0, 10f)] public float roastingTime;


    [Header("Food Effects")] public FoodEffect foodEffect;


    private Transform _transform;
    private Rigidbody _rb;
    private Grabbable _grab;

    private void Start()
    {
        //skMRenderer = GetComponent<SkinnedMeshRenderer>()
        if (foodEffect != null) onHit += foodEffect.onHit;
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<Grabbable>();
    }

    public IEnumerator Cook(string toolIdentifier)
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
                    cookingTime += Time.deltaTime;
                    yield return null;
                }

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
                    cookingTime += Time.deltaTime;
                    yield return null;
                }
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
                    cookingTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    private void Test()
    {
    }

    public void Cut()
    {
        Instantiate(cutIngredient, transform.position, Quaternion.identity);
        cutVFX?.Play();
        //yield return WaitUntil();
        Destroy((this));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb.velocity.magnitude >= minSpeedOnHitThreshold)
        {
            if (onHit != null) onHit.Invoke();
        }
    }
}