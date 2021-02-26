using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuadMesh : MonoBehaviour
{
    MeshFilter meshFilter;
    [SerializeField]
    public float width;
    [SerializeField]
    public float height;
    Vector3[] vertices;
    int[] tris = new int[6]
    {
        // lower left triangle
        0, 2, 1,
        // upper right triangle
        2, 3, 1
    };
    Vector3[] normals;
    Vector2[] uv;
    QuadMesh(){
        width = 1;
        height = 1;
        normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
    }
    public void Awake()
    {
        if (meshFilter == null || !meshFilter.mesh)
            BuildMesh();
    }
    void BuildMesh(){
        MeshRenderer meshRenderer;
        Mesh mesh = new Mesh();
        vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        
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
}
