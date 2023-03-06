using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


[CustomEditor(typeof(Puddle))]
public class PuddleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Puddle customInspector = (Puddle)target;

        EditorGUILayout.LabelField("Puddle Properties", EditorStyles.whiteLargeLabel);
        customInspector.isSlowing = EditorGUILayout.Toggle("Is Slowing?", customInspector.isSlowing);
        if (customInspector.isSlowing)
        {
            EditorGUI.indentLevel++;
            customInspector.speedPercent = EditorGUILayout.FloatField("Speed Percentage", customInspector.speedPercent);
            EditorGUI.indentLevel--;
        }

        customInspector.isSlippery = EditorGUILayout.Toggle("Is Slippery?", customInspector.isSlippery);
        if (customInspector.isSlippery)
        {
            EditorGUI.indentLevel++;
            customInspector.slippingForce = EditorGUILayout.FloatField("Slipping Force", customInspector.slippingForce);
            EditorGUI.indentLevel--;
        }

        customInspector.isFlammable = EditorGUILayout.Toggle("Is Flammable?", customInspector.isFlammable);
        if (customInspector.isFlammable)
        {
            EditorGUI.indentLevel++;
            customInspector.totalFlameDamage =
                EditorGUILayout.FloatField("Total Flame Damage", customInspector.totalFlameDamage);
            customInspector.numTicks = EditorGUILayout.IntField("Number of ticks", customInspector.numTicks);
            customInspector.timeBetweenTicks =
                EditorGUILayout.FloatField("Time between ticks", customInspector.timeBetweenTicks);
            EditorGUI.indentLevel--;
        }

        customInspector.timeAlive = EditorGUILayout.FloatField("Time alive", customInspector.timeAlive);
        if (PrefabStageUtility.GetCurrentPrefabStage())
        {
            if (GUILayout.Button("Save changes"))
            {
                EditorUtility.SetDirty(customInspector);
                PrefabUtility.RecordPrefabInstancePropertyModifications(customInspector);
            }
        }
        ///PrefabUtility.RecordPrefabInstancePropertyModifications(customInspector);
        //EditorUtility.SetDirty(customInspector);
    }
}