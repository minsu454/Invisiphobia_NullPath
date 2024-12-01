using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.MapEditor;
using System;
using System.IO;
using UnityEngine.UIElements;

public class MapLayoutEditor : ComstomEditor<MapLayoutEditor>
{
    private MapLayoutBuilder mapBuilder;                        //맵 gizmo 체크 class
    
    private Vector2 mapSize;                                    //맵 사이즈 저장 변수
    Rect areaRect;                                              //rect 저장 변수
    Color areaBackgroundColor = new Color(0.9f, 0.9f, 0.9f);    //에리어 배경 색 변수

    private Vector2 saveScrollPos;                              // 스크롤 위치 저장 변수
    private string pickName = "";                               // 선택된 버튼의 이미지 이름 저장 변수
    private int pickIdx = -1;                                   // 선택된 버튼의 인덱스 저장 변수

    private Editor goEditor;

    private Texture2D[] texture2DArr;                                                           //Parts사진 저장 배열
    private Dictionary<string, RoomParts> partsGoDict = new Dictionary<string, RoomParts>();    //PartsGo 저장 Dictionary

    protected override void OnEnable()
    {
        base.OnEnable();

        GUIParts.LoadAllInFolder(EditorPath.texturePath, out texture2DArr);
        GUIParts.LoadAllInFolder(EditorPath.partsPath, out partsGoDict);

        controller.leftMouseDownEvent += OnleftMouseDown;
        controller.rightMouseUpEvent += OnrightMouseUp;
    }

    [MenuItem("Tools/MapEditor/2DMap")]
    static void Init()
    {
        CreateComstomWindow("Create 2D Map", new Vector2(800f, 580f), new Vector2(800f, 580f));
    }

    private void OnGUI()
    {
        // Normal =====================================================================
        GUILayout.Space(5f);
        GUIParts.CreateHorizontal(MapSizeField, CreateBtn, DeleteBtn);

        if (!editorManager.IsCreateData)
            return;

        // Save =======================================================================



        // ScrollView =================================================================

        areaRect = new Rect(10, 30, 535, 540); // 독립적인 Area

        GUIParts.CreateArea(areaRect, areaBackgroundColor, RoomScrollView);

        // FreView =================================================================

        areaRect = new Rect(550, 30, 245, 240);
        PickGoPreView(areaRect);
    }

    protected override void OnSceneGUI(SceneView sceneView)
    {
        if (mapBuilder == null)
            return;

        mapBuilder.MapSize = mapSize;

        Event e = controller.GetEvent();

        if (e.alt || e.shift || e.control)
        {
            return;
        }

        Vector3 mousePosition = e.mousePosition;

        HandleUtility.FindNearestVertex(mousePosition, mapBuilder.GridTransforms, out Vector3 nearestVertex);

        mapBuilder.HoveredPosition = nearestVertex;

        Bounds bounds = new Bounds(nearestVertex + Vector3.up * mapBuilder.TileScale.y / 2f, mapBuilder.TileScale);

        controller.InputMouse(e);
    }

    /// <summary>
    /// 맵 크기필드 UI 함수
    /// </summary>
    private void MapSizeField()
    {
        GUI.enabled = !editorManager.IsCreateData;
        mapSize = EditorGUILayout.Vector2Field("", mapSize, GUILayout.Width(200));
        GUI.enabled = true;
    }

    /// <summary>
    /// 맵 생성 버튼
    /// </summary>
    private void CreateBtn()
    {
        GUI.enabled = !editorManager.IsCreateData;

        if (GUILayout.Button("Create", GUILayout.Width(100), GUILayout.Height(20)))
        {
            if (mapSize.x <= 0 || mapSize.x % 1 != 0 || mapSize.y <= 0 || mapSize.y % 1 != 0)
            {
                Debug.LogWarning("Map Size Value is OutOfRange. (integer, Positive Number)");
                return;
            }

            editorManager.CreateMap(ref mapBuilder);

            Run();
            CreateGrid();
        }

        GUI.enabled = true;
    }

