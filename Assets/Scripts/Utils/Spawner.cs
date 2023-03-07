using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnableItem;
    private Hand tryHand;
    private bool itemInside;
    private float halfHeight;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        var boxBounds = GetComponent<Collider>().bounds;
        halfHeight = (boxBounds.max.y - boxBounds.min.y) / 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hand"))
        {
            itemInside = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!itemInside && other.TryGetComponent<Hand>(out tryHand))
        {
            if (tryHand.IsGrabbing())
            {
                Spawn();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Hand"))
        {
            itemInside = false;
        }
    }

    public void Spawn()
    {
        Instantiate(spawnableItem, _transform.position + _transform.up * halfHeight, Quaternion.identity);
        itemInside = true;
    }
}