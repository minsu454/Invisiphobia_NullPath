using System.Collections.Generic;
using UnityEngine;

public sealed class MapSaveManager
{
    private readonly HashSet<Parts> saveGoHashSet = new HashSet<Parts>();

    public void Init(string path)
    {

    }

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

    public void SaveMap()
    {

    }

    public void LoadMap()
    {

    }
}