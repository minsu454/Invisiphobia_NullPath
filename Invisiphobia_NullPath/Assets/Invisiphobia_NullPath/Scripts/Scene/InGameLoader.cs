using Common.Objects;
using Common.Path;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    public Volume Volume;

    private const string volumePath = "Volume/GeneralVolume";
    private const string navMeshBakerPath = "NavMesh/NavMeshBaker";

    protected override void InitScene()
    {
        CreateGameManager();
        CreateVolume();

        MapManager.Instance.Init();
        EntityManager.Instance.Init();
    }

    /// <summary>
    /// 블륨 생성 함수
    /// </summary>
    private void CreateGameManager()
    {
        GameObject go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
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

    private void Start()
    {
        Managers.Sound.BGMPlay(SceneType.InGame, 0.5f);
    }
}
