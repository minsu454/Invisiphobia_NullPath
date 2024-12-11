using Common.Objects;
using Common.Path;
using UnityEngine;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    public EntityManager EntityManager;

    private const string volumePath = "Volume/GeneralVolume";

    protected override void InitScene()
    {
        TextAsset asset = ObjectManager.Return<TextAsset>(AddressablePath.MapFilePath("Floor01"));
        TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(asset.text);

        CreateVolume();
        CreateEntityManager(totalData);
        CreateMapManager(totalData);
    }

    private void CreateVolume()
    {
        ObjectManager.Instantiate(volumePath);
    }

    private void CreateEntityManager(TotalMapData totalData)
    {
        GameObject go = new GameObject("MapManager");
        EntityManager entityManager = go.AddComponent<EntityManager>();

        entityManager.Init(totalData);
    }

    private void CreateMapManager(TotalMapData totalData)
    {
        GameObject go = new GameObject("MapManager");
        MapManager mapManager = go.AddComponent<MapManager>();

        mapManager.Init(totalData);
    }
}
