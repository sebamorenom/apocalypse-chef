using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Autohand;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnableItem;
    [SerializeField] private float spawnTimer;
    private Hand tryHand;
    private bool itemInside;
    private float halfHeight;

    private Transform _transform;

    private float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.fixedTime;
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
            if (Time.fixedTime >= lastSpawnTime + spawnTimer)
            {
                Spawn();
                lastSpawnTime = Time.fixedTime;
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