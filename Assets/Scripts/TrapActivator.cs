using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivator : MonoBehaviour
{
    [SerializeField] private Trap trapAssigned;
    [SerializeField] private Transform button;
    [SerializeField] private Vector3 pressedPosition;
    [SerializeField] private Vector3 unpressedPosition;


    // Update is called once per frame
    void Update()
    {
        if (button.position.y <= pressedPosition.y)
        {
            SwitchTrapState();
        }
    }

    private void FixedUpdate()
    {
        if (button.position.y <= pressedPosition.y)
        {
            button.position = Vector3.MoveTowards(button.position, unpressedPosition, 0.01f * Time.fixedDeltaTime);
        }
    }

    private void SwitchTrapState()
    {
        trapAssigned.SwitchState();
    }
}