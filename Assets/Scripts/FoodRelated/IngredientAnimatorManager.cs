using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public struct IngredientAnimatorTriggers
{
    public static int ToIdle = Animator.StringToHash("ToIdle");
    public static int ToScream = Animator.StringToHash("ToScream");
}

public class IngredientAnimatorManager : MonoBehaviour
{
    public bool isScreaming;

    private Animator _animator;
    private Grabbable _grabbable;
    private Weapon _weapon;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _grabbable = GetComponent<Grabbable>();
        _weapon = GetComponent<Weapon>();
    }

    private void Update()
    {
        CheckCurrentStatus();
    }

    private void CheckCurrentStatus()
    {
        if (_grabbable.BeingGrabbed() || _weapon.thrown)
        {
            if (!isScreaming)
            {
                _animator.SetTrigger(IngredientAnimatorTriggers.ToScream);
                isScreaming = true;
            }
        }
        else
        {
            if (isScreaming)
            {
                _animator.SetTrigger(IngredientAnimatorTriggers.ToIdle);
                isScreaming = false;
            }
        }
    }
}