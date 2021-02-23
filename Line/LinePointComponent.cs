using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointData {
    // point position;
    public Vector3 position;
    public float size;
    // gizmo colours
    public Color color;

    public PointData (
        Vector3 position = new Vector3(),
        float size = 0.1f,
        Color color = new Color()) {
        // point position;
        this.position = position;
        this.size = size;
        // gizmo colours
        this.color = color;
    }
}

[System.Serializable]
public class LinePointComponent : MonoBehaviour
{
    public float size;
    // gizmo colours
    public Color color;

    public bool drawGizmos = false;

    void OnDrawGizmosSelected()
    {
        if (drawGizmos) {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(gameObject.transform.position, size);
            GizmoHelpers.Defaults();
        }
    }
}
