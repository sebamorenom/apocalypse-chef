using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RecipeBook", fileName = "RecipeBook")]
public class RecipeBook : ScriptableObject
{
    public KeyValuePair<string, GameObject[]> pairs;
    public Dictionary<string, GameObject[]> recipeList;
}