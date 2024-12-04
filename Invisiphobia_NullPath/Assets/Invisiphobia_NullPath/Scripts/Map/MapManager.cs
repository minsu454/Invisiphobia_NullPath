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
        Map();
    }

    public void Map()
    {
        TextAsset asset = ObjectManager.Return<TextAsset>(AddressablePath.MapFilePath("Test"));
        TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(asset.text);

        try
        {
            foreach (RoomData data in totalData.RoomDataList)
            {
                GameObject go = ObjectManager.Instantiate(AddressablePath.MapPartsPath(data.Name));
                go.name = data.Name;
                go.transform.position = data.Pos;
                go.transform.Rotate(new Vector3(0, data.RatateY, 0));

                RoomParts parts = go.GetComponent<RoomParts>();

                Material floor = ObjectManager.Return<Material>(AddressablePath.MapMaterialPath(data.FloorMaterialName));
                Material wall = ObjectManager.Return<Material>(AddressablePath.MapMaterialPath(data.WallMaterialName));

                parts.Init(floor, wall);
            }
        }
        catch
        {
            Debug.LogWarning("This file cannot be loaded.");
        }
    }
}
