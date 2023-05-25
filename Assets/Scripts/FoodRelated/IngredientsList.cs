using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredients List")]
public class IngredientsList : ScriptableObject
{
    [SerializeField] public Ingredient[] ingredientList;

    public Ingredient GetRandomIngredient()
    {
        return ingredientList[Random.Range(0, ingredientList.Length)];
    }
}