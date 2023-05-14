#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DismembermentTrigger))]
public class DismTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DismembermentTrigger customEditor = (DismembermentTrigger)target;

        EditorGUILayout.LabelField("Limb to dismember", (customEditor.dismemberedLimb).ToString());
        EditorGUILayout.LabelField("Weight percent", (customEditor.weightPercent * 100).ToString() + "%");

        EditorGUILayout.LabelField("Select the part that will be dismembered");
        if (GUILayout.Button("Head"))
        {
            customEditor.dismemberedLimb = DismembermentTrigger.Limb.Head;
            customEditor.UpdateLimb();
        }

        if (GUILayout.Button("Arm"))
        {
            customEditor.dismemberedLimb = DismembermentTrigger.Limb.Arm;
            customEditor.UpdateLimb();
        }

        if (GUILayout.Button("Leg"))
        {
            customEditor.dismemberedLimb = DismembermentTrigger.Limb.Leg;
            customEditor.UpdateLimb();
        }

        customEditor.forceDetach = EditorGUILayout.Toggle("Force detachment", customEditor.forceDetach);
    }
}
#endif