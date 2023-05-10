using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public struct IngredientAnimatorTriggers
{
    public static int ToScream = Animator.StringToHash("ToScream");
}

public class IngredientAnimatorManager : MonoBehaviour
{
    public bool isScreaming;

    public Animator animator;
    public AudioSource audioSource;


    public void Scream()
    {
        animator.SetTrigger(IngredientAnimatorTriggers.ToScream);
        audioSource.Play();
        isScreaming = true;
    }
}