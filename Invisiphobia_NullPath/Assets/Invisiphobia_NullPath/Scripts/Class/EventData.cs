using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 방 저장 데이터
/// </summary>
[Serializable]
public class EventData
{
    public string Name;
    public Vector3 Pos;
    public Quaternion Rot;
    public List<PointData> eventList;
    public string useGoPath;

    public EventData(string name, Vector3 pos, Quaternion rot, string useGoPath, List<PointData> eventList)
    {
        Name = name;
        Pos = pos;
        Rot = rot;

        this.useGoPath = useGoPath;
        this.eventList = eventList;
    }
}