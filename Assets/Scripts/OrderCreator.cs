using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCreator : MonoBehaviour
{
    public DifficultySettings settings;

    public Order CreateOrder()
    {
        /* if (settings.difficulty == "Easy")
         {
             return new Order(Random.Range(3, 6));
         }
 
         if (settings.difficulty == "Medium")
         {
             return new Order(Random.Range(6, 9));
         }
 
         if (settings.difficulty == "Difficult")
         {
             return new Order(Random.Range(8, 11));
         }
 
         if (settings.difficulty == "GodMode")
         {
             return new Order(15);
         }
 
         return new Order(Random.Range(0, 11));
 */
        return null;
    }
}