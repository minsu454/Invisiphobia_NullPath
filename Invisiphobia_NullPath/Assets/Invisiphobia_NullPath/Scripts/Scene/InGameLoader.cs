using Common.Objects;
using Common.Path;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    private const string navMeshBakerPath = "NavMesh/NavMeshBaker";
    private SaveManager mapManager;

    protected override void InitScene()
    {
        CreateGameManager();
        SaveManager();

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
    private void SaveManager()
    {
        GameObject go = new GameObject("SaveManager");
        mapManager = go.AddComponent<SaveManager>();
        mapManager.Init();
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
        Managers.Sound.FirstSceneBGMPlay(SceneType.InGame, 0.5f);
    }
}
