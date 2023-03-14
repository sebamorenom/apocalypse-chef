using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingIa : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameInfo.objectivesTransform == null)
        {
            gameInfo.CreateArrays();
        }

        gameInfo.objectivesTransform.Insert(0, transform);
        gameInfo.objectivesHealth.Insert(0, GetComponent<Health>());
    }

    // Update is called once per frame
    void Update()
    {
    }
}