using System.Collections.Generic;

/// <summary>
/// 맵에 해당한 모든 데이터
/// </summary>
public class TotalMapData
{
    public string MapName;
    public EntityData EntityData = new EntityData();
    public List<RoomData> RoomDataList = new List<RoomData>();
    public List<PointData> DecorDataList = new List<PointData>();
    public List<ItemData> ItemDataList = new List<ItemData>();
    public List<EventData> EventDataList = new List<EventData>();
}
