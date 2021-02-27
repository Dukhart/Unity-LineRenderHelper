using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLine : MonoBehaviour
{
    [SerializeField]
    public Color color = Color.blue;
    [SerializeField]
    public Material lineMaterial;
    [SerializeField]
    public bool drawGizmos = false;
    [SerializeField]
    public bool inFront;
    // if true last point will connect to first point
    [SerializeField]
    public bool loops;
    // list of points that make up the line
    [SerializeField]
    public GameObject pointPrefab;
    [SerializeField]
    public List<GameObject> points = new List<GameObject>();

    void Awake () {
        CreateLineMaterial();
    }
    public void CreateLineMaterial()
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
    // adds a point at the input index, index < 0 = end of the list
    public void ShiftPoint(int index, bool dir)
    {
        int i = dir ? index + 1 : index - 1;

        if (i >= points.Count){
            i = 0;
        }
        else if (i < 0) {
            i = points.Count-1;
        }
        
        GameObject tempa = points[index];
        GameObject tempb = points[i];
        tempa.GetComponent<LinePointComponent>().index = i;
        tempb.GetComponent<LinePointComponent>().index = index;
        points[i] = tempa;
        points[index] = tempb;
    }
    // adds a point at the input index, index < 0 = end of the list
    public void AddPoint(int index, bool dir)
    {
        DebugTools.Log("Add " + index + " " + dir);
        index += dir ? 1 : 0;
        AddPoint(index);
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
            // if last point
            if (i == points.Count - 1)
                position = new Vector3(points[i].transform.position.x + 1, points[i].transform.position.y, points[i].transform.position.z);
            else
                position = points[i].transform.position + (points[i + 1].transform.position - points[i].transform.position) / 2;
        }
        // default index to the end of the list
        if (index < 0 || index > points.Count) {
            index = points.Count;
        }
        SpawnPoint(index, position);
        Rebuild();
    }
    void UpdatePointIndices(int index, int amount = 1, bool dir = false){
        if (dir){
            for (int i = index; i >= 0; --i) {
                if (points[i] != null && points[i].GetComponent<LinePointComponent>())
                    points[i].GetComponent<LinePointComponent>().index += amount;
            }
        }
        else {
            for (int i = index; i < points.Count; ++i) {
                if (points[i] != null && points[i].GetComponent<LinePointComponent>())
                    points[i].GetComponent<LinePointComponent>().index += amount;
            }
        }
    }
    // spawns a point and stores in Target.points at the input index, index < 0 = end of the list
    public LinePointComponent SpawnPoint(int index, Vector3 position)
    {
        if (pointPrefab == null) {
            DebugTools.Log("Spawn Point Prefab Missing! " + index, logType.error);
            return null;
        }
        else if (index > points.Count || index < 0)
        {
            DebugTools.Log("Spawn Point: Invalid Index! " + index, logType.error);
            return null;
        }
        GameObject linePointO = Instantiate<GameObject>(pointPrefab);
        LinePointComponent linePoint = linePointO.GetComponent<LinePointComponent>();
        linePointO.transform.SetParent(transform);
        linePointO.transform.position = position;
        linePoint.index = index;
        if (index < points.Count - 1) {
            UpdatePointIndices(index);
        }
        points.Insert(index,linePointO);
        
        return linePoint;
    }
    // Remove the last point
    public bool RemovePoint(int index = - 1, bool deletePoint = true)
    {
        if (index < 0 || index >= points.Count)
            index = points.Count - 1;
        if (points.Count <= 2)
        {
            if (points.Count <= 0) {
                DebugTools.Log("No points detected to remove.");
                return false;
            }
            else {
                DebugTools.Log("A line must have at least 2 points, Start & End.", logType.warning);
            }
        }
        UpdatePointIndices(index, -1);
        if (deletePoint)
        {
            //remove from list
            if (Application.isEditor){
                DestroyImmediate(points[index]);
            } else {
                Destroy(points[index]);
            }
        }
        points.RemoveAt(index);
        Rebuild();
        return true;
    }
    // removes the input point
    public bool RemovePoint(GameObject point, bool deletePoint = true)
    {
        if (point.GetComponent<LinePointComponent>())
            return RemovePoint(point.GetComponent<LinePointComponent>().index, deletePoint);
        else return false;
        /*
        if (points.Count <= 2)
        {
            if (points.Count <= 0) {
                DebugTools.Log("No points detected to remove.");
                return false;
            }
            else {
                DebugTools.Log("A line must have at least 2 points, Start & End.", logType.warning);
            }
        }
        if (point.GetComponent<LinePointComponent>())
            UpdatePointIndices(point.GetComponent<LinePointComponent>().index, -1);
        points.Remove(point);
        
        if (deletePoint)
        {
            //remove from list
            if (Application.isEditor){
                DestroyImmediate(point);
            } else {
                Destroy(point);
            }
        }
        Rebuild();
        return true;
        */
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
    public virtual void UpdateAll() {}
    public virtual void UpdateMesh(){}
    public virtual void Rebuild(){}
}
