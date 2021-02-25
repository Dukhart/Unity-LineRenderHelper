using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLineComponent : DukhartLine
{
    public void Awake(){
        BuildMesh();
    }
    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        RenderLines();
    }
    void RenderLines() {
        LinePlane line = gameObject.GetComponent<LinePlane>();
        //var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
        if (points.Count != line.vertices.GetLength(0)/2) {
            line.BuildMesh(points);
            Debug.Log("Diff Count");
        }
        else {
            line.UpdateMesh(points);
        }
    }
    void BuildMesh () {
        LinePlane line;
        if (!gameObject.GetComponent<LinePlane>())
            line = gameObject.AddComponent<LinePlane>();
        else line = gameObject.GetComponent<LinePlane>();

        line.BuildMesh(points);
        if (lineMaterial) {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = lineMaterial;
        }
    }
}