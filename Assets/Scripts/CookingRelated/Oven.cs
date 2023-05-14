using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Oven : CookingTool
{
    [SerializeField] public Collider handleCollider;


    // Start is called before the first frame update
    private void Start()
    {
        cookingIngredients = new List<Ingredient>(maxFoodItems);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other == handleCollider && cookingIngredients.Count > 0)
        {
            foreach (var ingredient in cookingIngredients)
            {
                ingredient.Cook(toolIdentifier);
            }
        }
        else if (other.CompareTag("Food"))
        {
            AddIngredientsToList(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == handleCollider)
        {
            foreach (var ingredient in cookingIngredients)
            {
                ingredient.StopCooking();
            }
        }
        else if (other.CompareTag("Food"))
        {
            RemoveIngredientsFromList(cookingIngredients.IndexOf(other.GetComponent<Ingredient>()));
        }
    }
}