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
    [SerializeField]
    private int _numSides;
    public int NumSides { 
        get { return _numSides; }
        set {
            if (value < 2) {
                value = 2;
            }
            _numSides = value;
        }
    }
    LineMesh(){
        //_numSides = 2;
    }
    public void BuildMesh(List<GameObject> points, bool loops){
        
        Vector3[] v = CalcMeshVerts(points);
        Debug.Log("Vs");
        Debug.Log(DebugTools.ArrayToString(v));
        BuildMesh(v, loops);
    }
    
    public void BuildMesh(
    Vector3[] vertices,
    bool loops,
    int[] tris = null,
    Vector3[] normals = null,
    Vector2[] uv = null) {
        // check for null input create default plane
        if (vertices == null)
            vertices = MeshBuilder.Default_Verts();
        if (tris == null)
            tris = MeshBuilder.Default_Tris(vertices, _numSides, loops);
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

        meshFilter.sharedMesh = mesh;

        MeshCollider col;
        if (!gameObject.GetComponent<MeshCollider>())
            col = gameObject.AddComponent<MeshCollider>();
        else col = gameObject.GetComponent<MeshCollider>();
        col.sharedMesh = mesh;
    }
    public Vector3[] CalcPointVertices(GameObject point){
        LinePointComponent lc = point.GetComponent<LinePointComponent>();
        Vector3[] verts = new Vector3[NumSides];
        // calculate angle between points
        int angle = 360/NumSides;
        // get the center point
        Vector3 origin = point.transform.localPosition;
        // get the first point
        Vector3 v1 = origin;
        v1.y -= lc.size;
        v1 = VectorMath.RotatePointAroundPivot(v1,origin,point.transform.localRotation);
        for (int i = 0; i < NumSides; i++) {
            
            verts[i] = VectorMath.RotatePointAroundPivot(v1,origin, new Vector3(-angle*i,0,0));
        }
        return verts;
    }
    public Vector3[] CalcMeshVerts (List<GameObject> points){
        // calculate verts
        List<Vector3> vectors = new List<Vector3>();
        for (int i = 0; i < points.Count; ++i) {
            Vector3[] verts = CalcPointVertices(points[i]);
            for (int j = 0; j < verts.GetLength(0); j++)
            {
                vectors.Add(verts[j]);
            }
        }
        return vectors.ToArray();
    }
    public void UpdateMeshVert (GameObject point, int index){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        Vector3[] v = CalcPointVertices(point);
        for (int i = 0; i < v.GetLength(0); i++)
        {
            meshFilter.sharedMesh.vertices[index*2+i] = v[i];
        }
    }
    public void UpdateMesh(List<GameObject> points){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCollider col = gameObject.GetComponent<MeshCollider>();
        Vector3[] verts = CalcMeshVerts(points);
        meshFilter.sharedMesh.vertices = verts;
        col.sharedMesh.vertices = verts;
    }
    public void UpdateMesh(GameObject point, int index){
        UpdateMeshVert(point,index);
    }
}