    /// <summary>
    /// 클릭하기위한 바닥과 격자 띄워주는 함수
    /// </summary>
    public void CreateGrid()
    {
        GameObject background = GameObject.CreatePrimitive(PrimitiveType.Cube);
        background.transform.SetParent(mapBuilder.transform);
        background.transform.position = new Vector3(0, -1, 0);

        GameObject plane = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EditorPath.planePath));
        plane.transform.position = new Vector3(0, -0.49f, 0);
        plane.transform.SetParent(mapBuilder.transform);

        background.transform.localScale = new Vector3(mapSize.x, 1, mapSize.y);
        plane.transform.localScale = new Vector3(mapSize.x / 10, 1, mapSize.y / 10);
    }

    /// <summary>
    /// 맵 삭제 버튼
    /// </summary>
    private void DeleteBtn()
    {
        GUI.enabled = editorManager.IsCreateData;

        if (GUILayout.Button("Delete", GUILayout.Width(100), GUILayout.Height(20)))
        {
            if (mapSize.x <= 0 || mapSize.x % 1 != 0 || mapSize.y <= 0 || mapSize.y % 1 != 0)
            {
                Debug.LogWarning("Map Size Value is OutOfRange. (integer, Positive Number)");
                return;
            }

            editorManager.DeleteMap(ref mapBuilder);
            goEditor = null;
            Stop();
        }

        GUI.enabled = true;
    }

    /// <summary>
    /// 방 선택 스크롤 뷰 생성 함수
    /// </summary>
    private void RoomScrollView(Rect rect)
    {
        // 스크롤 뷰 시작
        saveScrollPos = GUILayout.BeginScrollView(saveScrollPos, GUILayout.Width(rect.width), GUILayout.Height(rect.height));

        // 창 크기에 따라 열 개수 계산
        float cellWidth = 100; // 셀 너비
        int columns = Mathf.Max(1, Mathf.FloorToInt((rect.width - 20) / cellWidth)); // 최소 1열
        int rows = Mathf.CeilToInt(texture2DArr.Length / (float)columns); // 줄 수 계산

        // 버튼 그리드 렌더링
        for (int row = 0; row < rows; row++)
        {
            GUIParts.CreateHorizontal(() =>
            {
                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;
                    if (index >= texture2DArr.Length) break;

                    bool isSelected = (pickIdx == index);

                    GUIStyle buttonStyle = new GUIStyle("Button");
                    buttonStyle.normal.textColor = Color.white;
                    buttonStyle.normal.background = GUIParts.CreateTexture(isSelected ? new Color(0.2f, 0.6f, 1.0f) : new Color(0.7f, 0.7f, 0.7f));

                    GUIContent content = new GUIContent(texture2DArr[index]);
                    if (GUILayout.Button(content, buttonStyle, GUILayout.Width(cellWidth), GUILayout.Height(100)))
                    {
                        string name = texture2DArr[index].name;
                        pickName = name;
                        mapBuilder.TileScale = partsGoDict[name].Size;
                        pickIdx = index;

                        if (!partsGoDict.TryGetValue(pickName, out RoomParts partsPrefab))
                            return;

                        goEditor = Editor.CreateEditor(partsPrefab.gameObject);
                    }
                }
            });
        }

        GUILayout.EndScrollView();
    }

    private void PickGoPreView(Rect rect)
    {
        if (goEditor == null)
            return;

        GUIStyle gStyle = new GUIStyle();
        gStyle.normal.background = Texture2D.grayTexture;
        goEditor.OnInteractivePreviewGUI(rect, gStyle);
    }

    /// <summary>
    /// 왼쪽 마우스 입력 action 함수
    /// </summary>
    private void OnleftMouseDown(Vector3 mousePos)
    {
        if (!partsGoDict.TryGetValue(pickName, out RoomParts partsPrefab))
            return;

        GameObject partsGo = Instantiate(partsPrefab.gameObject);
        partsGo.transform.position = mapBuilder.HoveredPosition + Vector3.up * 0.5f;
        RoomParts roomParts = partsGo.GetComponent<RoomParts>();

        saveManager.Add(roomParts);
    }

    /// <summary>
    /// 오른쪽 마우스 입력해제 action 함수
    /// </summary>
    private void OnrightMouseUp(Vector3 mousePos)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            return;

        if (!hit.collider.TryGetComponent(out Parts parts))
            return;

        saveManager.Remove(parts);
        DestroyImmediate(parts.gameObject);
    }

    protected override void OnDisable()
    {
        if (mapBuilder != null)
            editorManager.DeleteMap(ref mapBuilder);

        base.OnDisable();
    }
}
