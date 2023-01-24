using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "FoodEffect")]
public class FoodEffect : ScriptableObject
{
    [HideInInspector] public bool isExplosive;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public float explosionForce;
    [HideInInspector] public float explosionDamage;
    [HideInInspector] public VisualEffect explosionVFX;

    [HideInInspector] public bool isSticky;
    [HideInInspector] public float slownessPercent;

    [HideInInspector] public bool isNoisy;
    [HideInInspector] public float noiseRadius;

    [HideInInspector] public bool isFlammable;
    [HideInInspector] public float flammableRadius;

    [HideInInspector] public Transform fTransform;
    [HideInInspector] public Collider fCollider;

    public void OnHit()
    {
    }

    public void Explode()
    {
        var affected = Physics.OverlapSphere(fTransform.position, explosionRadius);
        IDamageable tryEnemy;
        Rigidbody tryRb;
        foreach (var entity in affected)
        {
            if (entity.TryGetComponent<IDamageable>(out tryEnemy))
            {
                tryEnemy.Hurt(explosionDamage);
            }

            if (entity.TryGetComponent<Rigidbody>(out tryRb))
            {
                tryRb.AddExplosionForce(explosionForce, fTransform.position, explosionRadius);
            }
        }
    }

    public void ApplyEffects()
    {
        Debug.Log("Saved");
    }
}