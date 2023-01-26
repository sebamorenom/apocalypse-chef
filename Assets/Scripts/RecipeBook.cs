using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public struct Recipe
{
    public string recipeName;
    public GameObject[] ingredients;
    public GameObject result;
}

[ExecuteAlways]
[CreateAssetMenu(menuName = "RecipeBook", fileName = "RecipeBook")]
public class RecipeBook : ScriptableObject
{
    public Recipe[] recipes;
    public Dictionary<string[], GameObject> recipeList;

    public void BuildDictionary()
    {
        if (recipeList == null)
        {
            recipeList = new Dictionary<String[], GameObject>();
            for (int i = 0; i < recipes.Length; i++)
            {
                string[] ingInRecipe = new string[2];
                for (int j = 0; j < 2; j++)
                {
                    Ingredient ing;
                    if (!recipes[i].ingredients[j].IsUnityNull())
                        ingInRecipe[i] = recipes[i].ingredients[j].GetComponent<Ingredient>().identifier;
                }

                if (recipes[i].ingredients.Length != 2 || !CheckIngredients(recipes[i]))
                {
                    throw new SystemException("Recipe " + i + " does not have the correct amount of ingredients (2)");
                }

                if (recipes[i].result.IsUnityNull())
                {
                    throw new SystemException("Recipe " + i + " does not have a resulting GameObject");
                }

                recipeList.TryAdd(ingInRecipe, recipes[i].result);
            }
        }

        Debug.Log("Dictionary built: " + recipeList.Count + " recipes.");
    }

    private bool CheckIngredients(Recipe recipe)
    {
        for (int i = 0; i < 2; i++)
        {
            if (recipe.ingredients[i].IsUnityNull())
                return false;
        }

        return true;
    }

    public GameObject GetResulting(string ing0, string ing1)
    {
        string[] ingredients = new string[2];
        string[] ingredientsReverse = new string[2];
        ingredients[0] = ingredientsReverse[1] = ing0;
        ingredients[1] = ingredientsReverse[0] = ing1;
        GameObject resulting;

        if (recipeList.TryGetValue(ingredients, out resulting) ||
            recipeList.TryGetValue(ingredientsReverse, out resulting))
        {
            return resulting;
        }

        return null;
    }
}