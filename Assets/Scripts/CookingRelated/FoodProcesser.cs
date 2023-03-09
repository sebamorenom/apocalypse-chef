using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcesser : MonoBehaviour
{
    [SerializeField] public RecipeBook recipeBook;

    private void Start()
    {
        if (recipeBook.recipeList!=null)
        {
            recipeBook.BuildDictionary();
        }
    }

    public GameObject ChangeFood(Ingredient ing1, Ingredient ing2)
    {
        print(recipeBook.recipeList.Count);
        print("Changing food");
        return recipeBook.GetResulting(ing1.foodIdentifier, ing2.foodIdentifier);
    }
}