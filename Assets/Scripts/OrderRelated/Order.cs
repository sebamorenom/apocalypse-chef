using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Order")]
public class Order : ScriptableObject
{
    [HideInInspector] public TextMeshProUGUI[] text;
    public string[] orderIngredients = new string[6];
    public bool breadType;
    public IngredientsList ingList;

    /// <summary>
    /// This method goes through a list of the ingredients of the order and presents them in a text format
    /// </summary>
    /// <returns>
    /// A string containing the identifiers of the ingredients that this order is made of, in the specific order they are placed
    /// </returns>
    public void ToShow()
    {
        for (int i = 0; i < orderIngredients.Length; i++)
        {
            text[i].text = orderIngredients[i];
        }
    }

    public void Fill(int numIngredients)
    {
        Initialize();
        orderIngredients = new string[numIngredients];
        breadType = (Random.value > 0.5f) ? true : false;
        //breadType==true  Bun, Loaf
        for (int i = 0; i < orderIngredients.Length; i++)
        {
            orderIngredients[i] = ingList.GetRandomIngredient().foodIdentifier;
            if (i == 0 || i == numIngredients - 1)
            {
                orderIngredients[i] = breadType ? "Bun" : "Loaf";
            }
        }

        ToShow();
    }

    public void Initialize()
    {
        for (int i = 0; i < orderIngredients.Length; i++)
        {
            orderIngredients[i] = "";
        }
    }
}