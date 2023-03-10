using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [ReadOnly] public float currentHealth;
    [SerializeField] public bool dead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public float Hurt(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (currentHealth == 0) dead = true;

        return currentHealth;
    }


    private float startingTime;
    private float currentTime;

    public IEnumerator Burn(float damagePerTick, float timeOnFlames, float timeBetweenTicks)
    {
        startingTime = Time.fixedTime;
        currentTime = startingTime;

        while (currentTime <= startingTime + timeOnFlames)
        {
            Hurt(damagePerTick);
            yield return new WaitForSeconds(timeBetweenTicks);
        }
    }
}