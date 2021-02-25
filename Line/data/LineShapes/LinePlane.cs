using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePlane : MonoBehaviour
{
    [SerializeField]
    public Vector3[] vertices;
    int[] tris;
    Vector3[] normals;
    Vector2[] uv;
    LinePlane(){
    }
    public void Start()
    {
        //if (meshFilter == null || !meshFilter.mesh)
            //BuildMesh();
    }
    
    public void BuildMesh(List<GameObject> points){
        
        UpdateMeshVerts(points);
        BuildMesh();
    }
    public void BuildMesh(){
        
        if (vertices == null)
            vertices = new Vector3[4]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0)
            };
        //for (int i = 0; i < vertices.GetLength(0); ++i)
            //Debug.Log("verts: " + vertices[i]);
        if (tris == null)
        {
            // calculate tris
            List<int> temp_list = new List<int>();
            int[] p = new int[6]
                {
                // lower left triangle
                0, 2, 1,
                // upper right triangle
                2, 3, 1
                };
            for (int i =0; i < vertices.GetLength(0)/2-1; i++) {
                for (int j = 0; j < 6; j++) {
                    int x = p[j]+(2*i);
                    temp_list.Add(x);
                    //Debug.Log("tris: " + x);
                }
            }
            tris = temp_list.ToArray();
        }
        if (normals == null) {
            List<Vector3> temp_list = new List<Vector3>(vertices.GetLength(0));
            for (int i = 0; i < vertices.GetLength(0); i++) {
                temp_list.Insert(i,-Vector3.forward);
                //Debug.Log("norms: " + temp_list[i]);
            }
            temp_list.TrimExcess();
            normals = temp_list.ToArray();
        }
        if (uv == null){
            Vector2[] temp_uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            List<Vector2> temp_list = new List<Vector2>();
            int j = 0;
            for (int i = 0; i < vertices.GetLength(0); i++){
                temp_list.Add(temp_uv[j]);
                //Debug.Log("uvs: " + temp_uv[j]);
                j += 1;
                j = j >= 4 ? 0:j;
            }
            uv = temp_list.ToArray();
        }
        
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Mesh mesh = new Mesh();
        
        if (!gameObject.GetComponent<MeshRenderer>())
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        else meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (!gameObject.GetComponent<MeshFilter>())
            meshFilter = gameObject.AddComponent<MeshFilter>();
        else meshFilter = gameObject.GetComponent<MeshFilter>();

        if (!meshRenderer.sharedMaterial) {
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        }
        
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;

        meshFilter.mesh = mesh;
        gameObject.AddComponent<MeshCollider>();
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
    public void UpdateMeshVerts (List<GameObject> points){
        // calculate verts
        List<Vector3> vectors = new List<Vector3>();
        for (int i = 0; i < points.Count; ++i) {
            Vector3[] v = CalcPointVertices(points[i]);
            vectors.Add(v[0]);
            vectors.Add(v[1]);
        }
        vertices = vectors.ToArray();
    }
    public void UpdateMeshVert (GameObject point, int index){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        Vector3[] v = CalcPointVertices(point);
        vertices[index*2] = v[0];
        meshFilter.mesh.vertices[index*2] = v[0];
        vertices[index*2+1] = v[1];
        meshFilter.mesh.vertices[index*2+1] = v[1];
        //Debug.Log("verts: " + v[0] +"   "+ v[1]);
    }
    public void UpdateMesh(List<GameObject> points){
        UpdateMeshVerts(points);
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh.vertices = vertices;
    }
    public void UpdateMesh(GameObject point, int index){
        UpdateMeshVert(point,index);
    }
}
