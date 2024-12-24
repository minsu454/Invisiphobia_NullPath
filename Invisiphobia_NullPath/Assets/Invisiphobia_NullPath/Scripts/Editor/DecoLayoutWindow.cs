using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DecoLayoutWindow : CustomWindow<DecoLayoutWindow>
{
    private Rect areaRect;                  //rect 저장 변수

    private Vector2 saveScrollPos;          //스크롤 위치 저장 변수
    private string pickName = "";           //선택된 버튼의 이미지 이름 저장 변수
    private int pickIdx = -1;               //선택된 버튼의 인덱스 저장 변수

    private Editor pickGoEditor;            //선택 오브젝트 프리뷰

    private bool isLoaded = false;

    private Texture2D[] texture2DArr;                                                           //Parts사진 저장 배열
    private Dictionary<string, DecoParts> partsGoDict = new Dictionary<string, DecoParts>();  //PartsGo 저장 Dictionary

    private TotalMapData totalData;

    protected override void OnEnable()
    {
        base.OnEnable();

        GUIParts.LoadAllInFolder(EditorPath.DecoTexturePath, out texture2DArr);
        GUIParts.LoadAllInFolder(EditorPath.DecoPartsPath, out partsGoDict);

        string path = EditorUtility.OpenFilePanel("Open File", "", "json");
        saveManager.LoadMap(path, LoadMap);
    }

    [MenuItem("Tools/MapEditor/Deco Layout", priority = 1)]
    static void Init()
    {
        CreateComstomWindow("Deco Layout", new Vector2(800f, 580f), new Vector2(800f, 580f));
    }

    private void OnGUI()
    {
        if (!isLoaded)
            return;

        // Normal =====================================================================
        GUILayout.Space(5f);
        GUIParts.CreateHorizontal(DeleteBtn);

        if (!editorManager.IsCreateData)
            return;

        // Save =======================================================================

        SaveBtn();

        // ScrollView =================================================================

        areaRect = new Rect(10, 30, 535, 540); // 독립적인 Area

        GUIParts.CreateArea(areaRect, new Color(0.9f, 0.9f, 0.9f), ItemScrollView);

        // PickFreView =================================================================

        areaRect = new Rect(550, 30, 245, 265);
        GUIParts.CreateArea(areaRect, Color.red, PickGoPreView, SpawnDecorBtn, DestroyDecorBtn);
    }

    protected override void OnSceneGUI(SceneView sceneView)
    {

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
    /// 데코 스폰 버튼
    /// </summary>
    private void SpawnDecorBtn(Rect rect)
    {
        if (GUI.Button(new Rect(42, 238, 100, 25), "Spawn"))
        {
            if (!partsGoDict.TryGetValue(pickName, out DecoParts partsPrefab))
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

            DecoParts decorParts = partsGo.GetComponent<DecoParts>();

            saveManager.Add(decorParts);
        }
    }

    /// <summary>
    /// 데코 삭제 버튼
    /// </summary>
    private void DestroyDecorBtn(Rect rect)
    {
        if (GUI.Button(new Rect(142, 238, 100, 25), "Destroy"))
        {
            UnityEngine.Object[] selectedObjects = Selection.objects;

            foreach (UnityEngine.Object obj in selectedObjects)
            {
                GameObject selectedObject = obj as GameObject;
                if (selectedObject == null)
                    continue;

                if (!selectedObject.TryGetComponent(out DecoParts parts))
                    continue;

                saveManager.Remove(parts);
                DestroyImmediate(selectedObject);
            }
        }
    }

    /// <summary>
    /// 방 선택 스크롤 뷰 생성 함수
    /// </summary>
    private void ItemScrollView(Rect rect)
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

                        if (!partsGoDict.TryGetValue(pickName, out DecoParts partsPrefab))
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

        if (!partsGoDict.TryGetValue(pickName, out DecoParts item))
            return;

        GUIStyle styleLabel = new GUIStyle("label");
        styleLabel.alignment = TextAnchor.LowerRight;
        styleLabel.normal.textColor = new Color(1, 1, 1, 0.8f);
        styleLabel.fontSize = 10;

        GUIContent nameLabel = new GUIContent(item.gameObject.name, item.gameObject.name);
        EditorGUI.LabelField(new Rect(3, 217, 239, 20), nameLabel, styleLabel);
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
        saveManager.Clear();
        pickGoEditor = null;
        totalData = null;
        Stop();
        editorManager.LeaveMapEditor();
    }

    /// <summary>
    /// 데이터 json으로 변환 함수
    /// </summary>
    private string SaveSerialize(string path)
    {
        TotalMapData data = totalData;

        data.DecorDataList.Clear();
        data.DecorDataList = new List<PointData>();

        foreach (IParts parts in saveManager.SavePartsHashSet)
        {
            DecoParts decorParts = parts as DecoParts;
            PointData decorData = new PointData(
                decorParts.name,
                decorParts.transform.position,
                decorParts.transform.rotation);

            data.DecorDataList.Add(decorData);
        }

        string json = JsonUtility.ToJson(data);

        return json;
    }

    private void LoadMap(string json)
    {
        totalData = JsonUtility.FromJson<TotalMapData>(json);

        if (totalData.MapName == null)
            return;

        CreateMap();

        foreach (RoomData data in totalData.RoomDataList)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.RoomPartsPath}/{data.Name}.prefab");
            GameObject go = Instantiate(prefab);

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            RoomParts parts = go.GetComponent<RoomParts>();
            Material floor = AssetDatabase.LoadAssetAtPath<Material>($"{EditorPath.materialPath}/{data.FloorMaterialName}.mat");
            Material wall = AssetDatabase.LoadAssetAtPath<Material>($"{EditorPath.materialPath}/{data.WallMaterialName}.mat");

            parts.Init(floor, wall);
        }

        foreach (PointData data in totalData.DecorDataList)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.DecoPartsPath}/{data.Name}.prefab");
            GameObject go = Instantiate(prefab);

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            IParts parts = go.GetComponent<IParts>();

            saveManager.Add(parts);
        }

        foreach (PointData data in totalData.ItemDataList)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.ItemPartsPath}/{data.Name}.prefab");
            GameObject go = Instantiate(prefab);

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;
        }


        GameObject playerprefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.EntityPartsPath}/{totalData.EntityData.playerData.Name}.prefab");
        if (playerprefab != null)
        {
            GameObject go = Instantiate(playerprefab);

            go.name = totalData.EntityData.playerData.Name;
            go.transform.position = totalData.EntityData.playerData.Pos;
            go.transform.rotation = totalData.EntityData.playerData.Rot;
        }

        foreach (PointData data in totalData.EntityData.monsterDataList)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.EntityPartsPath}/{data.Name}.prefab");
            GameObject go = Instantiate(prefab);

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;
        }

        foreach (EventData data in totalData.EventDataList)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.EventPartsPath}/{data.Name}.prefab");
            GameObject go = Instantiate(prefab);

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            EventParts parts = go.GetComponent<EventParts>();
            parts.Init(data.useGoPath, data.eventList);
        }

        isLoaded = true;
    }

    protected override void OnDisable()
    {
        if (editorManager.IsCreateData)
            Clear();

        base.OnDisable();
    }
}