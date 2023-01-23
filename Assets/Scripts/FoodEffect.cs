using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "FoodEffect")]
public class FoodEffect : ScriptableObject
{
    [HideInInspector] public bool isExplosive;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public float explosionDamage;
    [HideInInspector] public VisualEffect explosionVFX;

    [HideInInspector] public bool isSticky;
    [HideInInspector] public float slownessPercent;
    [HideInInspector] public bool isNoisy;
    [HideInInspector] public float noiseRadius;
}