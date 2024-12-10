using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    public PointData playerData;
    public List<PointData> monsterDataList = new List<PointData>();
}
