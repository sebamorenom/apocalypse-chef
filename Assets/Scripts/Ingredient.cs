using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ingredient : Weapon, ICook
{
    [SerializeField] private string ingredientIdentifier;
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


    private void Start()
    {
        //skMRenderer = GetComponent<SkinnedMeshRenderer>();
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

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable;
    }
}