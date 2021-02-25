using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinePointComponent))]
[CanEditMultipleObjects]
public class LinePointComponentEditor : Editor
{
    int index;
    DukhartLine line;
    
    void OnEnable()
    {
        if (target){
            LinePointComponent point = (LinePointComponent)target;
            if (point && point.gameObject && point.gameObject.transform.parent){
                index = point.index;
                GameObject go = point.gameObject.transform.parent.gameObject;
                if (go)
                    line = go.GetComponent<DukhartLine>();
            }
        }
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
