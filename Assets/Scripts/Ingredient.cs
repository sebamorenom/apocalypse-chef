using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour, ICook
{
    [SerializeField] private string ingredientIdentifier;
    [Header("CookingType")] bool canBeFried;
    bool canBeMicrowaved;
    bool canBeRoasted;

    public void Cook(string toolIdentifier)
    {
        if (toolIdentifier == "Pan" && canBeFried)
        {
        }

        if (toolIdentifier == "Microwave" && canBeMicrowaved)
        {
        }

        if (toolIdentifier == "Oven" && canBeRoasted)
        {
        }
    }
}