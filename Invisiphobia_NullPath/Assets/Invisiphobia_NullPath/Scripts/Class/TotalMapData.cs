using System;
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
    public float RatateY;
    public string FloorMaterialName;
    public string WallMaterialName;

    public RoomData(string name, Vector3 pos, float ratateY, string floorMaterialName, string wallMaterialName)
    {
        Name = name;
        Pos = pos;
        RatateY = ratateY;
        FloorMaterialName = floorMaterialName;
        WallMaterialName = wallMaterialName;
    }
}
