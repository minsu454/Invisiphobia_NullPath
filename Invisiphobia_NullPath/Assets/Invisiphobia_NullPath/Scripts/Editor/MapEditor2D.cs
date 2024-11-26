using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class MapEditor2D : EditorWindow
{
    private static MapEditor2D window;
    private bool isCreate = false;

    private GameObject map;

    [MenuItem("Tools/MapEditor/2DMap")]
    public static void Init()
    {
        if (window != null)
        {
            return;
        }

        window = GetWindow<MapEditor2D>("Create 2D Map");
        window.Show();

        //// 최소, 최대 크기 지정
        window.minSize = new Vector2(340f, 100f);
        window.maxSize = new Vector2(500f, 1000f);
    }

    private void OnGUI()
    {
        // 굵은 글씨 
        Color originColor = EditorStyles.boldLabel.normal.textColor;
        EditorStyles.boldLabel.normal.textColor = Color.yellow;

        // Header =====================================================================
        GUILayout.Space(10f);
        GUILayout.Label("기본", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("맵 제작"))
        {
            Create2DMap();
        }

        if (GUILayout.Button("맵 삭제"))
        {
            Delete2DMap();
        }
        GUILayout.EndHorizontal();

        if (!isCreate)
            return;

        GUILayout.Label("스테이지 설정 도구", EditorStyles.boldLabel);
        if (GUILayout.Button("새 스테이지 생성"))
        {
            
        }
    }

    /// <summary>
    /// 2D맵 생성 함수
    /// </summary>
    private void Create2DMap()
    {
        if (map == null)
        {
            map = new GameObject("Map");
            Debug.Log("Create Completed");
        }
        else
        {
            Debug.LogWarning("map has already been created.");
        }
    }

    /// <summary>
    /// 2D맵 삭제 함수
    /// </summary>
    private void Delete2DMap()
    {
        if (map != null)
        {
            Debug.Log("Delete Completed");
            DestroyImmediate(map);
        }
        else
        {
            Debug.LogWarning("map has already been deleted.");
        }
    }

    private void OnDisable()
    {
        if (map != null)
            Delete2DMap();
    }
}
