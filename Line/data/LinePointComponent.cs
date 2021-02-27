using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePointComponent : MonoBehaviour
{
    [SerializeField]
    public float size;
    [SerializeField]
    public Color color;
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
