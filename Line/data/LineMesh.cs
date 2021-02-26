using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LineMesh : MonoBehaviour
{
    // never less than 2 sides
    // 1 and 2 sides need the same verts to make a plane, just use 2 for 1 sided
    // the math for 1 sided will decimate the plane
    // shader/material can be used for 2 sided or 1 sided line effect.
    private int _numSides;
    public int NumSides { 
        get { return _numSides; }
        set {
            if (value < 2) value = 2;
            _numSides = value;
        }
    }
    LineMesh(){
        _numSides = 2;
    }
    public void BuildMesh(List<GameObject> points){
        
        Vector3[] v = CalcMeshVerts(points);
        BuildMesh(v);
    }
    
    public void BuildMesh(
    Vector3[] vertices = null, 
    int[] tris = null,
    Vector3[] normals = null,
    Vector2[] uv = null) {
        // check for null input create default plane
        if (vertices == null)
            vertices = MeshBuilder.Default_Verts();
        if (tris == null)
            tris = MeshBuilder.Default_Tris(vertices, _numSides);
        if (normals == null)
            normals = MeshBuilder.Default_Normal(vertices);
        if (uv == null)
            uv = MeshBuilder.Default_UV(vertices);
        
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Mesh mesh = new Mesh();
        
        if (!gameObject.GetComponent<MeshRenderer>())
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        else meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (!gameObject.GetComponent<MeshFilter>())
            meshFilter = gameObject.AddComponent<MeshFilter>();
        else meshFilter = gameObject.GetComponent<MeshFilter>();

        if (!meshRenderer.sharedMaterial)
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
    public Vector3[] CalcPointVertices(GameObject point){
        Vector3[] verts = new Vector3[2];
        LinePointComponent lc = point.GetComponent<LinePointComponent>();
        Vector3 v1;
        Vector3 v2;

        v1 = v2 = point.transform.localPosition;

        v1.y += lc.size;
        v2.y -= lc.size;

        verts[0] = v1;
        verts[1] = v2;

        return verts;
    }
    public Vector3[] CalcMeshVerts (List<GameObject> points){
        // calculate verts
        List<Vector3> vectors = new List<Vector3>();
        for (int i = 0; i < points.Count; ++i) {
            Vector3[] v = CalcPointVertices(points[i]);
            vectors.Add(v[0]);
            vectors.Add(v[1]);
        }
        return vectors.ToArray();
    }
    public void UpdateMeshVert (GameObject point, int index){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        Vector3[] v = CalcPointVertices(point);
        //vertices[index*2] = v[0];
        meshFilter.mesh.vertices[index*2] = v[0];
        //vertices[index*2+1] = v[1];
        meshFilter.mesh.vertices[index*2+1] = v[1];
        //Debug.Log("verts: " + v[0] +"   "+ v[1]);
    }
    public void UpdateMesh(List<GameObject> points){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh.vertices = CalcMeshVerts(points);
    }
    public void UpdateMesh(GameObject point, int index){
        UpdateMeshVert(point,index);
    }
}
