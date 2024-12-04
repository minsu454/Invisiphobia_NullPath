using System;
using System.Collections.Generic;
using UnityEngine;

public class TotalMapData
{
    public string MapName;
    public Vector2 MapSize;
    public List<RoomData> RoomDataList = new List<RoomData>();
    public List<DecorData> DecorDataList = new List<DecorData>();
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

[Serializable]
public class DecorData
{
    public string Name;
    public Vector3 Pos;
    public Quaternion Rot;

    public DecorData(string name, Vector3 pos, Quaternion rot)
    {
        Name = name;
        Pos = pos;
        Rot = rot;
    }
}