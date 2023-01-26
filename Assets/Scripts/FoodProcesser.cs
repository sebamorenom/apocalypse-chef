using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcesser : MonoBehaviour
{
    [SerializeField] public RecipeBook recipeBook;

    private void Start()
    {
        if (recipeBook.recipeList == null)
        {
            recipeBook.BuildDictionary();
        }
    }

    public GameObject ChangeFood(Ingredient ing1, Ingredient ing2)
    {
        print("Changing food");
        return recipeBook.GetResulting(ing1.identifier, ing2.identifier);
    }
}