using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Order")]
public class Order : ScriptableObject
{
    [SerializeField] private DifficultySettings settings;
    [HideInInspector] public TextMeshProUGUI text;
    public string[] orderIngredients = new string[3];
    public IngredientsList ingList;

    /// <summary>
    /// This method goes through a list of the ingredients of the order and presents them in a text format
    /// </summary>
    /// <returns>
    /// A string containing the identifiers of the ingredients that this order is made of, in the specific order they are placed
    /// </returns>
    public string ToShow()
    {
        string toShow = "";
        for (int i = 0; i < orderIngredients.Length; i++)
        {
            toShow += "-";
            toShow += orderIngredients[i];
            if (i != orderIngredients.Length - 1)
            {
                toShow += "\n";
            }
        }

        return toShow;
    }

    public void Fill()
    {
        orderIngredients = new string[3];
        var i = 0;
        while (i < orderIngredients.Length)
        {
            orderIngredients[i++] = ingList.GetRandomIngredient().foodIdentifier;
        }

        text.text = ToShow();
        Debug.Log(text.text);
    }
}