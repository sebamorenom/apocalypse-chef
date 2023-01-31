using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public string[] orderIngredients;

    // Start is called before the first frame update
    void Start()
    {
    }

    public Order(int numIngredients)
    {
        orderIngredients = new string[numIngredients];
    }

    // Update is called once per frame
    void Update()
    {
    }
}