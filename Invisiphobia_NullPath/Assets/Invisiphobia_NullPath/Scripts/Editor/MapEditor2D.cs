using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapEditor2D : EditorWindow
{
    private static MapEditor2D window;
    private bool isCreate = false;

    private GameObject map;
    private Vector2 mapSize;

    private string brforeScenePath; // 현재 씬 저장용
    private Scene brforeScene;     // 임시 씬
    private const string UseScenePath = "Assets/Invisiphobia_NullPath/Scenes/MapEditor.unity";

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
        // Normal =====================================================================
        GUILayout.Space(10f);
        GUILayout.Label("Normal", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Map Create"))
        {
            Create2DMap();
        }

        if (GUILayout.Button("Map Delete"))
        {
            Delete2DMap();
        }
        GUILayout.EndHorizontal();

        if (!isCreate)
            return;

        // Normal =====================================================================
        GUILayout.Space(10f);

        mapSize = EditorGUILayout.Vector2Field("Map Size", mapSize);
        if (GUILayout.Button("Bake"))
        {
            if (mapSize.x <= 0 || mapSize.x % 1 != 0 || mapSize.y <= 0 || mapSize.y % 1 != 0)
            {
                Debug.LogWarning("Map Size Value is OutOfRange. (integer, Positive Number)");
                return;
            }

            Debug.Log("hi");
        }
    }

    /// <summary>
    /// 2D맵 생성 함수
    /// </summary>
    private void Create2DMap()
    {
        if (map == null)
        {
            if (brforeScene.IsValid())
            {
                Debug.LogWarning("이미 맵 편집 씬이 열려있습니다.");
                return;
            }

            brforeScenePath = EditorSceneManager.GetActiveScene().path;
            brforeScene = EditorSceneManager.OpenScene(UseScenePath);

            map = new GameObject("Map");
            Debug.Log("Create Completed");

            isCreate = true;
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

            isCreate = false;

            if (!brforeScene.IsValid())
                return;

            // 임시 씬 닫기
            EditorSceneManager.OpenScene(brforeScenePath);

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
