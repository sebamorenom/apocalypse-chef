﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTool : MonoBehaviour
{
    [SerializeField] public string toolIdentifier;
    public List<Ingredient> cookingIngredients = new List<Ingredient>(2);
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
                GameObject resulting = fProcesser.ChangeFood(cookingIngredients[0], cookingIngredients[1]);
                Instantiate(resulting, transform.position, Quaternion.identity);
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
            RemoveIngredientsFromLists(index);
        }
    }

    private void AddIngredientsToLists(Collider other, ICook cookable)
    {
        if (cookingIngredients.Count == 2)
        {
            cookingIngredients.RemoveAt(1);
        }

        var foodToInsert = other.GetComponent<Ingredient>();
        if (!foodToInsert.isCooked)
        {
            cookable.Cook(toolIdentifier);
        }

        cookingIngredients.Insert(0, other.GetComponent<Ingredient>());
    }


    private GameObject RemoveIngredientsFromLists(int index)
    {
        GameObject auxGameObj = cookingIngredients[index].gameObject;
        cookingIngredients.RemoveAt(index);
        return auxGameObj;
    }
}