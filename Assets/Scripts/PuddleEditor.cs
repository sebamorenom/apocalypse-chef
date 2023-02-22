using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        }

        customInspector.isSlippery = EditorGUILayout.Toggle("Is Slippery?", customInspector.isSlippery);
        if (customInspector.isSlippery)
        {
            EditorGUI.indentLevel++;
            customInspector.slippingForce = EditorGUILayout.FloatField("Slipping Force", customInspector.slippingForce);
        }

        customInspector.isFlammable = EditorGUILayout.Toggle("Is Flammable?", customInspector.isFlammable);
        if (customInspector.isFlammable)
        {
            EditorGUI.indentLevel++;
            customInspector.flameDamage = EditorGUILayout.FloatField("Flame Damage", customInspector.flameDamage);
        }
    }
}