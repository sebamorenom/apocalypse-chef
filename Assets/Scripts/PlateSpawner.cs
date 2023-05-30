using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class PlateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject plate;
    [SerializeField] private float timer;

    private Transform _transform;
    private Hand _tryHand;

    private float _lastSpawnTime = float.MinValue;
    private float halfHeight;
    private bool itemInside;

    private void Start()
    {
        _transform = transform;
        var boxBounds = GetComponent<Collider>().bounds;
        halfHeight = (boxBounds.max.y - boxBounds.min.y) / 2f;
    }

    public void Spawn()
    {
        Instantiate(plate, _transform.position + _transform.up * halfHeight,
            Quaternion.identity);
        itemInside = true;
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
        if (!itemInside && other.TryGetComponent<Hand>(out _tryHand))
        {
            if (Time.fixedTime >=
                _lastSpawnTime + timer)
            {
                Spawn();
                _lastSpawnTime = Time.fixedTime;
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
}