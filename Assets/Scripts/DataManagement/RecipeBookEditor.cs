using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RecipeBook))]
public class RecipeBookEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RecipeBook myScript = (RecipeBook)target;

        if (GUILayout.Button("Save Recipe Book"))
        {
            myScript.BuildDictionary();
        }
    }
}