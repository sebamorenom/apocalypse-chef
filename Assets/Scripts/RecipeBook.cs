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
    private Dictionary<Ingredient[], GameObject> recipeList;

    public void BuildDictionary()
    {
        recipeList = new Dictionary<Ingredient[], GameObject>();
        for (int i = 0; i < recipes.Length; i++)
        {
            Ingredient[] ingInRecipe = new Ingredient[2];
            for (int j = 0; j < 2; j++)
            {
                Ingredient ing;
                if (!recipes[i].ingredients[j].IsUnityNull() &&
                    recipes[i].ingredients[j].TryGetComponent<Ingredient>(out ing))
                    ingInRecipe[i] = ing;
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

    public GameObject GetResulting(Ingredient ing0, Ingredient ing1)
    {
        Ingredient[] ingredients = new Ingredient[2];
        Ingredient[] ingredientsReverse = new Ingredient[2];
        ingredients[0] = ingredientsReverse[1] = ing1;
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