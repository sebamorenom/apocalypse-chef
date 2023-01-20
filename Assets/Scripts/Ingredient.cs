using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ingredient : MonoBehaviour, ICook
{
    [SerializeField] private string ingredientIdentifier;
    [Header("Cooking Type")] public bool canBeFried;
    public bool canBeMicrowaved;
    public bool canBeRoasted;
    private SkinnedMeshRenderer skMRenderer;

    [Header("Cooking Time")] [Range(0, 10f)]
    public float fryingTime;

    [Range(0, 10f)] public float microwavingTime;
    [Range(0, 10f)] public float roastingTime;


    private void Start()
    {
        skMRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void Cook(string toolIdentifier)
    {
        if (toolIdentifier == "Pan" && canBeFried)
        {
            print("Is being fried");
        }

        if (toolIdentifier == "Microwave" && canBeMicrowaved)
        {
            print("Is being microwaved");
        }

        if (toolIdentifier == "Oven" && canBeRoasted)
        {
            print("Is being roasted");
        }
    }
}