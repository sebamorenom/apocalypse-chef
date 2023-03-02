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
    private bool showPuddleEffects = false;
    private bool showOnTravelEffects = false;
    private bool showAudioClips = false;

    private Editor puddleEditor;

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

            customInspector.spawnPuddle = EditorGUILayout.Toggle("Spawn Puddle", customInspector.spawnPuddle);
            if (customInspector.spawnPuddle)
            {
                EditorGUI.indentLevel++;
                showPuddleEffects = EditorGUILayout.Foldout(showPuddleEffects, "Show Puddle effects", true);
                {
                    if (showPuddleEffects)
                    {
                        customInspector.puddleGameObject =
                            EditorGUILayout.ObjectField("Puddle to spawn", customInspector.puddleGameObject,
                                typeof(GameObject), false) as GameObject;
                        if (!customInspector.puddleGameObject.IsUnityNull() &&
                            customInspector.puddleGameObject.TryGetComponent<Puddle>(out var puddle))
                        {
                            customInspector.puddleProperties = customInspector.puddleGameObject.GetComponent<Puddle>();
                            {
                                if (!customInspector.puddleProperties.IsUnityNull())
                                {
                                    customInspector.puddleProperties = EditorGUILayout.ObjectField("Puddle properties",
                                        customInspector.puddleGameObject.GetComponent<Puddle>(), typeof(Puddle),
                                        false) as Puddle;
                                    puddleEditor = Editor.CreateEditor(customInspector.puddleProperties);
                                    var root = puddleEditor.CreateInspectorGUI();
                                    puddleEditor.OnInspectorGUI();
                                    Repaint();
                                }
                            }
                        }
                    }
                }
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