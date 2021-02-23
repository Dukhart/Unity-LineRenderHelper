using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePointComponent : MonoBehaviour
{
    public float size = 0.1f;
    // gizmo colours
    public Color color;
    public bool drawGizmos = true;
    [SerializeField]
    public int index = 0;

    void OnDrawGizmos()
    {
        if (drawGizmos) {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(gameObject.transform.position, size);
            GizmoHelpers.Defaults();
        }
    }
}
