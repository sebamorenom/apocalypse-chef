using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GeneralButton : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onCollisionEnter;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && onTriggerEnter.GetPersistentEventCount() > 0)
        {
            onTriggerEnter.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Hand") && onCollisionEnter.GetPersistentEventCount() > 0)
        {
            onCollisionEnter.Invoke();
        }
    }
}