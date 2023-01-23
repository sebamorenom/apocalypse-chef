using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTool : MonoBehaviour
{
    [SerializeField] public string toolIdentifier;
    private List<Ingredient> cookingIngredients;
    private List<Coroutine> cookingCoroutines;

    private void OnTriggerEnter(Collider other)
    {
        ICook cookable;
        if (other.TryGetComponent<ICook>(out cookable))
        {
            print("In cooking trigger");
            AddIngredientsToLists(other, cookable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICook cookable;
        if (other.TryGetComponent<ICook>(out cookable))
        {
            print("In cooking trigger");
            int index = cookingIngredients.IndexOf(other.GetComponent<Ingredient>());
            StopCoroutine(cookingCoroutines[index]);
            RemoveIngredientsFromLists(index);
        }
    }
    private void AddIngredientsToLists(Collider other, ICook cookable)
    {
        cookingIngredients.Add(other.GetComponent<Ingredient>());
        cookingCoroutines.Add(StartCoroutine(cookable.Cook(toolIdentifier)));
    }


    private void RemoveIngredientsFromLists(int index)
    {
        cookingIngredients.RemoveAt(index);
        cookingCoroutines.RemoveAt(index);
    }
}