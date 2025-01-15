using System;
using UnityEngine;
/// <summary>
/// 위치 저장 데이터
/// </summary>
[Serializable]
public class ItemData
{
    public int Id;
    public string Name;
    public Vector3 Pos;
    public Quaternion Rot;
    public PropStateType type;

    public ItemData(int id, string name, Vector3 pos, Quaternion rot, PropStateType type)
    {
        Id = id;
        Name = name;
        Pos = pos;
        Rot = rot;
        this.type = type;
    }
}