using System;
using System.Collections.Generic;
using UnityEngine;

public class TotalItemData
{
    public string MapName;
    public List<ItemData> RoomDataList = new List<ItemData>();
}

[Serializable]
public class ItemData
{
    public string Name;
    public Vector3 Pos;
    public float RatateY;

    public ItemData(string name, Vector3 pos, float ratateY)
    {
        Name = name;
        Pos = pos;
        RatateY = ratateY;
    }
}