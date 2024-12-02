using System.Collections.Generic;
using UnityEngine;

public sealed class MapSaveManager
{
    private readonly HashSet<Parts> saveGoHashSet = new HashSet<Parts>();

    /// <summary>
    /// HashSet에 추가해주는 함수
    /// </summary>
    public void Add(Parts parts)
    {
        if (saveGoHashSet.Contains(parts))
            return;

        saveGoHashSet.Add(parts);
    }

    /// <summary>
    /// HashSet에 삭제해주는 함수
    /// </summary>
    public void Remove(Parts parts)
    {
        if (!saveGoHashSet.Contains(parts))
            return;

        saveGoHashSet.Remove(parts);
    }

    public void SaveMap(string path)
    {
        if (saveGoHashSet.Count == 0)
        {
            Debug.LogWarning("There is no data to save.");
            return;
        }

        MapTotalData data = new MapTotalData();
    }

    public void LoadMap(string path)
    {

    }
}