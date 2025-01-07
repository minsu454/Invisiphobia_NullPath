using Codice.CM.Client.Differences.Graphic;
using Common.Path;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class JsonToPrefab
{
    [MenuItem("Tools/MapEditor/Json To Prefab", priority = 5)]
    private static void ToPrefab()
    {
        try
        {
            string path = EditorUtility.OpenFilePanel("Open File", "", "json");
            string json = File.ReadAllText(path);

            TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(json);

            if (totalData.MapName == null)
                return;

            SceneEditorManager.OpenTempScene(EditorPath.UseScenePath);

            GameObject ingame = new GameObject("InGamePrefab");

            GameObject go = new GameObject("--------------Map--------------");
            go.transform.SetParent(ingame.transform);

            go = new GameObject("MapManager");
            go.transform.SetParent(ingame.transform);
            MapManager mapManager = go.AddComponent<MapManager>();

            foreach (RoomData data in totalData.RoomDataList)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.RoomPartsPath}/{data.Name}.prefab");
                go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, ingame.transform);

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
                go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, ingame.transform);

                go.name = data.Name;
                go.transform.position = data.Pos;
                go.transform.rotation = data.Rot;
            }

            foreach (PointData data in totalData.ItemDataList)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.ItemPartsPath}/{data.Name}.prefab");
                go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, ingame.transform);

                go.name = data.Name;
                go.transform.position = data.Pos;
                go.transform.rotation = data.Rot;

                mapManager.itemPartsList.Add(go.GetComponent<Prop>());
            }

            foreach (EventData data in totalData.EventDataList)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.EventPartsPath}/{data.Name}.prefab");
                go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, ingame.transform);

                go.name = data.Name;
                go.transform.position = data.Pos;
                go.transform.rotation = data.Rot;

                EventParts parts = go.GetComponent<EventParts>();
                parts.Init(data.useGoPath, data.eventList);
            }

            CreateEntityManager(totalData, ingame);

            string initialFilename = "SaveData_" + DateTime.Now.ToString(("MM_dd_HH_mm_ss")) + ".prefab";
            path = EditorUtility.SaveFilePanel("Save File", "", initialFilename, "prefab");

            PrefabUtility.SaveAsPrefabAsset(ingame, path);
        }
        catch
        {
            throw new System.Exception("is not To Prefab.");
        }

        SceneEditorManager.CloseTempScene();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// EntityManager 생성 함수
    /// </summary>
    private static void CreateEntityManager(TotalMapData totalData, GameObject ingame)
    {
        GameObject go = new GameObject("-----------Entity-------------");
        go.transform.SetParent(ingame.transform);

        go = new GameObject("EntityManager");
        go.transform.SetParent(ingame.transform);
        EntityManager entityManager = go.AddComponent<EntityManager>();

        string name = totalData.EntityData.playerData.Name.ToFirstName("_");
        GameObject playerprefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.EntityPath}/{name}.prefab");
        if (playerprefab != null)
        {
            go = (GameObject)PrefabUtility.InstantiatePrefab(playerprefab, ingame.transform);

            go.name = name;
            go.transform.position = totalData.EntityData.playerData.Pos;
            go.transform.rotation = totalData.EntityData.playerData.Rot;

            Player player = go.GetComponent<Player>();
            entityManager.Player = player;
        }

        foreach (PointData data in totalData.EntityData.monsterDataList)
        {
            name = data.Name.ToFirstName("_");
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPath.EntityPath}/{name}.prefab");
            go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, ingame.transform);

            go.name = name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            Monster monster = go.GetComponent<Monster>();
            entityManager.AddMonster(monster);
        }
    }

    public static string ToFirstName(this string value, string separator)
    {
        string[] arr = value.Split(separator);

        if (arr.Length <= 1)
        {
            throw new Exception($"The value is not the target of ToName : {value}");
        }

        return arr[0];
    }
}
