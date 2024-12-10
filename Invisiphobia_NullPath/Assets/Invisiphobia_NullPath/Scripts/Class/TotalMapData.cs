using System.Collections.Generic;

public class TotalMapData
{
    public string MapName;
    public EntityData EntityData = new EntityData();
    public List<RoomData> RoomDataList = new List<RoomData>();
    public List<PointData> DecorDataList = new List<PointData>();
    public List<PointData> ItemDataList = new List<PointData>();
}
