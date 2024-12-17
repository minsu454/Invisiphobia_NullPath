using System;
using UnityEngine;

/// <summary>
/// 방 저장 데이터
/// </summary>
[Serializable]
public class RoomData
{
    public string Name;
    public Vector3 Pos;
    public Quaternion Rot;
    public string FloorMaterialName;
    public string WallMaterialName;

    public RoomData(string name, Vector3 pos, Quaternion rot, string floorMaterialName, string wallMaterialName)
    {
        Name = name;
        Pos = pos;
        Rot = rot;
        FloorMaterialName = floorMaterialName;
        WallMaterialName = wallMaterialName;
    }
}
