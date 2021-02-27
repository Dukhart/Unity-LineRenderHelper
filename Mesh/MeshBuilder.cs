using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MeshBuilder
{
    static public Vector3[] Default_Verts(){
         Vector3[] v = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };
        //Debug.Log("Default Verts");
        //Debug.Log(DebugTools.ArrayToString(v));
        return v;
    }
    static public int[] Default_Tris(Vector3[] vertices, int numSides, bool loops){
        // calculate tris
        List<int> temp_list = new List<int>();
        int numPoints = vertices.GetLength(0)/numSides;
        int numFaces = (numPoints - 1) * numSides;
        // i  = column
        int i = 0;
        for (i = 0; i < numPoints - 1; ++i) {
            int j = 0;
            // j = row
            for (j = 0; j < numSides - 1; ++j){
                // tri 1
                temp_list.Add(i * numSides + j);
                temp_list.Add((i + 1) * numSides + j);
                temp_list.Add(i * numSides + (j + 1));
                // tri 2
                temp_list.Add((i + 1) * numSides + j);
                temp_list.Add((i + 1) * numSides + (j + 1));
                temp_list.Add(i * numSides + (j + 1));
            }
            // tri 1
            temp_list.Add(i * numSides + j);
            temp_list.Add((i + 1) * numSides + j);
            temp_list.Add(i * numSides);
            // tri 2
            temp_list.Add((i + 1) * numSides + j);
            temp_list.Add((i + 1) * numSides);
            temp_list.Add(i * numSides);
        }
        if (loops) {
            int c = vertices.GetLength(0);
            int j = 0;
            i = numPoints - 1;
            // j  = row
            for (j = 0; j < numSides - 1; ++j){
                // tri 1
                temp_list.Add(i * numSides + j);
                temp_list.Add(j);
                temp_list.Add(i * numSides + (j + 1));
                // tri 2
                temp_list.Add(j);
                temp_list.Add(j + 1);
                temp_list.Add(i * numSides + (j + 1));
            }
            temp_list.Add(i * numSides + j);
            temp_list.Add(j);
            temp_list.Add(i * numSides);
            // tri 2
            temp_list.Add(j);
            temp_list.Add(0);
            temp_list.Add(i * numSides);
        }
        //Debug.Log("Tris");
        //Debug.Log(DebugTools.ListToString(temp_list));
        return temp_list.ToArray();
    }
    static public Vector3[] Default_Normal(Vector3[] vertices){
        List<Vector3> temp_list = new List<Vector3>(vertices.GetLength(0));
        for (int i = 0; i < vertices.GetLength(0); i++) {
            temp_list.Insert(i,-Vector3.forward);
        }
        temp_list.TrimExcess();
        return temp_list.ToArray();
    }
    
    static public Vector2[] Default_UV(Vector3[] vertices) {
        Vector2[] temp_uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
        List<Vector2> temp_list = new List<Vector2>();
        int j = 0;
        for (int i = 0; i < vertices.GetLength(0); i++){
            temp_list.Add(temp_uv[j]);
            //Debug.Log("uvs: " + temp_uv[j]);
            j += 1;
            j = j >= 4 ? 0:j;
        }
        //Debug.Log("UVs");
        //Debug.Log(DebugTools.ListToString(temp_list));
        return temp_list.ToArray();
    }

}
