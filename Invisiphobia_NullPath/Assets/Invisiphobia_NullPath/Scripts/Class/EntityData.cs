using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity저장 데이터
/// </summary>
[Serializable]
public class EntityData
{
    public PointData playerData;
    public List<PointData> monsterDataList = new List<PointData>();
}
