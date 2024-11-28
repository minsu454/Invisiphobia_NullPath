using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MapEditor2D : EditorWindow
{
    private static MapEditor2D window;
    private bool isCreateData = false;
    private bool isCreateMap = false;

    private MapBuilder mapBuilder;
    private Vector2 mapSize;

    private GameObject background;
    private GameObject plane;

    private string brforeScenePath; // 현재 씬 저장용
    private Scene brforeScene;     // 임시 씬
    private const string UseScenePath = "Assets/Invisiphobia_NullPath/Scenes/MapEditor.unity";
    private const string planePath = "Assets/Invisiphobia_NullPath/Prefabs/Map/Plane.prefab";


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

        if (!isCreateData)
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

            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
        }
    }

    /// <summary>
    /// 2D맵 생성 함수
    /// </summary>
    private void Create2DMap()
    {
        if (mapBuilder == null)
        {
            if (brforeScene.IsValid())
            {
                Debug.LogWarning("이미 맵 편집 씬이 열려있습니다.");
                return;
            }

            brforeScenePath = EditorSceneManager.GetActiveScene().path;
            brforeScene = EditorSceneManager.OpenScene(UseScenePath);

            GameObject go = new GameObject("Map");
            mapBuilder = go.AddComponent<MapBuilder>();
            Debug.Log("Create Completed");

            isCreateData = true;
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
        if (mapBuilder != null)
        {
            Debug.Log("Delete Completed");
            DestroyImmediate(mapBuilder);

            isCreateData = false;
            isCreateMap = false;

            if (!brforeScene.IsValid())
                return;

            // 임시 씬 닫기
            EditorSceneManager.OpenScene(brforeScenePath);
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
        else
        {
            Debug.LogWarning("map has already been deleted.");
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (mapBuilder == null && !mapBuilder.busy)
        {
            return;
        }

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        HandleUtility.AddDefaultControl(controlId);

        Event e = Event.current;

        if (e.alt || e.shift || e.control)
        {
            return;
        }

        Vector3 mousePosition = e.mousePosition;

        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        Debug.Log(mousePosition);
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
        if (mapBuilder != null)
            Delete2DMap();
    }
}