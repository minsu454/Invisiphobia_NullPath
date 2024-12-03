using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.MapEditor;
using System;
using System.IO;
using UnityEngine.UIElements;
using DG.Tweening.Plugins.Core.PathCore;
using Path = System.IO.Path;
using Unity.VisualScripting;

public class MapLayoutWindow : ComstomWindow<MapLayoutWindow>
{
    private MapLayoutBuilder mapBuilder;    //맵 gizmo 체크 class
  
    private Vector2 mapSize;                //맵 사이즈 저장 변수
    Rect areaRect;                          //rect 저장 변수

    private Vector2 saveScrollPos;          //스크롤 위치 저장 변수
    private string pickName = "";           //선택된 버튼의 이미지 이름 저장 변수
    private int pickIdx = -1;               //선택된 버튼의 인덱스 저장 변수

    private Material floorMaterial;         //바닥 머터리얼
    private Material wallMaterial;          //벽 머터리얼

    private Editor pickGoEditor;            //선택 오브젝트 프리뷰
    private Editor floorEditor;             //선택 바닥 프리뷰
    private Editor wallEditor;              //선택 벽 프리뷰

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

    [MenuItem("Tools/MapEditor/CreateMap")]
    static void Init()
    {
        CreateComstomWindow("Create Map", new Vector2(800f, 580f), new Vector2(800f, 580f));
    }

    private void OnGUI()
    {
        // Normal =====================================================================
        GUILayout.Space(5f);
        GUIParts.CreateHorizontal(MapSizeField, CreateBtn, LoadBtn, DeleteBtn);

        if (!editorManager.IsCreateData)
            return;

        // Save =======================================================================

        SaveBtn();

        // ScrollView =================================================================

        areaRect = new Rect(10, 30, 535, 540); // 독립적인 Area

        GUIParts.CreateArea(areaRect, new Color(0.9f, 0.9f, 0.9f), RoomScrollView);

        // PickFreView =================================================================

        areaRect = new Rect(550, 30, 245, 265);
        GUIParts.CreateArea(areaRect, Color.red, PickGoPreView);

        // MaterialFreView =================================================================

        areaRect = new Rect(550, 300, 245, 132f);
        GUIParts.CreateArea(areaRect, Color.red, FloorMaterialPreView);

        areaRect = new Rect(550, 437f, 245, 132f);
        GUIParts.CreateArea(areaRect, Color.red, WallMaterialPreView);
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

            CreateMap();
        }

