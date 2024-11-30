using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Common.Editor;

public class MapEditor : EditorWindow
{
    private static MapEditor window;
    private bool isCreateMap = false;

    private MapBuilder mapBuilder;
    private MapEditorManager mapManager;

    private Vector2 mapSize;

    private GameObject background;
    private GameObject plane;

    private const string planePath = "Assets/Invisiphobia_NullPath/Prefabs/Map/Plane.prefab";
    private const string texturePath = "Assets/Invisiphobia_NullPath/Image/UI/progress-bar.png";

    private readonly HashSet<GameObject> mapGo = new HashSet<GameObject>();
    private readonly HashSet<GameObject> itemGo = new HashSet<GameObject>();

    private Vector2 scrollPos; // 스크롤 위치 저장
    int selGridInt = 0;
    private int selectedIndex = -1; // 선택된 버튼의 인덱스 (-1은 선택되지 않은 상태)
    string[] selStrings = { "radio1", "radio2", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3",
    "radio1", "radio2", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3",
    "radio1", "radio2", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3",
    "radio1", "radio2", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3", "radio3"};

    private void OnEnable()
    {
        mapManager = new MapEditorManager();
    }

    [MenuItem("Tools/MapEditor/2DMap")]
    static void Init()
    {
        if (window != null)
        {
            return;
        }

        window = GetWindow<MapEditor>("Create 2D Map");
        window.Show();

        //// 최소, 최대 크기 지정
        window.minSize = new Vector2(1000f, 700f);
        window.maxSize = new Vector2(1000f, 700f);
    }

    private void OnGUI()
    {
        // Normal =====================================================================
        Rect areaRect = new Rect(10, 10, 500, 125); // 독립적인 Area
        Color backgroundColor = new Color(0.9f, 0.9f, 0.9f); // 연한 회색

        // Area 배경 박스
        GUI.color = backgroundColor; // 배경색 설정
        GUI.Box(areaRect, GUIContent.none); // 배경 박스 그리기
        GUI.color = Color.white; // 기본 색상 복원

        GUILayout.BeginArea(areaRect, GUIStyle.none); // Area 시작

        GUILayout.Space(10f);
        GUILayout.Label("Normal", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Map Create"))
        {
            mapManager.CreateMap(ref mapBuilder);
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        if (GUILayout.Button("Map Delete"))
        {
            mapManager.DeleteMap(ref mapBuilder);
        }
        GUILayout.EndHorizontal();


        if (!mapManager.IsCreateData)
        {
            GUILayout.EndArea(); // Area 끝
            return;
        }
        
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

            if (background == null)
            {
                background = GameObject.CreatePrimitive(PrimitiveType.Cube);
                background.transform.SetParent(mapBuilder.transform);
                background.transform.position = new Vector3(0, -1, 0);

                plane = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(planePath));
                plane.transform.position = new Vector3(0, -0.49f, 0);
                plane.transform.SetParent(mapBuilder.transform);
            }

            background.transform.localScale = new Vector3(mapSize.x, 1, mapSize.y);
            plane.transform.localScale = new Vector3(mapSize.x / 10, 1, mapSize.y / 10);

            isCreateMap = true;
        }

        GUILayout.EndArea(); // Area 끝

        if (!isCreateMap)
            return;

        GUILayout.Space(10f);
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2)); // 구분선
        GUILayout.Space(10f);

