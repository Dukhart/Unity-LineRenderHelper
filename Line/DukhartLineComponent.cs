using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLineComponent : DukhartLine
{

    public void Awake() {
        CreateLineMaterial();
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
            line.BuildMesh(points, loops);
            Debug.Log("Diff Count p " + points.Count + " l " + mesh.vertices.GetLength(0) + " n " + line.NumSides+" l/n " + mesh.vertices.GetLength(0)/line.NumSides);
        }
        else {
            line.UpdateMesh(points);
        }
    }
    public void BuildMesh () {
        LineMesh line;
        if (!gameObject.GetComponent<LineMesh>())
            line = gameObject.AddComponent<LineMesh>();
        else line = gameObject.GetComponent<LineMesh>();

        line.BuildMesh(points, loops);

        if (lineMaterial) {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = lineMaterial;
        }
    }
    public override void UpdateMesh(){
        LineMesh line = gameObject.GetComponent<LineMesh>();
        if (line) {
            line.UpdateMesh(points);
        }
    }
}