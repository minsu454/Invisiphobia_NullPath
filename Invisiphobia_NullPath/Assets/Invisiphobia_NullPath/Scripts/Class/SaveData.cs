using System.Collections.Generic;

/// <summary>
/// 맵 저장 데이터
/// </summary>
public class SaveData
{
    public PlayerData PlayerData;
    public List<ItemData> ItemDataList = new List<ItemData>();
    public List<EventData> EventDataList = new List<EventData>();

    public SaveData()
    {

    }

    public SaveData(PlayerData playerData, List<ItemData> itemDataList, List<EventData> eventDataList)
    {
        PlayerData = playerData;
        ItemDataList = itemDataList;
        EventDataList = eventDataList;
    }
}