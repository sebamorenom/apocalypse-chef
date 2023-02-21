using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTool : MonoBehaviour
{
    [SerializeField] public string toolIdentifier;
    [SerializeField] private int maxFoodItems;
    public List<Ingredient> cookingIngredients;
    private FoodProcesser fProcesser;

    private void Start()
    {
        fProcesser = GetComponent<FoodProcesser>();
        cookingIngredients = new List<Ingredient>(maxFoodItems);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grill"))
        {
            foreach (var ingredient in cookingIngredients)
            {
                if (!ingredient.isCooked)
                {
                    ingredient.Cook(toolIdentifier);
                }
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            foreach (var ingredient in cookingIngredients)
            {
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICook>(out ICook cookable))
        {
            if (cookingIngredients.Count < maxFoodItems)
            {
                AddIngredientsToLists(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICook cookable;
        if (other.TryGetComponent<ICook>(out cookable))
        {
            cookable.StopCooking();
            int index = cookingIngredients.IndexOf(other.GetComponent<Ingredient>());
            RemoveIngredientsFromLists(index);
        }
    }

    private void AddIngredientsToLists(Collider other)
    {
        var foodToInsert = other.GetComponent<Ingredient>();
        cookingIngredients.Insert(0, foodToInsert);
    }


    private void RemoveIngredientsFromLists(int index)
    {
        cookingIngredients.RemoveAt(index);
    }
}