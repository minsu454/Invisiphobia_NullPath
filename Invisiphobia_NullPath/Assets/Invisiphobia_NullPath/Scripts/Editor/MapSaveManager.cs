using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class MapSaveManager
{
    private readonly HashSet<Parts> savePartsHashSet = new HashSet<Parts>();

    /// <summary>
    /// HashSet에 추가해주는 함수
    /// </summary>
    public void Add(Parts parts)
    {
        if (savePartsHashSet.Contains(parts))
            return;

        savePartsHashSet.Add(parts);
    }

    /// <summary>
    /// HashSet에 삭제해주는 함수
    /// </summary>
    public void Remove(Parts parts)
    {
        if (!savePartsHashSet.Contains(parts))
            return;

        savePartsHashSet.Remove(parts);
    }

    /// <summary>
    /// HashSet 클리어해주는 함수
    /// </summary>
    public void Clear()
    {
        savePartsHashSet.Clear();
    }

    public void SaveMap(string path, Vector2 mapSize)
    {
        if (path == "")
            return;

        if (savePartsHashSet.Count == 0)
        {
            Debug.LogWarning("There is no data to save.");
            return;
        }

        TotalMapData totalData = new TotalMapData();

        foreach (Parts parts in savePartsHashSet)
        {
            RoomData roomData = new RoomData(parts.name, parts.transform.position);
            totalData.RoomDataList.Add(roomData);
        }

        totalData.MapName = Path.GetFileNameWithoutExtension(path);
        totalData.MapSize = mapSize;
        string json = JsonUtility.ToJson(totalData);

        File.WriteAllText(path, json);
    }

    public void LoadMap<T>(string path, Action create, ref Vector2 mapSize, Dictionary<string, T> partsGoDict) where T : Parts
    {
        if (path == "")
            return;

        try
        {
            string json = File.ReadAllText(path);
            TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(json);
            mapSize = totalData.MapSize;

            create.Invoke();

            foreach (RoomData data in totalData.RoomDataList)
            {
                GameObject go = Object.Instantiate(partsGoDict[data.Name].gameObject);
                go.name = data.Name;
                go.transform.position = data.Pos;
                savePartsHashSet.Add(go.GetComponent<Parts>());
            }
        }
        catch
        {
            Debug.LogWarning("This file cannot be loaded.");
        }
    }
}