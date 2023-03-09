using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : CookingTool
{
    // Start is called before the first frame update
    private void Start()
    {
        cookingIngredients = new List<Ingredient>(maxFoodItems);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            cookingIngredients.Add(other.GetComponent<Ingredient>());
        }
    }

    private Ingredient auxIngredient;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            auxIngredient = other.GetComponent<Ingredient>();
            cookingIngredients.Remove(auxIngredient);
            auxIngredient.StopCooking();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grill"))
        {
            foreach (var ingredient in cookingIngredients)
            {
                ingredient.Cook(toolIdentifier);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Grill"))
        {
            foreach (var ingredient in cookingIngredients)
            {
                ingredient.StopCooking();
            }
        }
    }
}