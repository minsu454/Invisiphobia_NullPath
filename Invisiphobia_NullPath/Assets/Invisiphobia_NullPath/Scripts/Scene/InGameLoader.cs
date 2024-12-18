using Common.Objects;
using Common.Path;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    public EntityManager Entity;
    public GameManager Game;
    public Volume Volume;

    private const string volumePath = "Volume/GeneralVolume";
    private const string navMeshBakerPath = "NavMesh/NavMeshBaker";

    protected override void InitScene()
    {
        TextAsset asset = ObjectManager.Return<TextAsset>(AddressablePath.MapFilePath("Floor01"));
        TotalMapData totalData = JsonUtility.FromJson<TotalMapData>(asset.text);

        CreateGameManager();
        CreateVolume();
        CreateEntityManager(totalData);
        CreateMapManager(totalData);
        CreateNavMeshBaker();
    }

    /// <summary>
    /// 블륨 생성 함수
    /// </summary>
    private void CreateGameManager()
    {
        GameObject go = new GameObject("GameManager");
        Game = go.AddComponent<GameManager>();
    }

    /// <summary>
    /// 블륨 생성 함수
    /// </summary>
    private void CreateVolume()
    {
        GameObject go = ObjectManager.Instantiate(volumePath);
        Volume = go.GetComponent<Volume>();
    }

    /// <summary>
    /// navmeshBaker 생성 함수
    /// </summary>
    private void CreateNavMeshBaker()
    {
        GameObject go = ObjectManager.Instantiate(navMeshBakerPath);
        NavMeshBaker navMeshBaker = go.GetComponent<NavMeshBaker>();

        navMeshBaker.Init();
    }

    /// <summary>
    /// EntityManager 생성 함수
    /// </summary>
    private void CreateEntityManager(TotalMapData totalData)
    {
        GameObject go = new GameObject("EntityManager");
        EntityManager entityManager = go.AddComponent<EntityManager>();

        entityManager.Init(totalData);
    }

    /// <summary>
    /// MapManager 생성 함수
    /// </summary>
    private void CreateMapManager(TotalMapData totalData)
    {
        GameObject go = new GameObject("MapManager");
        MapManager mapManager = go.AddComponent<MapManager>();

        mapManager.Init(totalData);
    }
}
