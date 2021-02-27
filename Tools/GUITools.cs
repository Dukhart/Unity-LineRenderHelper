using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUITools
{
    public static GUIStyle CenterStyle { 
        get {
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.alignment = TextAnchor.UpperCenter;
            return style;
        }
    }
    public static GUIStyle TitleStyle { 
        get {
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.alignment = TextAnchor.UpperCenter;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 16;
            return style;
        }
    }
    public static GUIStyle HeaderStyle { 
        get {
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 14;
            return style;
        }
    }
}
