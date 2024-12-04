using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using Path = System.IO.Path;

public class MapLayoutWindow : CustomWindow<MapLayoutWindow>
{
    private Rect areaRect;                  //rect 저장 변수

    private Vector2 saveScrollPos;          //스크롤 위치 저장 변수
    private string pickName = "";           //선택된 버튼의 이미지 이름 저장 변수
    private int pickIdx = -1;               //선택된 버튼의 인덱스 저장 변수

    private Material floorMaterial;         //바닥 머터리얼
    private Material wallMaterial;          //벽 머터리얼

    private Editor pickGoEditor;            //선택 오브젝트 프리뷰

    private int curRotateCount = 0;         //회전 값 변수
    private int rotate = 90;                //회전 각도
    private const int rotateMax = 4;        //회전 각 횟수(90도)

    private Texture2D[] texture2DArr;                                                           //Parts사진 저장 배열
    private Dictionary<string, RoomParts> partsGoDict = new Dictionary<string, RoomParts>();    //PartsGo 저장 Dictionary

    protected override void OnEnable()
    {
        base.OnEnable();

        GUIParts.LoadAllInFolder(EditorPath.roomTexturePath, out texture2DArr);
        GUIParts.LoadAllInFolder(EditorPath.roomPartsPath, out partsGoDict);

        floorMaterial = AssetDatabase.LoadAssetAtPath<Material>($"{EditorPath.materialPath}/Lit.mat");
        wallMaterial = AssetDatabase.LoadAssetAtPath<Material>($"{EditorPath.materialPath}/Lit.mat");
    }

    [MenuItem("Tools/MapEditor/Room Layout", priority = 0)]
    static void Init()
    {
        CreateComstomWindow("Room Layout", new Vector2(800f, 580f), new Vector2(800f, 580f));
    }

    private void OnGUI()
    {
        // Normal =====================================================================
        GUILayout.Space(5f);
        GUIParts.CreateHorizontal(CreateBtn, LoadBtn, DeleteBtn);

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

        // Tools =======================================================================

        areaRect = new Rect(550, 300, 245, 30f);
        GUIParts.CreateArea(areaRect, Color.red, MapTools);

        // MaterialFreView =============================================================

        areaRect = new Rect(550, 496f, 245, 74f);
        GUIParts.CreateArea(areaRect, Color.red, FloorMaterialPreView, WallMaterialPreView);
    }

    protected override void OnSceneGUI(SceneView sceneView)
    {
        
    }

    /// <summary>
    /// 맵 생성 버튼
    /// </summary>
    private void CreateBtn()
    {
        GUI.enabled = !editorManager.IsCreateData;

        if (GUILayout.Button("Create", GUILayout.Width(100), GUILayout.Height(20)))
        {
            CreateMap();
        }

        GUI.enabled = true;
    }

    /// <summary>
    /// 맵 삭제 버튼
    /// </summary>
    private void DeleteBtn()
    {
        GUI.enabled = editorManager.IsCreateData;

        if (GUILayout.Button("Delete", GUILayout.Width(100), GUILayout.Height(20)))
        {
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

    private void MapTools(Rect rect)
    {
        GUILayout.Space(5f);
        GUIParts.CreateHorizontal(RoomRotateBtn, SpawnRoomBtn, DestroyRoomBtn);
    }

    /// <summary>
    /// 방 돌리는 버튼
    /// </summary>
    private void RoomRotateBtn()
    {
        if (GUILayout.Button("↻", GUILayout.Width(25), GUILayout.Height(20)))
        {

        }
    }

    /// <summary>
    /// 방 스폰 버튼
    /// </summary>
    private void SpawnRoomBtn()
    {
        if (GUI.Button(new Rect(42, 2, 100, 25), "Spawn"))
        {
            if (!partsGoDict.TryGetValue(pickName, out RoomParts partsPrefab))
                return;

            GameObject partsGo = Instantiate(partsPrefab.gameObject);

            partsGo.name = partsPrefab.gameObject.name;
            var sceneCamera = SceneView.lastActiveSceneView.camera;

            if (sceneCamera != null)
            {
                Vector3 spawnPosition = sceneCamera.transform.position + sceneCamera.transform.forward * 5f;
                partsGo.transform.position = spawnPosition;
            }
            else
            {
                Debug.LogWarning("Scene view camera not found.");
                partsGo.transform.position = Vector3.zero;
            }

            RoomParts roomParts = partsGo.GetComponent<RoomParts>();

            roomParts.Init(floorMaterial, wallMaterial);

            saveManager.Add(roomParts);
        }
    }

    /// <summary>
    /// 방 삭제 버튼
    /// </summary>
    private void DestroyRoomBtn()
    {
        if (GUI.Button(new Rect(142, 2, 100, 25), "Destroy"))
        {
            if (Selection.activeGameObject == null)
                return;

            GameObject selectedObject = Selection.activeGameObject;

            if (!selectedObject.TryGetComponent(out RoomParts parts))
                return;

            saveManager.Remove(parts);
            DestroyImmediate(selectedObject);
        }
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
                        pickIdx = index;
                        curRotateCount = 0;

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
        GUILayout.Label("Floor Material", EditorStyles.boldLabel);

        // 드래그 앤 드롭으로 Material 선택
        Material newMaterial = (Material)EditorGUILayout.ObjectField(floorMaterial, typeof(Material), true);

        if (newMaterial == floorMaterial)
            return;

        floorMaterial = newMaterial;
    }

    /// <summary>
    /// 벽 머터리얼 프리뷰
    /// </summary>
    private void WallMaterialPreView(Rect rect)
    {
        GUILayout.Label("Wall Material", EditorStyles.boldLabel);

        // 드래그 앤 드롭으로 Material 선택
        Material newMaterial = (Material)EditorGUILayout.ObjectField(wallMaterial, typeof(Material), true);

        if (newMaterial == wallMaterial)
            return;

        wallMaterial = newMaterial;
    }

    /// <summary>
    /// 맵 제작 함수
    /// </summary>
    public void CreateMap()
    {
        editorManager.LoadMapEditor();

        Run();
    }

    /// <summary>
    /// 클리어 함수
    /// </summary>
    private void Clear()
    {
        editorManager.LeaveMapEditor();
        saveManager.Clear();
        pickGoEditor = null;
        Stop();
    }

    /// <summary>
    /// 데이터 json으로 변환 함수
    /// </summary>
    private string SaveSerialize(string path)
    {
        TotalMapData totalData = new TotalMapData();

        foreach (IParts parts in saveManager.SavePartsHashSet)
        {
            RoomParts roomParts = parts as RoomParts;
            RoomData roomData = new RoomData(
                roomParts.name,
                roomParts.transform.position,
                roomParts.transform.rotation,
                roomParts.CustomFloorMaterial.name,
                roomParts.CustomWallMaterial.name);

            totalData.RoomDataList.Add(roomData);
        }

        totalData.MapName = Path.GetFileNameWithoutExtension(path);
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
            CreateMap();

            foreach (RoomData data in totalData.RoomDataList)
            {
                GameObject go = Instantiate(partsGoDict[data.Name].gameObject);
                go.name = data.Name;
                go.transform.position = data.Pos;
                go.transform.rotation = data.Rot;

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

    protected override void OnDisable()
    {
        if (editorManager.IsCreateData)
            editorManager.LeaveMapEditor();

        base.OnDisable();
    }
}
