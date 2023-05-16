using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    private Health _health;

    private void Start()
    {
        _health.GetComponent<Health>();
    }

    private void Update()
    {
        if (_health.dead)
        {
            Destroy(gameObject, timeToDestroy);
        }
    }

    private void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}