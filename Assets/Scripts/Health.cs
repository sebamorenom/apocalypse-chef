using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [ReadOnly] private float currentHealth;
    [SerializeField] public bool dead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Hurt(float damage)
    {
        Mathf.Max(currentHealth - damage, 0);
        if (currentHealth == 0) dead = true;
    }
}