        GUI.enabled = true;
    }

    public void CreateMap()
    {
        editorManager.CreateMap(ref mapBuilder);

        Run();
        CreateGrid();
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
            Clear();
        }

        GUI.enabled = true;
    }

    /// <summary>
    /// 맵 세이브 버튼
    /// </summary>
    private void SaveBtn()
    {
        Rect buttonRect = new Rect(695, 5, 100, 20);

        if (GUI.Button(buttonRect, "Save"))
        {
            string initialFilename = "SaveData_" + DateTime.Now.ToString(("MM_dd_HH_mm_ss")) + ".json";

            string path = EditorUtility.SaveFilePanel("Save File", "", initialFilename, "json");
            string json = SaveSerialize(path);

            saveManager.SaveMap(path, json);
        }
    }

    /// <summary>
    /// 맵 로드 버튼
    /// </summary>
    private void LoadBtn()
    {
        GUI.enabled = !editorManager.IsCreateData;

        if (GUILayout.Button("Load", GUILayout.Width(100), GUILayout.Height(20)))
        {
            string path = EditorUtility.OpenFilePanel("Open File", "", "json");
            saveManager.LoadMap(path, LoadUnserialize);
        }

        GUI.enabled = true;
    }

    /// <summary>
    /// 방 선택 스크롤 뷰 생성 함수
    /// </summary>
    private void RoomScrollView(Rect rect)
    {
        saveScrollPos = GUILayout.BeginScrollView(saveScrollPos, GUILayout.Width(rect.width), GUILayout.Height(rect.height));

        float cellWidth = 100;
        int columns = Mathf.Max(1, Mathf.FloorToInt((rect.width - 20) / cellWidth));
        int rows = Mathf.CeilToInt(texture2DArr.Length / (float)columns);

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

                        pickGoEditor = Editor.CreateEditor(partsPrefab.gameObject);
                    }
                }
            });
        }

        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 선택한 오브젝트 프리뷰
    /// </summary>
    /// <param name="rect"></param>
    private void PickGoPreView(Rect rect)
    {
        if (pickGoEditor == null)
            return;

        GUIStyle gStyle = new GUIStyle();
        gStyle.normal.background = Texture2D.grayTexture;
        pickGoEditor.OnInteractivePreviewGUI(new Rect(3, 3, 239, 234), gStyle);

        if (!partsGoDict.TryGetValue(pickName, out RoomParts parts))
            return;

        GUIStyle styleLabel = new GUIStyle("label");
        styleLabel.alignment = TextAnchor.LowerRight;
        styleLabel.normal.textColor = new Color(1, 1, 1, 0.8f);
        styleLabel.fontSize = 10;

        GUIContent nameLabel = new GUIContent(parts.gameObject.name, parts.gameObject.name);
        EditorGUI.LabelField(new Rect(3, 217, 239, 20), nameLabel, styleLabel);

        styleLabel.alignment = TextAnchor.MiddleLeft;
        styleLabel.normal.textColor = new Color(1, 1, 1, 0.8f);
        styleLabel.fontSize = 15;

        GUIContent label = new GUIContent(parts.Description, parts.Description);
        EditorGUI.LabelField(new Rect(3, 237, 239, 25), label, styleLabel);
    }

    /// <summary>
    /// 바닥 머터리얼 프리뷰
    /// </summary>
    private void FloorMaterialPreView(Rect rect)
    {
        GUILayout.Label("Material Preview", EditorStyles.boldLabel);

        // 드래그 앤 드롭으로 Material 선택
        Material newMaterial = (Material)EditorGUILayout.ObjectField(floorMaterial, typeof(Material), true);

        if (newMaterial == floorMaterial)
            return;

        floorMaterial = newMaterial;

        //pickGoEditor.OnInteractivePreviewGUI(new Rect(3, 3, 239, 234), gStyle);

        //GUIStyle styleLabel = new GUIStyle("label");
        //styleLabel.alignment = TextAnchor.LowerRight;
        //styleLabel.normal.textColor = new Color(1, 1, 1, 0.8f);
        //styleLabel.fontSize = 10;

        //GUIContent nameLabel = new GUIContent(parts.gameObject.name, parts.gameObject.name);
        //EditorGUI.LabelField(new Rect(3, 217, 239, 20), nameLabel, styleLabel);

        //styleLabel.alignment = TextAnchor.MiddleLeft;
        //styleLabel.normal.textColor = new Color(1, 1, 1, 0.8f);
        //styleLabel.fontSize = 15;

        //GUIContent label = new GUIContent(parts.Description, parts.Description);
        //EditorGUI.LabelField(new Rect(3, 237, 239, 25), label, styleLabel);
    }

    /// <summary>
    /// 벽 머터리얼 프리뷰
    /// </summary>
    private void WallMaterialPreView(Rect rect)
    {
        GUILayout.Label("Material Preview", EditorStyles.boldLabel);

        // 드래그 앤 드롭으로 Material 선택
        Material newMaterial = (Material)EditorGUILayout.ObjectField(wallMaterial, typeof(Material), true);

        if (newMaterial == wallMaterial)
            return;

        wallMaterial = newMaterial;
    }
    

    /// <summary>
    /// 왼쪽 마우스 입력 action 함수
    /// </summary>
    private void OnleftMouseDown(Vector3 mousePos)
    {
        if (!partsGoDict.TryGetValue(pickName, out RoomParts partsPrefab))
            return;

        GameObject partsGo = Instantiate(partsPrefab.gameObject);

        partsGo.name = partsPrefab.gameObject.name;
        partsGo.transform.position = mapBuilder.HoveredPosition + Vector3.up * 0.5f;
        RoomParts roomParts = partsGo.GetComponent<RoomParts>();

        roomParts.Init(floorMaterial, wallMaterial);

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

    /// <summary>
    /// 데이터 json으로 변환 함수
    /// </summary>
    private string SaveSerialize(string path)
    {
        TotalMapData totalData = new TotalMapData();

        foreach (Parts parts in saveManager.SavePartsHashSet)
        {
            RoomData roomData = new RoomData(parts.name, parts.transform.position, floorMaterial.name, wallMaterial.name);
            totalData.RoomDataList.Add(roomData);
        }

        totalData.MapName = Path.GetFileNameWithoutExtension(path);
        totalData.MapSize = mapSize;
        string json = JsonUtility.ToJson(totalData);

        return json;
    }

    /// <summary>
    /// json 데이터 맵으로 변환 함수
    /// </summary>
    private void LoadUnserialize(string json)
    {
        TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(json);

        try
        {
            mapSize = totalData.MapSize;

            CreateMap();

            foreach (RoomData data in totalData.RoomDataList)
            {
                GameObject go = Instantiate(partsGoDict[data.Name].gameObject);
                go.name = data.Name;
                go.transform.position = data.Pos;

                RoomParts parts = go.GetComponent<RoomParts>();

                Material floor = AssetDatabase.LoadAssetAtPath<Material>($"{EditorPath.materialPath}/{data.FloorMaterialName}.mat");
                Material wall = AssetDatabase.LoadAssetAtPath<Material>($"{EditorPath.materialPath}/{data.WallMaterialName}.mat");

                parts.Init(floor, wall);

                saveManager.Add(parts);
            }
        }
        catch
        {
            Debug.LogWarning("This file cannot be loaded.");
        }
    }

    /// <summary>
    /// 클리어 함수
    /// </summary>
    private void Clear()
    {
        editorManager.DeleteMap(ref mapBuilder);
        saveManager.Clear();
        pickGoEditor = null;
        Stop();
    }

    protected override void OnDisable()
    {
        if (mapBuilder != null)
            editorManager.DeleteMap(ref mapBuilder);

        base.OnDisable();
    }
}
