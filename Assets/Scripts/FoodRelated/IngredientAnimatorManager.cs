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

    public Animator animator;
    public AudioSource audioSource;
    private Grabbable _grabbable;
    private Weapon _weapon;


    private void Start()
    {
        _grabbable = GetComponent<Grabbable>();
        _weapon = GetComponent<Weapon>();
    }

    private void Update()
    {
        CheckCurrentStatus();
    }
    
    private void Scream()
    {
    if (!isScreaming)
                {
                    animator.SetTrigger(IngredientAnimatorTriggers.ToScream);
                    audioSource.Play();
                    isScreaming = true;
                }
    }

    private void CheckCurrentStatus()
    {
        if (_grabbable.BeingGrabbed() || _weapon.thrown)
        {
            if (!isScreaming)
            {
                animator.SetTrigger(IngredientAnimatorTriggers.ToScream);
                audioSource.Play();
                isScreaming = true;
            }
        }
        else
        {
            if (isScreaming)
            {
                animator.SetTrigger(IngredientAnimatorTriggers.ToIdle);
                audioSource.Stop();
                isScreaming = false;
            }
        }
    }
}