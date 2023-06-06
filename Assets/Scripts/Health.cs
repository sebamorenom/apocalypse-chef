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
    [HideInInspector] public bool burning;

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


    public void StartBurning(float damagePerTick, int numTicks, float timeBetweenTicks)
    {
        StartCoroutine(Burn(damagePerTick, numTicks, timeBetweenTicks));
    }

    private float startingTime;
    private float currentTime;

    private IEnumerator Burn(float damagePerTick, int numTicks, float timeBetweenTicks)
    {
        if (!burning)
        {
            burning = true;
            startingTime = Time.fixedTime;
            currentTime = startingTime;

            while (numTicks > 0)
            {
                Hurt(damagePerTick);
                numTicks = numTicks - 1;
                currentTime += Time.fixedDeltaTime + timeBetweenTicks;
                yield return new WaitForSeconds(timeBetweenTicks);
            }

            burning = false;
        }
    }
}