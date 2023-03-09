using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookingToolType
{
    Pan,
    Oven
};

public class CookingTool : MonoBehaviour
{
    [SerializeField] protected string toolIdentifier;
    [SerializeField] protected int maxFoodItems;
    [SerializeField] protected CookingToolType cookingToolType;
    public List<Ingredient> cookingIngredients;

    protected Ingredient foodToInsert;

    protected void AddIngredientsToList(Collider other)
    {
        if (cookingIngredients.Count <= maxFoodItems)
        {
            foodToInsert = other.GetComponent<Ingredient>();
            cookingIngredients.Insert(0, foodToInsert);
        }
    }


    protected void RemoveIngredientsFromList(int index)
    {
        cookingIngredients.RemoveAt(index);
    }
}