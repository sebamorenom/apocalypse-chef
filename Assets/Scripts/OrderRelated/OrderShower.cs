using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class OrderShower : MonoBehaviour
{
    [SerializeField] private int numIngredientsInOrder;
    [SerializeField] private TextMeshProUGUI[] orderText;
    [SerializeField] private Order[] orderList;
    [SerializeField] private GameInfo gameInfo;

    [SerializeField]
    private void Start()
    {
        Director.Instance.orderShower = this;
        Invoke("FillAllOrders", 0.5f);
        FillAllOrders();
    }

    public void CheckCompletion(Plate plateToCheck)
    {
        string foodOnPlateString = GetAsText(plateToCheck);
        Debug.Log("Food on plate: " + foodOnPlateString);
        foreach (Order order in orderList)
        {
            //Debug.Log(GetAsText(order));
            if (foodOnPlateString == GetAsText(order))
            {
                gameInfo.currentDayScore += plateToCheck.plateValue;
                order.Fill(numIngredientsInOrder);
            }
        }
    }

    public void FillAllOrders()
    {
        for (int i = 0; i < orderList.Length; i++)
        {
            orderList[i].text = new TextMeshProUGUI[numIngredientsInOrder];
            for (int j = 0; j < orderList[i].orderIngredients.Length; j++)
            {
                orderList[i].text[j] = orderText[i * orderList[i].orderIngredients.Length + j];
            }

            orderList[i].Fill(numIngredientsInOrder);
        }
    }

    public string GetAsText(Order order)
    {
        string toText = "";
        for (int i = 0; i < order.orderIngredients.Length; i++)
        {
            toText += order.orderIngredients[i];

            toText += "\n";
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

    public int GetOrderPoints()
    {
        return 0;
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