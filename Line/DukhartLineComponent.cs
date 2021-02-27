using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLineComponent : DukhartLine
{
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
    [SerializeField]
    bool hasCollision;

    DukhartLineComponent(){
        NumSides = 2;
        hasCollision = false;
    }
    public void Awake() {
        BuildMesh();
        AssignMaterial();
        if (hasCollision) AddCollider();
    }
    public override void Rebuild() {
        if (gameObject.GetComponent<MeshFilter>())
        {
            if (Application.isEditor){
                DestroyImmediate(gameObject.GetComponent<MeshFilter>());
            } else {
                Destroy(gameObject.GetComponent<MeshFilter>());
            }
        }  
        if (gameObject.GetComponent<MeshRenderer>())
        {
            if (Application.isEditor){
                DestroyImmediate(gameObject.GetComponent<MeshRenderer>());
            } else {
                Destroy(gameObject.GetComponent<MeshRenderer>());
            }
        }
        if (gameObject.GetComponent<MeshCollider>())
        {
            if (Application.isEditor){
                DestroyImmediate(gameObject.GetComponent<MeshCollider>());
            } else {
                Destroy(gameObject.GetComponent<MeshCollider>());
            }
        }
        BuildMesh();
        AssignMaterial();
        if (hasCollision) AddCollider();
    }
    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        RenderLines();
    }
    void RenderLines() {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
        if (points.Count != mesh.vertices.GetLength(0)/NumSides) {
            BuildMesh();
            Debug.Log("Diff Count p " + points.Count + " l " + mesh.vertices.GetLength(0) + " n " + NumSides+" l/n " + mesh.vertices.GetLength(0)/NumSides);
        }
        else {
            UpdateMesh();
        }
    }
    public Vector3[] CalcMeshVerts (){
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
    public override void UpdateAll() {
        UpdateMesh();
        AssignMaterial();
    }
    public override void UpdateMesh(){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCollider col = gameObject.GetComponent<MeshCollider>();
        Vector3[] verts = CalcMeshVerts();
        meshFilter.sharedMesh.vertices = verts;
        if (hasCollision) UpdateCollider(verts);
    }
    public void UpdateMesh(GameObject point, int index){
        UpdateMeshVert(point,index);
    }
    public void BuildMesh(){
        
        Vector3[] v = CalcMeshVerts();
        //Debug.Log("Vs");
        //Debug.Log(DebugTools.ArrayToString(v));
        BuildMesh(v);
    }
    
    public void BuildMesh(
    Vector3[] vertices,
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
        
        //MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Mesh mesh = new Mesh();
        
        //if (!gameObject.GetComponent<MeshRenderer>())
            //meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //else meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (!gameObject.GetComponent<MeshFilter>())
            meshFilter = gameObject.AddComponent<MeshFilter>();
        else meshFilter = gameObject.GetComponent<MeshFilter>();

        //if (!meshRenderer.sharedMaterial)
            //meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;

        meshFilter.sharedMesh = mesh;
    }
    public Vector3[] CalcPointVertices(GameObject point){
        if (point == null) return null;
        LinePointComponent lc = point.GetComponent<LinePointComponent>();
        Vector3[] verts = new Vector3[NumSides];
        // calculate angle between points
        int angle = 360/NumSides;
        // get the center point
        Vector3 origin = point.transform.localPosition;
        // get the first point
        Vector3 v1 = origin;
        v1.y -= lc.size * point.transform.localScale.magnitude;
        for (int i = 0; i < NumSides; i++) {
            verts[i] = VectorMath.RotatePointAroundPivot(v1,origin, new Vector3(-angle*i,0,0));
            verts[i] = VectorMath.RotatePointAroundPivot(verts[i],origin,point.transform.localRotation);
        }
        return verts;
    }
    void AssignMaterial () {
        MeshRenderer meshRenderer;
        if (!gameObject.GetComponent<MeshRenderer>())
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        else meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (lineMaterial) {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = lineMaterial;
        } else {
            CreateLineMaterial();
            meshRenderer.sharedMaterial = lineMaterial;
        }
    }
    void AddCollider() {
        MeshCollider col;
        if (!gameObject.GetComponent<MeshCollider>())
            col = gameObject.AddComponent<MeshCollider>();
        else col = gameObject.GetComponent<MeshCollider>();
        UpdateCollider(CalcMeshVerts());
    }
    void UpdateCollider(Vector3[] verts) {
        MeshCollider col;
        if (!gameObject.GetComponent<MeshCollider>())
            return;
        else col = gameObject.GetComponent<MeshCollider>();
        col.sharedMesh.vertices = verts;
    }
}