using System;
using UnityEngine;

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