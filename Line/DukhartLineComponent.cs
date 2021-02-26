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
        LineMesh line = gameObject.GetComponent<LineMesh>();
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
        if (points.Count != mesh.vertices.GetLength(0)/line.NumSides) {
            line.BuildMesh(points);
            Debug.Log("Diff Count");
        }
        else {
            line.UpdateMesh(points);
        }
    }
    void BuildMesh () {
        LineMesh line;
        if (!gameObject.GetComponent<LineMesh>())
            line = gameObject.AddComponent<LineMesh>();
        else line = gameObject.GetComponent<LineMesh>();

        line.BuildMesh(points);
        if (lineMaterial) {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = lineMaterial;
        }
    }
}