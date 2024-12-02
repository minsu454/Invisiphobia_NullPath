using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalMapData
{
    public string MapName;
    public Vector2 MapSize;
    public List<RoomData> RoomDataList = new List<RoomData>();
}

[Serializable]
public class RoomData
{
    public string Name;
    public Vector3 Pos;

    public RoomData(string name, Vector3 pos)
    {
        Name = name;
        Pos = pos;
    }
}