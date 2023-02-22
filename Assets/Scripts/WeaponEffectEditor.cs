using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

[CustomEditor(typeof(WeaponEffect))]
public class WeaponEffectEditor : Editor
{
    private bool showOnHitEffects = false;
    private bool showOnTravelEffects = false;
    private bool showAudioClips = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WeaponEffect customInspector = (WeaponEffect)target;

        EditorGUILayout.LabelField("Effect List", EditorStyles.whiteLargeLabel);

        showOnHitEffects = EditorGUILayout.Foldout(showOnHitEffects, "OnHitEffects", true);
        if (showOnHitEffects)
        {
            EditorGUI.indentLevel++;
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
                customInspector.slownessRadius =
                    EditorGUILayout.FloatField("Slowness Radius", customInspector.slownessRadius);
                customInspector.stickyVFX =
                    EditorGUILayout.ObjectField("Sticky VFX", customInspector.stickyVFX, typeof(VisualEffect), true) as
                        VisualEffect;

                EditorGUI.indentLevel--;
            }

            customInspector.isNoisy = EditorGUILayout.Toggle("Is Noisy?", customInspector.isNoisy);
            if (customInspector.isNoisy)
            {
                EditorGUI.indentLevel++;
                customInspector.noiseRadius = EditorGUILayout.FloatField("Noise radius", customInspector.noiseRadius);
                customInspector.noiseDuration =
                    EditorGUILayout.Slider("NoiseDuration", customInspector.noiseDuration, 1, 10);
                customInspector.noiseVFX =
                    EditorGUILayout.ObjectField("Noise VFX", customInspector.noiseVFX, typeof(VisualEffect), true) as
                        VisualEffect;
                EditorGUI.indentLevel--;
            }

            customInspector.isFlammable = EditorGUILayout.Toggle("Is Flammable?", customInspector.isFlammable);
            if (customInspector.isFlammable)
            {
                EditorGUI.indentLevel++;
                customInspector.flammableRadius =
                    EditorGUILayout.FloatField("Flammable Volume", customInspector.flammableRadius);
                customInspector.flammableVFX =
                    EditorGUILayout.ObjectField("Flammable VFX", customInspector.flammableVFX, typeof(VisualEffect),
                            true)
                        as VisualEffect;
                EditorGUI.indentLevel--;
            }

            customInspector.destroyOnHit = EditorGUILayout.Toggle("Destroy on hit?", customInspector.destroyOnHit);
            EditorGUI.indentLevel--;
        }

        showOnTravelEffects =
            EditorGUILayout.Foldout(showOnTravelEffects, "OnTravelEffects", true);
        if (showOnTravelEffects)
        {
            customInspector.isBoomerang = EditorGUILayout.Toggle("Is boomerang?", customInspector.isBoomerang);
            if (customInspector.isBoomerang)
            {
                EditorGUI.indentLevel++;
                customInspector.minDotYToLaunch =
                    EditorGUILayout.FloatField("Minimum Dot Y to launch", customInspector.minDotYToLaunch);
                customInspector.projectileVelocity =
                    EditorGUILayout.CurveField("Projectile Velocity", customInspector.projectileVelocity);
                customInspector.parabolaX =
                    EditorGUILayout.CurveField("Parabola X", customInspector.parabolaX);
                customInspector.parabolaZ = EditorGUILayout.CurveField("Parabola Z", customInspector.parabolaZ);
                EditorGUI.indentLevel--;
            }
        }


        showAudioClips = EditorGUILayout.Foldout(showAudioClips, "Audio Clips", true);
        if (showAudioClips)
        {
            customInspector.onThrowClip =
                EditorGUILayout.ObjectField("OnThrow AudioClip", customInspector.onThrowClip, typeof(AudioClip),
                    false) as AudioClip;
            customInspector.onDestroyClip =
                EditorGUILayout.ObjectField("OnDestroy AudioClip", customInspector.onDestroyClip, typeof(AudioClip),
                    false) as AudioClip;
            customInspector.onHandClip =
                EditorGUILayout.ObjectField("OnHand AudioClip", customInspector.onHandClip, typeof(AudioClip),
                    false) as AudioClip;
        }
        /*if (GUILayout.Button("Save current configuration"))
        {
            customInspector.ApplyEffects();
        }*/
    }
}