using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class MapSaveManager
{
    private readonly HashSet<IParts> savePartsHashSet= new HashSet<IParts>();
    public HashSet<IParts> SavePartsHashSet { get { return savePartsHashSet; } }

    /// <summary>
    /// HashSet에 추가해주는 함수
    /// </summary>
    public void Add(IParts parts)
    {
        if (savePartsHashSet.Contains(parts))
            return;

        savePartsHashSet.Add(parts);
    }

    /// <summary>
    /// HashSet에 삭제해주는 함수
    /// </summary>
    public void Remove(IParts parts)
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

    public void SaveMap(string path, string json)
    {
        if (path == "")
            return;

        if (savePartsHashSet.Count == 0)
        {
            Debug.LogWarning("There is no data to save.");
            return;
        }

        File.WriteAllText(path, json);
    }

    public void LoadMap(string path, Action<string> Load)
    {
        if (path == "")
            return;

        string json = File.ReadAllText(path);
        Load.Invoke(json);
    }
}