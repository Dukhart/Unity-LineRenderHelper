using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinePointComponent))]
[CanEditMultipleObjects]
public class LinePointComponentEditor : Editor
{
    int index;
    DukhartLineSimple line;
    void OnEnable()
    {
        LinePointComponent point = (LinePointComponent)target;
        index = point.index;
        GameObject go = point.gameObject.transform.parent.gameObject;
        line = go.GetComponent<DukhartLineSimple>();
    }
    public void OnDestroy()
     {
         if (Application.isEditor)
         {
             if((target) == null && line != null) {
                line.RemovePoint(index, false);
             }
         }
     }
}
