using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

[CustomEditor(typeof(FoodEffect))]
public class EffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FoodEffect customInspector = (FoodEffect)target;
        customInspector.isExplosive = EditorGUILayout.Toggle("Is Explosive?", customInspector.isExplosive);

        if (customInspector.isExplosive)
        {
            EditorGUI.indentLevel++;
            customInspector.explosionRadius =
                EditorGUILayout.FloatField("Explosive Radius", customInspector.explosionRadius);
            customInspector.explosionDamage =
                EditorGUILayout.FloatField("Explosive Damage", customInspector.explosionDamage);
            customInspector.explosionVFX =
                EditorGUILayout.ObjectField("Explosion VFX", customInspector.explosionVFX, typeof(VisualEffect), true)
                    as VisualEffect;
            EditorGUI.indentLevel--;
        }

        customInspector.isSticky = EditorGUILayout.Toggle("Is Sticky?", customInspector.isSticky);
        if (customInspector.isSticky)
        {
            EditorGUI.indentLevel++;
            customInspector.slownessPercent =
                EditorGUILayout.Slider("Slowness Percent", customInspector.slownessPercent, 0f, 100f);
            EditorGUI.indentLevel--;
        }

        if (customInspector.isNoisy)
        {
            EditorGUI.indentLevel++;
            customInspector.noiseRadius = EditorGUILayout.FloatField("Noise radius", customInspector.noiseRadius);
            EditorGUI.indentLevel--;
        }
    }
}