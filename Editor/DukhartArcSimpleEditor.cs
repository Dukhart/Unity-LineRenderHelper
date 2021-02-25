using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(DukhartArcSimple))]
[CanEditMultipleObjects]
public class DukhartArcSimpleEditor : Editor
{
    SerializedProperty lineMaterial;
    SerializedProperty color;
    SerializedProperty length;
    SerializedProperty resolution;
    SerializedProperty angle;
    SerializedProperty drawGizmos;
    
    void OnEnable()
    {
        DukhartArcSimple arc = (DukhartArcSimple)target;
        lineMaterial = serializedObject.FindProperty("lineMaterial");
        color = serializedObject.FindProperty("color");
        length = serializedObject.FindProperty("length");
        resolution = serializedObject.FindProperty("resolution");
        angle = serializedObject.FindProperty("angle");
        color = serializedObject.FindProperty("color");
        drawGizmos = serializedObject.FindProperty("drawGizmos");
    }
    public override void OnInspectorGUI()
    {
        DukhartArcSimple arc = (DukhartArcSimple)target;
        if (arc == null) return;
        int newRes = arc.resolution;
        serializedObject.Update();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Arc");
        GUILayout.BeginHorizontal("Box");
            
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.PropertyField(length);
            EditorGUIUtility.labelWidth = 100;
            //EditorGUILayout.PropertyField(resolution);
            newRes = EditorGUILayout.IntField("Resolution",newRes);
            if (newRes != arc.resolution) {
                if (newRes <= 1) {
                newRes = 1;
                }
                arc.resolution = newRes;
                EditorWindow view = EditorWindow.GetWindow<SceneView>();
                if (view)
                    view.Repaint();
            }
        GUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = 50;
            EditorGUIUtility.fieldWidth = 50;
            EditorGUILayout.PropertyField(angle);
            EditorGUILayout.PropertyField(color);
            EditorGUIUtility.labelWidth = 80;
            EditorGUILayout.PropertyField(drawGizmos);
        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
