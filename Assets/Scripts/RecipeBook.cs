using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Recipe
{
    public string recipeName;
    public Ingredient[] ingredients;
}

[ExecuteAlways]
[CreateAssetMenu(menuName = "RecipeBook", fileName = "RecipeBook")]
public class RecipeBook : ScriptableObject
{
    public Recipe[] recipes;
    private Dictionary<string, Ingredient[]> recipeList;

    public void BuildDictionary()
    {
        recipeList = new Dictionary<string, Ingredient[]>();
        foreach (var recipe in recipes)
        {
            recipeList.TryAdd(recipe.recipeName, recipe.ingredients);
        }

        Debug.Log("Dictionary built: " + recipeList.Count + " recipes.");
    }
}