using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTool : MonoBehaviour
{
    [SerializeField] public string toolIdentifier;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        ICook cookable;
        print("Trigger enter");
        if (other.TryGetComponent<ICook>(out cookable))
        {
            print("In cooking trigger");
            cookable.Cook(toolIdentifier);
        }
    }
}