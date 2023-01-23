using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcesser : MonoBehaviour
{
    public static RecipeBook recipeBook;

    private void Start()
    {
        
    }

    public static GameObject ChangeFood(Ingredient ing1, Ingredient ing2)
    {
        return recipeBook.GetResulting(ing1, ing2);
    }
}