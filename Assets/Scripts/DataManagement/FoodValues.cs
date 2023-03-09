using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct FoodValue
{
    public string foodName;
    public int foodValue;
}

[CreateAssetMenu(menuName = "Food Values")]
public class FoodValues : ScriptableObject
{
    public Dictionary<string, int> foodValueDictionary;

    public FoodValue[] foodValues;

    public void OnValidate()
    {
        ToDictionary();
    }

    public int GetFoodValue(string foodName)
    {
        return foodValueDictionary[foodName];
    }

    public int GetFoodValue(string[] foodNames)
    {
        int value = 0;
        foreach (var fName in foodNames)
        {
            value += foodValueDictionary[fName];
        }

        return value;
    }

    private void ToDictionary()
    {
        foodValueDictionary = new Dictionary<string, int>();
        foreach (var foodValue in foodValues)
        {
            foodValueDictionary.Add(foodValue.foodName, foodValue.foodValue);
        }
    }
}