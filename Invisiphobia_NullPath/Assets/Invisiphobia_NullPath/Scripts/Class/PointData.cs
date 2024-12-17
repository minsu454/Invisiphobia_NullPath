using System;
using UnityEngine;

/// <summary>
/// 위치 저장 데이터
/// </summary>
[Serializable]
public class PointData
{
    public string Name;
    public Vector3 Pos;
    public Quaternion Rot;

    public PointData(string name, Vector3 pos, Quaternion rot)
    {
        Name = name;
        Pos = pos;
        Rot = rot;
    }
}