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

    /// <summary>
    /// 맵 생성 함수
    /// </summary>
    private void Map(TotalMapData totalData)
    {
        try
        {
            Room(totalData.RoomDataList);
            Deco(totalData.DecorDataList);
            Item(totalData.ItemDataList);
        }
        catch
        {
            Debug.LogWarning("This file cannot be loaded.");
        }
    }

    /// <summary>
    /// 룸 생성 함수
    /// </summary>
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

    /// <summary>
    /// 데코 생성 함수
    /// </summary>
    private void Deco(List<PointData> dataList)
    {
        foreach (PointData data in dataList)
        {
            GameObject go = ObjectManager.Instantiate(AddressablePath.DecoPartsPath(data.Name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;
        }
    }

    /// <summary>
    /// 아이템 생성 함수
    /// </summary>
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
