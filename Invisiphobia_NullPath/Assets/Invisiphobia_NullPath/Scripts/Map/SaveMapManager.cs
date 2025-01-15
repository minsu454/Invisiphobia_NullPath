using Common.Objects;
using Common.Path;
using System.Collections.Generic;
using UnityEngine;

public class SaveMapManager : MonoBehaviour
{
    private readonly List<Prop> itemPartsList = new List<Prop>();
    public List<Prop> ItemPartsList { get { return itemPartsList; } }

    private readonly List<EventParts> eventPartsList = new List<EventParts>();
    public List<EventParts> EventPartsList { get { return eventPartsList; } }

    public void Init(SaveData saveData)
    {
        SpawnItem(saveData.ItemDataList);
        SpawnEvent(saveData.EventDataList);
    }

    /// <summary>
    /// 아이템 생성 함수
    /// </summary>
    private void SpawnItem(List<ItemData> dataList)
    {
        foreach (ItemData data in dataList)
        {
            GameObject go = ObjectManager.Instantiate(AddressablePath.ItemPartsPath(data.Name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            Prop prop = go.GetComponent<Prop>();
            itemPartsList.Add(prop);
            prop.Init(data.Id, data.type);
        }
    }

    /// <summary>
    /// 아이템 생성 함수
    /// </summary>
    private void SpawnEvent(List<EventData> dataList)
    {
        foreach (EventData data in dataList)
        {
            GameObject go = ObjectManager.Instantiate(AddressablePath.EventPartsPath(data.Name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            EventParts parts = go.GetComponent<EventParts>();
            eventPartsList.Add(parts);
            parts.Init(data.isCompleted, data.useGoPath, data.eventList);
        }
    }
}
