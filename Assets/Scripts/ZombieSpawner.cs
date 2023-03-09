using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject zombie;

    [SerializeField] public float timeBetweenRequests;

    [SerializeField] private Vector2 _timeVariation = new Vector2(0, 1);

    public bool wantZombie;

    // Update is called once per frame
    public void StartSpawning()
    {
        StartCoroutine(RequestZombies());
    }

    public IEnumerator RequestZombies()
    {
        while (!wantZombie)
        {
            wantZombie = true;
            yield return new WaitForSeconds(timeBetweenRequests + Random.Range(_timeVariation.x, _timeVariation.y));
        }
    }

    private void OnValidate()
    {
        if (_timeVariation.x >= _timeVariation.y)
        {
            _timeVariation.x = _timeVariation.y - 0.1f;
        }

        _timeVariation.x = Mathf.Clamp(_timeVariation.x, -10, 0);
        _timeVariation.y = Mathf.Clamp(_timeVariation.y, 0, 10);
    }
}