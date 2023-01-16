using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour, ICook
{
    [SerializeField] private string ingredientIdentifier;
    [Header("Cooking Type")] public bool canBeFried;
    public bool canBeMicrowaved;
    public bool canBeRoasted;

    [Header("Cooking Time")] [Range(0, 10f)]
    public float fryingTime, microwavingTime, roastingTime;


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