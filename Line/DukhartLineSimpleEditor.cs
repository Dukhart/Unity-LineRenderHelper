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
    SerializedProperty loops;
    SerializedProperty inFront;
    SerializedProperty graphicsMode;

    bool drawLineGizmos;
    bool drawPointGizmos;

    List<bool> showPoint = new List<bool>();
    List<bool> showExtras = new List<bool>();

    void OnEnable()
    {

        DukhartLineSimple line = (DukhartLineSimple)target;
        points = serializedObject.FindProperty("points");
        pointPrefab = serializedObject.FindProperty("pointPrefab");
        color = serializedObject.FindProperty("color");
        loops = serializedObject.FindProperty("loops");
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
        drawLineGizmos = drawPointGizmos = true;
    }

    public override void OnInspectorGUI()
    {
        DukhartLineSimple line = (DukhartLineSimple)target;
        serializedObject.Update();
        GUILayout.BeginHorizontal("box");
        EditorGUIUtility.labelWidth = 75;
        EditorGUILayout.PropertyField(pointPrefab);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("box", GUILayout.MaxWidth(75));
        EditorGUIUtility.labelWidth = 50;
        EditorGUILayout.PropertyField(loops);
        EditorGUILayout.PropertyField(inFront);
        GUILayout.EndHorizontal();
        line.color = EditorGUILayout.ColorField(line.color);

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Line Gizmo")) {
            line.drawGizmos = !drawLineGizmos;
            drawLineGizmos = !drawLineGizmos;
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        if (GUILayout.Button("Point Gizmos")) {
            for (int i = 0; i < line.points.Count; i++){
                LinePointComponent pointComp = line.points[i].GetComponent<LinePointComponent>();
                pointComp.drawGizmos = !drawPointGizmos;
            }
            drawPointGizmos = !drawPointGizmos;
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
        if (GUILayout.Button("off Gizmos")) {
            line.drawGizmos = false;
            for (int i = 0; i < line.points.Count; i++){
                LinePointComponent pointComp = line.points[i].GetComponent<LinePointComponent>();
                pointComp.drawGizmos = false;
            }
            drawLineGizmos = false;
            drawPointGizmos = false;
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
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        EditorGUILayout.Space();

        // extra data
        showExtras[index] = EditorGUILayout.Foldout(showExtras[index], "Extras");
        GUILayout.EndHorizontal();
        if (showExtras[index]) {
            if (GUILayout.Button("Remove Point")) {
                if (line.RemovePoint(point)) {
                    showPoint.RemoveAt(index);
                showExtras.RemoveAt(index);
                }
            }
        }
    }
}
