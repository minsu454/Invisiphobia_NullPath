using Common.Objects;
using Common.Path;
using Common.StringEx;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public void Init(TotalMapData totalData)
    {
        new GameObject("-------------Map--------------");
        Map(totalData);
    }

    private void Map(TotalMapData totalData)
    {
        try
        {
            Room(totalData.RoomDataList);
            Decor(totalData.DecorDataList);
            Item(totalData.ItemDataList);
        }
        catch
        {
            Debug.LogWarning("This file cannot be loaded.");
        }
    }

    private void Room(List<RoomData> dataList)
    {
        foreach (RoomData data in dataList)
        {
            GameObject go = ObjectManager.Instantiate(AddressablePath.RoomPartsPath(data.Name));
            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            RoomParts parts = go.GetComponent<RoomParts>();

            Material floor = ObjectManager.Return<Material>(AddressablePath.MapMaterialPath(data.FloorMaterialName));
            Material wall = ObjectManager.Return<Material>(AddressablePath.MapMaterialPath(data.WallMaterialName));

            parts.Init(floor, wall);
        }
    }

    private void Decor(List<PointData> dataList)
    {
        foreach (PointData data in dataList)
        {
            GameObject go = ObjectManager.Instantiate(AddressablePath.DecoPartsPath(data.Name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;
        }
    }

    private void Item(List<PointData> dataList)
    {
        foreach (PointData data in dataList)
        {
            GameObject go = ObjectManager.Instantiate(AddressablePath.ItemPartsPath(data.Name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;
        }
    }
}
