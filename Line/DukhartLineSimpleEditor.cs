using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(DukhartLineSimple))]
[CanEditMultipleObjects]
public class DukhartLineSimpleEditor : Editor
{
    SerializedProperty points;
    SerializedProperty pointPrefab;
    SerializedProperty color;
    SerializedProperty drawGizmos;
    SerializedProperty loops;
    SerializedProperty inFront;
    SerializedProperty graphicsMode;

    List<bool> showPoint = new List<bool>();
    List<bool> showExtras = new List<bool>();

    void OnEnable()
    {
        DukhartLineSimple line = (DukhartLineSimple)target;
        points = serializedObject.FindProperty("points");
        pointPrefab = serializedObject.FindProperty("pointPrefab");
        color = serializedObject.FindProperty("color");
        drawGizmos = serializedObject.FindProperty("drawGizmos");
        loops = serializedObject.FindProperty("loops");
        graphicsMode = serializedObject.FindProperty("graphicsMode");
        inFront = serializedObject.FindProperty("inFront");

        //if (showPoint) showPoint = new List<bool>();
        showPoint.Clear();
        //if (!showPoint) showExtras = new List<bool>();
        showExtras.Clear();
        if (line.points != null) {
            for (int i = 0; i < line.points.Count; i++) {
                showPoint.Add(false);
                showExtras.Add(false);
            }
        }
        
    }

    public override void OnInspectorGUI()
    {
        DukhartLineSimple line = (DukhartLineSimple)target;
        serializedObject.Update();
        GUILayout.BeginHorizontal("box");
        EditorGUIUtility.labelWidth = 75;
        EditorGUILayout.PropertyField(pointPrefab);
        EditorGUIUtility.labelWidth = 90;
        EditorGUILayout.PropertyField(graphicsMode);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("box", GUILayout.MaxWidth(75));
        EditorGUIUtility.labelWidth = 50;
        EditorGUILayout.PropertyField(loops);
        EditorGUILayout.PropertyField(inFront);
        GUILayout.EndHorizontal();
        line.color = EditorGUILayout.ColorField(line.color);
        GUILayout.BeginHorizontal("box", GUILayout.MaxWidth(100));
        EditorGUIUtility.labelWidth = 100;
        EditorGUILayout.PropertyField(drawGizmos);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Toggle Point Gizmos")) {
            for (int i = 0; i < line.points.Count; i++){
                LinePointComponent pointComp = line.points[i].GetComponent<LinePointComponent>();
                pointComp.drawGizmos = !pointComp.drawGizmos;
            }
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        if (GUILayout.Button("off Gizmos")) {
            line.drawGizmos = false;
            for (int i = 0; i < line.points.Count; i++){
                LinePointComponent pointComp = line.points[i].GetComponent<LinePointComponent>();
                pointComp.drawGizmos = false;
            }
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("points");
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Add Point")) {
            line.AddPoint();
            showPoint.Add(true);
            showExtras.Add(false);
        }
        if (GUILayout.Button("Remove Point")) {
            if (line.RemovePoint(showPoint.Count - 1)) {
                showPoint.RemoveAt(showPoint.Count - 1);
                showExtras.RemoveAt(showPoint.Count - 1);
            }
                
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < line.points.Count; i++) {
            showPoint[i] = EditorGUILayout.Foldout(showPoint[i], "Point " + i);
            if (showPoint[i]) {
                DrawPointGUI(line, i);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }


    public void DrawPointGUI (DukhartLineSimple line, int index) {
        GameObject point = line.points[index];
        Vector3 newposi;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        LinePointComponent pointComp = point.GetComponent<LinePointComponent>();

        // Draw position data
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("x: ");
        x = EditorGUILayout.FloatField(point.transform.position.x);
        GUILayout.Label("y: ");
        y = EditorGUILayout.FloatField(point.transform.position.y);
        GUILayout.Label("z: ");
        z = EditorGUILayout.FloatField(point.transform.position.z);
        GUILayout.EndHorizontal();
        newposi = new Vector3(x,y,z);
        point.transform.position = newposi;

        // size
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Size");
        pointComp.size = EditorGUILayout.FloatField(pointComp.size);
        GUILayout.EndHorizontal();

        // color
        GUILayout.BeginHorizontal("box");
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Color");
        pointComp.color = EditorGUILayout.ColorField(pointComp.color);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Tangent Color");
        pointComp.tanColor = EditorGUILayout.ColorField(pointComp.tanColor);
        EditorGUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        EditorGUILayout.Space();

        // extra data
        showExtras[index] = EditorGUILayout.Foldout(showExtras[index], "Extras");
        GUILayout.EndHorizontal();
        if (showExtras[index]) {
            Vector2 v2;
            GUILayout.BeginHorizontal("box");
            //width
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Width");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("in");
            pointComp.inWidth = EditorGUILayout.FloatField(pointComp.inWidth);
            GUILayout.Label("out");
            pointComp.outWidth = EditorGUILayout.FloatField(pointComp.outWidth);
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
            // strength
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Strength");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("in");
            pointComp.inStrength = EditorGUILayout.FloatField(pointComp.inStrength);
            GUILayout.Label("out");
            pointComp.outStrength = EditorGUILayout.FloatField(pointComp.outStrength);
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            // in angle
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("In Angle");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("x");
            x = EditorGUILayout.FloatField(pointComp.inAngle.x);
            GUILayout.Label("y");
            y = EditorGUILayout.FloatField(pointComp.inAngle.y);
            v2 = new Vector2(x,y);
            pointComp.inAngle = v2;
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // out angle
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Out Angle");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("x");
            x = EditorGUILayout.FloatField(pointComp.outAngle.x);
            GUILayout.Label("y");
            y = EditorGUILayout.FloatField(pointComp.outAngle.y);
            v2 = new Vector2(x,y);
            pointComp.outAngle = v2;
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Remove Point")) {
            if (line.RemovePoint(point)) {
                showPoint.RemoveAt(index);
                showExtras.RemoveAt(index);
            }
                
        }
        }
        
    }
}