        DrawScrollView();
    }

    private void DrawScrollView()
    {
        // Area 정의
        Rect areaRect = new Rect(10, 150, 530, 540); // 독립적인 Area
        Color backgroundColor = new Color(0.9f, 0.9f, 0.9f); // 연한 회색

        // Area 배경 박스
        GUI.color = backgroundColor; // 배경색 설정
        GUI.Box(areaRect, GUIContent.none); // 배경 박스 그리기
        GUI.color = Color.white; // 기본 색상 복원

        GUILayout.BeginArea(areaRect, GUIStyle.none); // Area 시작

        // 스크롤 뷰 시작
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(areaRect.width), GUILayout.Height(areaRect.height));

        // 창 크기에 따라 열 개수 계산
        float cellWidth = 100; // 셀 너비
        int columns = Mathf.Max(1, Mathf.FloorToInt((areaRect.width - 20) / cellWidth)); // 최소 1열
        int rows = Mathf.CeilToInt(selStrings.Length / (float)columns); // 줄 수 계산

        // 버튼 그리드 렌더링
        for (int row = 0; row < rows; row++)
        {
            GUILayout.BeginHorizontal(); // 한 줄 시작
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                if (index >= selStrings.Length) break; // 남은 셀 없으면 종료

                // 현재 버튼이 선택된 상태인지 확인
                bool isSelected = (selectedIndex == index);

                // 버튼 스타일 정의
                GUIStyle buttonStyle = new GUIStyle("Button");
                buttonStyle.normal.textColor = Color.white;

                // 선택된 상태에 따라 배경색 설정
                buttonStyle.normal.background = CreateTexture(isSelected ? new Color(0.2f, 0.6f, 1.0f) : new Color(0.7f, 0.7f, 0.7f));

                // 버튼 생성
                GUIContent content = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath));
                if (GUILayout.Button(content, buttonStyle, GUILayout.Width(cellWidth), GUILayout.Height(100)))
                {
                    selectedIndex = index; // 선택 상태 업데이트
                    Debug.Log("Selected: " + selStrings[index]);
                }
            }
            GUILayout.EndHorizontal(); // 한 줄 끝
        }

        GUILayout.EndScrollView(); // 스크롤 뷰 끝

        GUILayout.EndArea(); // Area 끝
    }

    private Texture2D CreateTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (mapBuilder == null)
            return;

        mapBuilder.MapSize = mapSize;

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        HandleUtility.AddDefaultControl(controlId);

        Event e = Event.current;

        if (e.alt || e.shift || e.control)
        {
            return;
        }

        Vector3 mousePosition = e.mousePosition;

        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        HandleUtility.FindNearestVertex(mousePosition, mapBuilder.GridTransforms, out Vector3 nearestVertex);

        mapBuilder.HoveredPosition = nearestVertex;

        Bounds bounds = new Bounds(nearestVertex + Vector3.up * mapBuilder.tileScale.y / 2f, mapBuilder.tileScale);

        bool t = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);

        if (e.button == 0)
        {

            switch (e.type)
            {
                case EventType.MouseDown:
                    mapBuilder.Dragging = bounds.IntersectRay(ray);

                    break;
                case EventType.MouseDrag:
                    break;

                case EventType.MouseLeaveWindow:

                case EventType.MouseUp:

                    //mapBuilder.InstantiateTiles();

                    //mapBuilder.Dragging = false;

                    Debug.Log("MouseUp");

                    break;
            }

            InternalEditorUtility.RepaintAllViews();

        }
        else if (e.button == 1 && hit.collider != null)
        {
            switch (e.type)
            {
                case EventType.MouseDown:

                    //GUIUtility.hotControl = controlId;

                    //_dragOnsetTileInstance = hit.transform.GetComponent<EditModeInstanceBhv>();
                    Debug.Log("MouseDown");
                    break;

                case EventType.MouseDrag:

                case EventType.MouseUp:

                    //EditModeInstanceBhv tileInstance = hit.transform.GetComponent<EditModeInstanceBhv>();

                    //if (tileInstance == _dragOnsetTileInstance)
                    //{
                    //    mapBuilder.DestroyTile(hit.collider);
                    //}
                    Debug.Log("MouseUp");
                    break;
            }

            InternalEditorUtility.RepaintAllViews();
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;

        if (mapBuilder != null)
            mapManager.DeleteMap(ref mapBuilder);

        mapManager = null;
    }
}