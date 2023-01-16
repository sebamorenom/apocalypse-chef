using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookingTrigger : MonoBehaviour
{
    public UnityEvent onTriggerStay;

    public void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke();
    }
}