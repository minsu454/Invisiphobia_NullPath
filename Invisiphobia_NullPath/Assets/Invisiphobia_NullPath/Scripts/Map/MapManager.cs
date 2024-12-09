using Common.Objects;
using Common.Path;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public void Init()
    {
        new GameObject("-------------Map--------------");
        Map();
    }

    private void Map()
    {
        TextAsset asset = ObjectManager.Return<TextAsset>(AddressablePath.MapFilePath("Floor01"));
        TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(asset.text);

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
            if (data.Name == "PlayerStarter")
            {
                InGameLoader.Instance.Player.transform.position = data.Pos;
                InGameLoader.Instance.Player.transform.rotation = data.Rot;
                continue;
            }

            GameObject go = ObjectManager.Instantiate(AddressablePath.ItemPartsPath(data.Name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;
        }
    }
}
