using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

[CustomEditor(typeof(FoodEffect))]
public class FoodEffectEditor : Editor
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
            customInspector.explosionForce =
                EditorGUILayout.FloatField("Explosion Force", customInspector.explosionForce);
            customInspector.explosionDamage =
                EditorGUILayout.FloatField("Explosive Damage", customInspector.explosionDamage);
            customInspector.explosionVFX =
                EditorGUILayout.ObjectField("Explosion VFX", customInspector.explosionVFX, typeof(VisualEffect),
                        true)
                    as VisualEffect;
            EditorGUI.indentLevel--;
        }

        customInspector.isSticky = EditorGUILayout.Toggle("Is Sticky?", customInspector.isSticky);
        if (customInspector.isSticky)
        {
            EditorGUI.indentLevel++;
            customInspector.slownessPercent =
                EditorGUILayout.Slider("Slowness Percent", customInspector.slownessPercent, 0f, 100f);
            customInspector.stickyVFX = EditorGUILayout.ObjectField("Sticky VFX", customInspector.stickyVFX, typeof(VisualEffect), true) as VisualEffect;

            EditorGUI.indentLevel--;
        }

        customInspector.isNoisy = EditorGUILayout.Toggle("Is Noisy?", customInspector.isNoisy);
        if (customInspector.isNoisy)
        {
            EditorGUI.indentLevel++;
            customInspector.noiseRadius = EditorGUILayout.FloatField("Noise radius", customInspector.noiseRadius);
            customInspector.noiseVFX = EditorGUILayout.ObjectField("Noise VFX", customInspector.noiseVFX, typeof(VisualEffect), true) as VisualEffect;
            EditorGUI.indentLevel--;
        }

        customInspector.isFlammable = EditorGUILayout.Toggle("Is Flammable?", customInspector.isFlammable);
        if (customInspector.isFlammable)
        {
            EditorGUI.indentLevel++;
            customInspector.flammableRadius =
                EditorGUILayout.FloatField("Flammable Volume", customInspector.flammableRadius);
            customInspector.flammableVFX = EditorGUILayout.ObjectField("Flammable VFX", customInspector.flammableVFX, typeof(VisualEffect), true) as VisualEffect;
            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Save current configuration"))
        {
            customInspector.ApplyEffects();
        }
    }
}