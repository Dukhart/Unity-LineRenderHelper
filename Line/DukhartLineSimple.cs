using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLineSimple : MonoBehaviour
{
    [SerializeField]
    public GameObject pointPrefab;
    [SerializeField]
    public Color color = Color.blue;
    [SerializeField]
    public Material lineMaterial;
    [SerializeField]
    public bool drawGizmos = false;
    // if true last point will connect to first point
    [SerializeField]
    public bool loops;
    [SerializeField]
    public bool inFront;

    // list of points that make up the line
    [SerializeField]
    public List<GameObject> points = new List<GameObject>();

    DukhartLineSimple() {
    }
    void Awake () {
        CreateLineMaterial();
    }
    void Start() {
    }

    // adds a point at the input index, index < 0 = end of the list
    public void AddPoint(int index = -1)
    {
        Vector3 position = new Vector3(0,0,0);
        //PointData linePoint = new PointData(new Vector3(0,0,0),0.1f, Color.green);
        if (points == null) {
            points = new List<GameObject>();
        }
        if (points.Count > 0)
        {
            int i = index <= 0 ? points.Count - 1 : index - 1;
            position = new Vector3(points[i].transform.position.x + 1, points[i].transform.position.y, points[i].transform.position.z);
        }
        // default index to the end of the list
        if (index < 0 || index > points.Count) {
            index = points.Count;
        }
        SpawnPoint(index, position);
    }

    // spawns a point and stores in Target.points at the input index, index < 0 = end of the list
    public LinePointComponent SpawnPoint(int index, Vector3 position)
    {
        if (pointPrefab == null) {
            Debug.Log("<color=red>Error: </color>Spawn Point Prefab Missing! " + index);
            return null;
        }
        else if (index > points.Count || index < 0)
        {
            Debug.Log("<color=red>Error: </color>Spawn Point: Invalid Index! " + index);
            return null;
        }
        GameObject linePointO = Instantiate<GameObject>(pointPrefab);
        LinePointComponent linePoint = linePointO.GetComponent<LinePointComponent>();
        linePointO.transform.SetParent(transform);
        linePointO.transform.position = position;

        points.Insert(index,linePointO);
        return linePoint;
    }

    // Remove the last point
    public bool RemovePoint(int index = - 1)
    {
        if (index < 0 || index >= points.Count)
            index = points.Count - 1;
        // remove the point
        return RemovePoint(points[index]);
    }
    // removes the input point
    public bool RemovePoint(GameObject point)
    {
        if (points.Count <= 2)
        {
            Debug.LogWarning("<color=yellow>Warning!</color> A line must have at least 2 points, Start & End.\n");
            return false;
        }
        else
        {
            //remove from list
            points.Remove(point);
            if (Application.isEditor){
                DestroyImmediate(point);
            } else {
                Destroy(point);
            }
            return true;
        }
    }
    void OnDrawGizmos()
    {
        if (drawGizmos){
            if (points.Count <= 1) return;
            Gizmos.color = color;
            Vector3 position1 = points[0].transform.position;
            Vector3 position2;
            for (int i = 1; i < points.Count; ++i) {
                position2 = points[i].transform.position;
                Gizmos.DrawLine(position1, position2);
                position1 = position2;
            }
            if (loops) {
                position2 = points[0].transform.position;
                Gizmos.DrawLine(position1, position2);
            }
            GizmoHelpers.Defaults();
        }
    }
    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader;
            if (inFront)
                shader = Shader.Find("GUI/Text Shader");
            else
                shader = Shader.Find("Hidden/Internal-Colored");

            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // depth writes
            lineMaterial.SetInt("_ZWrite",0);
        }
        lineMaterial.SetColor("_Color", color);
        lineMaterial.SetColor("_EmissionColor", color);
    }
    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        RenderLines();
    }
    void RenderLines() {
        // Apply the line material
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);
        // Draw lines
        Vector3 v1 = points[0].transform.position;
        Vector3 v2;
        GL.Begin(GL.LINES);
            // Vertex colors
            GL.Color(color);
            for (int i = 1; i < points.Count; i++){
                v2 = points[i].transform.position;
                GL.Vertex3(v1.x,v1.y,v1.z);
                GL.Vertex3(v2.x,v2.y,v2.z);
                v1 = v2;
            }
            if (loops) {
                v2 = points[0].transform.position;
                GL.Vertex3(v1.x,v1.y,v1.z);
                GL.Vertex3(v2.x,v2.y,v2.z);
            } 
        GL.End();
        GL.PopMatrix();
    }
}
