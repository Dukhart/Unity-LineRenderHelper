using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePointComponent : MonoBehaviour
{
    [SerializeField]
    public float size;
    // gizmo colours
    public Color color;
    public bool drawGizmos = true;
    [SerializeField]
    public int index = 0;
    LinePointComponent(){
        size = 0.1f;
    }
    void OnDrawGizmos()
    {
        if (drawGizmos) {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(gameObject.transform.position, size);
            GizmoHelpers.Defaults();
        }
    }
}
