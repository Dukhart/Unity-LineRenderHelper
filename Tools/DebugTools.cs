using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugTools
{
    // Start is called before the first frame update
    public static string ArrayToString (int[] collection) {
        string s = "";
        foreach (int item in collection)
        {
            s += item.ToString() + " ";
        }
        return s;
    }
    public static string ArrayToString (Vector2[] collection) {
        string s = "";
        foreach (Vector2 item in collection)
        {
            s += item.ToString() + " ";
        }
        return s;
    }
    public static string ArrayToString (Vector3[] collection) {
        string s = "";
        foreach (Vector3 item in collection)
        {
            s += item.ToString() + " ";
        }
        return s;
    }
    public static string ListToString (List<int> collection) {
        return ArrayToString(collection.ToArray());
    }
    public static string ListToString (List<Vector2> collection) {
        return ArrayToString(collection.ToArray());
    }
    public static string ListToString (List<Vector3> collection) {
        return ArrayToString(collection.ToArray());
    }
    
}
