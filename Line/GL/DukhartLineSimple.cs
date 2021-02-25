using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DukhartLineSimple : DukhartLine
{
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
        Vector3 v1 = points[0].transform.localPosition;
        Vector3 v2;
        GL.Begin(GL.LINES);
            for (int i = 1; i < points.Count; i++){
                v2 = points[i].transform.localPosition;
                GL.Vertex3(v1.x,v1.y,v1.z);
                GL.Vertex3(v2.x,v2.y,v2.z);
                v1 = v2;
            }
            if (loops) {
                v2 = points[0].transform.localPosition;
                GL.Vertex3(v1.x,v1.y,v1.z);
                GL.Vertex3(v2.x,v2.y,v2.z);
            }
        GL.End();
        GL.PopMatrix();
    }
}
