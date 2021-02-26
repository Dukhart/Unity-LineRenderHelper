using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMath
{
    //Rotates a vector3 p[oint around a Vector3 pivot by angles
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles) {
        return angles * (point - pivot) + pivot;
    }
}
