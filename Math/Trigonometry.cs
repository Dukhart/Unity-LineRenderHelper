using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Trigonometry
{
    public static Vector3 RotateLineEnd(Vector3 startLocation, Vector3 rotations, float Length)
    {
       //Vector3.
        Vector3 vEnd = new Vector3(Mathf.Sin(rotations.x)*Length, Mathf.Cos(rotations.x) * Length);
        vEnd.x += Mathf.Cos(rotations.y) * Length;
        vEnd.z += Mathf.Sin(rotations.y) * Length;
        vEnd += startLocation;
        return vEnd;
    }
}
