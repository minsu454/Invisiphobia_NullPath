using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : EditorWindow
{
    [MenuItem("Tools/MapEditor/Window")]
    public static void Open()
    {
        GetWindow<MapEditor>();
    }

    private void OnGUI()
    {
        GUILayout.Label("스테이지 설정 도구", EditorStyles.boldLabel);
        if (GUILayout.Button("새 스테이지 생성"))
        {
            
        }
    }
}
