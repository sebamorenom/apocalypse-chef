using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public class Ingredient : MonoBehaviour, ICook
{
    [Header("Ingredient parameters")] public string foodIdentifier;
    [Header("Cut parameters")] public GameObject cutIngredient;
    public float cuttingHealth;
    public VisualEffect cutVFX;

    [Header("Cooking Type")] public bool canBeFried;
    public bool canBeMicrowaved;
    public bool canBeRoasted;
    private SkinnedMeshRenderer skMRenderer;
    public string cookingState;
    public float cookingTime;

    [Header("Cooking Time")]
    [Range(0, 10f)]
    public float fryingTime;

    [Range(0, 10f)] public float microwavingTime;
    [Range(0, 10f)] public float roastingTime;
    [HideInInspector] public bool isCooked;

    private IEnumerator cookingCoroutine;

    private Transform _transform;
    public Rigidbody rb;
    private Grabbable _grab;


    private void Start()
    {
        //skMRenderer = GetComponent<SkinnedMeshRenderer>()

        _transform = transform;
        rb = GetComponent<Rigidbody>();
        _grab = GetComponent<Grabbable>();
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
                    cookingTime += Time.deltaTime;
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
                    cookingTime += Time.deltaTime;
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
                    cookingTime += Time.deltaTime;
                    yield return null;
                }

                isCooked = true;
            }
        }

        isCooked = true;
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
        Instantiate(cutIngredient, transform.position, Quaternion.identity);
        cutVFX?.Play();
        //yield return WaitUntil();
        Destroy((this));
    }
}