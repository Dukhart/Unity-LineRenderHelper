using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinePointComponent))]
[CanEditMultipleObjects]
public class LinePointComponentEditor : Editor
{
    int index;
    DukhartLine line;
    bool showExtras;

    void OnEnable()
    {
        if (target){
            LinePointComponent point = (LinePointComponent)target;
            if (point && point.gameObject && point.gameObject.transform.parent){
                index = point.index;
                GameObject go = point.gameObject.transform.parent.gameObject;
                if (go) {
                    line = go.GetComponent<DukhartLine>();
                    line.UpdateMesh();
                }
            }
        }
        showExtras = false;
    }
    public void OnDestroy()
     {
         if (Application.isEditor)
         {
             if((target) == null && line != null) {
                line.RemovePoint(index, false);
             }
         }
     }
    public override void OnInspectorGUI()
    {
        LinePointComponent point = (LinePointComponent)target;
        GameObject go = point.gameObject.transform.parent.gameObject;
        if (go) {
            line = go.GetComponent<DukhartLine>();
            line.UpdateAll();
        }
        DrawPointGUI();
    }
    public void OnSceneGUI()
    {
        LinePointComponent point = (LinePointComponent)target;
        GameObject go = point.gameObject.transform.parent.gameObject;
        if (go) {
            line = go.GetComponent<DukhartLine>();
            line.UpdateMesh();
        }
        
    }
    void DrawPointGUI () {
        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        Vector3 newposi;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        LinePointComponent pointComp = (LinePointComponent)target;
        
        // Draw position data
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("x: ");
        x = EditorGUILayout.FloatField(pointComp.transform.localPosition.x);
        GUILayout.Label("y: ");
        y = EditorGUILayout.FloatField(pointComp.transform.localPosition.y);
        GUILayout.Label("z: ");
        z = EditorGUILayout.FloatField(pointComp.transform.localPosition.z);
        GUILayout.EndHorizontal();
        newposi = new Vector3(x,y,z);
        pointComp.transform.localPosition = newposi;

        // size
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Size");
        pointComp.size = EditorGUILayout.FloatField(pointComp.size);
        GUILayout.Label("index " + index);
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
        showExtras = EditorGUILayout.Foldout(showExtras, "Extras");
        GUILayout.EndHorizontal();
        if (showExtras) {
                GameObject go = pointComp.gameObject.transform.parent.gameObject;
                if (go) {
                    line = go.GetComponent<DukhartLine>();
                }
                if (line) {
                    GUILayout.BeginHorizontal("box");
                    if (GUILayout.Button("<-")){
                        line.ShiftPoint(index, false);
                        index = pointComp.index;
                    }
                    GUILayout.Label("Shift",centeredStyle);
                    if (GUILayout.Button("->")){
                        line.ShiftPoint(index, true);
                        index = pointComp.index;
                    }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal("box");
                    if (GUILayout.Button("<-")){
                        line.AddPoint(index, false);
                    }
                    GUILayout.Label("Add",centeredStyle);
                    if (GUILayout.Button("->")){
                        line.AddPoint(index, true);
                    }
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Remove Point")) {
                    //if (line.RemovePoint(pointComp.gameObject)) {
                    //}
                    DestroyImmediate(pointComp.gameObject);
                }
            }
        }
    }
    
}
