using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DukhartArcSimple : DukhartLine
{
    public int resolution;
    [SerializeField]
    float length;
    [SerializeField]
    Vector3 angle;

    DukhartArcSimple() {
        length = 100;
        resolution = 100;
        angle = new Vector3(0,0,0);
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        RenderLines();
    }

    void OnDrawGizmos()
    {
        if (drawGizmos){
            Gizmos.color = color;
            Vector3 v1 = new Vector3(0,0,0);
            Vector3 v2 = gameObject.transform.position;
            for (int i = 0; i < resolution; i++){
                v2 = CalcNextPoint(v1);
                Gizmos.DrawLine(transform.TransformPoint(v1),transform.TransformPoint(v2));
                v1 = v2;
            }
            GizmoHelpers.Defaults();
        }
    }
    void RenderLines() {
        
        // Apply the line material
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);
        // Draw lines
        Vector3 v1 = new Vector3(0,0,0);
        GL.Begin(GL.LINES);
            for (int i = 0; i < resolution; i++){
                GL.Vertex3(v1.x,v1.y,v1.z);
                v1 = CalcNextPoint(v1);
                GL.Vertex3(v1.x,v1.y,v1.z);
            }
        GL.End();
        GL.PopMatrix();
    }
    
    Vector3 CalcNextPoint(Vector3 current){
        float dist = length/(float)resolution;
        Vector3 dir = gameObject.transform.forward * dist;
        dir = dir + current;
        dir = Quaternion.Euler(angle.x,angle.y,angle.z)* dir;
        return dir;
    }
}

