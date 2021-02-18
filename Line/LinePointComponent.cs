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
    public Color tanColor;
    //
    public float inWidth;
    public float outWidth;

    public Vector2 inAngle;
    public Vector2 outAngle;

    public float inStrength;
    public float outStrength;

    public PointData (
        Vector3 position = new Vector3(),
        float size = 0.1f,
        float inWidth = 1,
        float outWidth = 1,
        Vector2 inAngle = new Vector2(),
        Vector2 outAngle = new Vector2(),
        float inStrength = .1f,
        float outStrength = .1f) {
        // point position;
        this.position = position;
        this.size = size;
        // gizmo colours
        this.color = Color.green;
        this.tanColor = Color.red;
        //
        this.inWidth = inWidth;
        this.outWidth = outWidth;

        this.inAngle = inAngle;
        this.outAngle = outAngle;

        this.inStrength = inStrength;
        this.outStrength = outStrength;
    }
}

[System.Serializable]
public class LinePointComponent : MonoBehaviour
{
    public float size;
    // gizmo colours
    public Color color;
    public Color tanColor;
    //
    public float inWidth;
    public float outWidth;

    public Vector2 inAngle;
    public Vector2 outAngle;

    public float inStrength;
    public float outStrength;

    public bool drawGizmos = false;
    
    void Awake () {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmos) {
            /*
            Gizmos.color = data.color;
            Gizmos.DrawWireSphere(data.position, 1);
            Gizmos.color = data.tanColor;
            Gizmos.DrawLine(data.position, Trigonometry.RotateLineEnd(data.position, data.inAngle, data.inStrength));
            Gizmos.DrawLine(data.position, Trigonometry.RotateLineEnd(data.position, data.outAngle, data.outStrength * -1));
            */
            Gizmos.color = color;
            Gizmos.DrawWireSphere(gameObject.transform.position, size);
            Gizmos.color = tanColor;
            Gizmos.DrawLine(gameObject.transform.position, Trigonometry.RotateLineEnd(gameObject.transform.position, inAngle, inStrength));
            Gizmos.DrawLine(gameObject.transform.position, Trigonometry.RotateLineEnd(gameObject.transform.position, outAngle, outStrength * -1));
            GizmoHelpers.Defaults();
        }
        
    }
}
