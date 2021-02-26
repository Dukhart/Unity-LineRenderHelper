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
        return v;
    }
    static public int[] Default_Tris(Vector3[] vertices, int numSides){
        // calculate tris
        List<int> temp_list = new List<int>();
        int[] p = new int[6]
            {
                // lower left triangle
                0, 2, 1,
                // upper right triangle
                2, 3, 1
            };
        int n = vertices.GetLength(0)/numSides - 1;
        for (int i =0; i < n; i++) {
            for (int j = 0; j < 6; j++) {
                int x = p[j]+(2*i);
                temp_list.Add(x);
            }
        }
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
        return temp_list.ToArray();
    }

}
