using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLineComponent : DukhartLine
{
    [SerializeField] bool hasCollision = false;
    [SerializeField, Min(2)] int numSides = 2;
    public int NumSides { 
        get { return numSides; }
        set {
            if (value < 2) {
                value = 2;
            }
            numSides = value;
        }
    }

    public void Awake() {
        // build the mesh
        BuildMesh();
        // add materials
        AssignMaterial();
        // if has collision add collider
        if (hasCollision) AddCollider();
    }
    // Rebuilds Mesh
    public override void Rebuild() {
        // flush old data
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
        // build the mesh
        BuildMesh();
        // assing materilas
        AssignMaterial();
        // if has collision add collider
        if (hasCollision) AddCollider();
    }
    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        // render the line
        RenderLines();
    }
    void RenderLines() {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        //var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
        if (points.Count != mesh.vertices.GetLength(0)/NumSides) {
            // build the mesh if count has changed
            BuildMesh();
            Debug.Log("Diff Count p " + points.Count + " l " + mesh.vertices.GetLength(0) + " n " + NumSides+" l/n " + mesh.vertices.GetLength(0)/NumSides);
        }
        else {
            // update mesh faster than build
            UpdateMesh();
        }
    }
    // calculates the mesh verts
    public Vector3[] CalcMeshVerts (){
        // calculate verts
        List<Vector3> vectors = new List<Vector3>();
        for (int i = 0; i < points.Count; ++i) {
            // calculate point
            Vector3[] verts = CalcPointVertices(points[i]);
            for (int j = 0; j < verts.GetLength(0); j++)
            {
                // add point verts to list
                vectors.Add(verts[j]);
            }
        }
        // return vertices
        return vectors.ToArray();
    }
    // updates a point on 
    public void UpdateMeshVert (GameObject point, int index){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        Vector3[] v = CalcPointVertices(point);
        for (int i = 0; i < v.GetLength(0); i++)
        {
            meshFilter.sharedMesh.vertices[index*2+i] = v[i];
        }
    }
    // updates all mesh params
    public override void UpdateAll() {
        UpdateMesh(); // update mesh
        AssignMaterial(); // update material
    }
    // updates vertices locations
    public override void UpdateMesh(){
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCollider col = gameObject.GetComponent<MeshCollider>();
        Vector3[] verts = CalcMeshVerts();
        meshFilter.sharedMesh.vertices = verts;
        // update the collider if we have collision
        if (hasCollision) UpdateCollider(verts);
    }
    // updates mesh point
    public void UpdateMesh(GameObject point, int index){
        UpdateMeshVert(point,index);
    }
    // builds the mesh calculating vertices
    public void BuildMesh(){
        // calculate vertices
        Vector3[] v = CalcMeshVerts();
        // build the mesh
        BuildMesh(v);
    }
    // builds the mesh with input vertices
    public void BuildMesh(
    Vector3[] vertices,
    int[] tris = null,
    Vector3[] normals = null,
    Vector2[] uv = null) {
        // check for null input create default plane
        if (vertices == null)
            vertices = MeshBuilder.Default_Verts();
        if (tris == null)
            tris = MeshBuilder.Default_Tris(vertices, NumSides, loops);
        if (normals == null)
            normals = MeshBuilder.Default_Normal(vertices);
        if (uv == null)
            uv = MeshBuilder.Default_UV(vertices);
        // create new mesh
        Mesh mesh = new Mesh();
        // get or create mesh filter
        MeshFilter meshFilter;
        if (!gameObject.GetComponent<MeshFilter>())
            meshFilter = gameObject.AddComponent<MeshFilter>();
        else meshFilter = gameObject.GetComponent<MeshFilter>();
        // assign data
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;
        meshFilter.sharedMesh = mesh;
    }
    // calculates the vertices at a point
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
            //verts[i] = VectorMath.RotatePointAroundPivot(verts[i],origin,point.transform.localRotation);
        }
        // return the verts
        return verts;
    }
    // assigns the lines material
    void AssignMaterial () {
        // get / create mesh renderer
        MeshRenderer meshRenderer;
        if (!gameObject.GetComponent<MeshRenderer>())
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        else meshRenderer = gameObject.GetComponent<MeshRenderer>();
        // get / create line material
        if (lineMaterial) {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = lineMaterial;
        } else {
            CreateLineMaterial();
            meshRenderer.sharedMaterial = lineMaterial;
        }
    }
    // adds a mesh collider to the line
    void AddCollider() {
        MeshCollider col;
        if (!gameObject.GetComponent<MeshCollider>())
            col = gameObject.AddComponent<MeshCollider>();
        else col = gameObject.GetComponent<MeshCollider>();
        UpdateCollider(CalcMeshVerts());
    }
    // updates vertex locatiions for mesh collider
    void UpdateCollider(Vector3[] verts) {
        MeshCollider col;
        if (!gameObject.GetComponent<MeshCollider>())
            return;
        else col = gameObject.GetComponent<MeshCollider>();
        col.sharedMesh.vertices = verts;
    }
}