using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
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
    public Dictionary<string, GameObject> recipeList;

    public void BuildDictionary()
    {
        if (recipeList == null)
        {
            recipeList = new Dictionary<string, GameObject>();
            for (int i = 0; i < recipes.Length; i++)
            {
                string ingInRecipe = "";
                for (int j = 0; j < 2; j++)
                {
                    Ingredient ing;
                    if (recipes[i].ingredients[j]!=null)
                        ingInRecipe += recipes[i].ingredients[j].GetComponent<Ingredient>().foodIdentifier;
                    if (j == 0)
                    {
                        ingInRecipe += "+";
                    }
                }

                if (recipes[i].ingredients.Length != 2 || !CheckIngredients(recipes[i]))
                {
                    throw new SystemException("Recipe " + i + " does not have the correct amount of ingredients (2)");
                }

                if (recipes[i].result==null)
                {
                    throw new SystemException("Recipe " + i + " does not have a resulting GameObject");
                }

                Debug.Log(ingInRecipe);
                recipeList.TryAdd(ingInRecipe, recipes[i].result);
            }
        }

        Debug.Log("Dictionary built: " + recipeList.Count + " recipes.");
    }

    private bool CheckIngredients(Recipe recipe)
    {
        for (int i = 0; i < 2; i++)
        {
            if (recipe.ingredients[i]==null)
                return false;
        }

        return true;
    }

    public GameObject GetResulting(string ing0, string ing1)
    {
        string ingredients = ing0 + "+" + ing1;
        string ingredientsReverse = ing1 + "+" + ing0;
        GameObject resulting;

        Debug.Log("Ingredients: " + ingredients + "\n" + "Ingredients in Reverse: " + ingredientsReverse);
        if (recipeList.TryGetValue(ingredients, out resulting) ||
            recipeList.TryGetValue(ingredientsReverse, out resulting))
        {
            Debug.Log("Food changed");
            return resulting;
        }

        return null;
    }
}