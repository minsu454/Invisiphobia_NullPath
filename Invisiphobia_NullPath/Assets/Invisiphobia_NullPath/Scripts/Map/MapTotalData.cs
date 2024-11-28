using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTotalData
{
    public string mapName;
    public MapData[] MapDataList;
}

[Serializable]
public class MapData
{
    public string Path;
    public Vector3 Pos;
}