using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodProcesser))]
public class CookingTool : MonoBehaviour
{
    [SerializeField] public string toolIdentifier;
    public List<Ingredient> cookingIngredients = new List<Ingredient>(2);
    private List<Coroutine> cookingCoroutines = new List<Coroutine>(2);
    private FoodProcesser fProcesser;

    private void Start()
    {
        fProcesser = GetComponent<FoodProcesser>();
        cookingIngredients.Capacity = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        ICook cookable;
        if (other.TryGetComponent<ICook>(out cookable))
        {
            //print("In cooking trigger");
            AddIngredientsToLists(other, cookable);
            if (cookingIngredients.Count >= 2)
            {
                cookingIngredients.TrimExcess();
                //print("Trying to change food");
                Instantiate(fProcesser.ChangeFood(cookingIngredients[0], cookingIngredients[1]));
                Destroy(RemoveIngredientsFromLists(0));
                Destroy(RemoveIngredientsFromLists(0));
            }
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


    private GameObject RemoveIngredientsFromLists(int index)
    {
        GameObject auxGameObj = cookingIngredients[index].gameObject;
        cookingIngredients.RemoveAt(index);
        cookingCoroutines.RemoveAt(index);
        return auxGameObj;
    }
}