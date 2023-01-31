using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredients List")]
public class IngredientsList : ScriptableObject
{
    [SerializeField] private string[] ingredientList;

    public string GetRandomIngredient()
    {
        return ingredientList[Random.Range(0, ingredientList.Length)];
    }
}