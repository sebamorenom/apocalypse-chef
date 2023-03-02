using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OrderShower : MonoBehaviour
{
    [SerializeField] private OrderCreator orderCreator;
    [SerializeField] private GameObject orderGameObject;
    [SerializeField] private TextMeshProUGUI[] orderText;
    [SerializeField] private Order[] orderList;
    [SerializeField] private GameInfo gameInfo;

    private void Start()
    {
        for (int i = 0; i < orderList.Length; i++)
        {
            orderList[i].text = orderText[i];
            orderList[i].Fill();
        }
    }

    public void CheckCompletion(Plate plateToCheck)
    {
        string foodOnPlateString = GetAsText(plateToCheck);
        Debug.Log("Food on plate: " + foodOnPlateString);
        foreach (Order order in orderList)
        {
            Debug.Log(GetAsText(order));
            if (foodOnPlateString == GetAsText(order))
            {
                order.Fill();
            }
        }
    }

    public string GetAsText(Order order)
    {
        string toText = "";
        for (int i = 0; i < order.orderIngredients.Length; i++)
        {
            toText += order.orderIngredients[i];
            if (i != order.orderIngredients.Length - 1)
            {
                toText += "\n";
            }
        }

        return toText;
    }

    public string GetAsText(Plate plate)
    {
        string toText = "";
        string[] foodOnPlate = plate.GetFoodOnPlate();
        /*
         foreach (var test in foodOnPlate)
        {
            Debug.Log(order.orderIngredients);
        }
        */

        for (int i = 0; i < foodOnPlate.Length; i++)
        {
            toText += foodOnPlate[i];
            if (i != foodOnPlate.Length - 1)
            {
                toText += "\n";
            }
        }

        return toText;
    }

    private void OnTriggerStay(Collider other)
    {
        //if (checking)
        //{
        Plate tryPlate;
        if (other.TryGetComponent<Plate>(out tryPlate))
        {
            CheckCompletion(tryPlate);
            Destroy(other.gameObject);
        }

        //}
    }
}