using Common.Objects;
using Common.Path;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    private const string navMeshBakerPath = "NavMesh/NavMeshBaker";
    private const string saveDataPath = "JSON/SaveData/Floor01_Original";
    private SaveManager saveManager;

    public GameManager Game;

    protected override void InitScene()
    {
        CreateGameManager();

        TextAsset json = Resources.Load<TextAsset>(saveDataPath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json.text);

        Player player = EntityManager.Instance.Player;
        player.transform.position = saveData.playerData.Pos;
        player.transform.rotation = saveData.playerData.Rot;
        player.Init();

        SaveManager(saveData);

        EntityManager.Instance.Init();
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
    private void SaveManager(SaveData saveData)
    {
        new GameObject("-------------Save--------------");
        GameObject go = new GameObject("SaveManager");
        saveManager = go.AddComponent<SaveManager>();
        saveManager.Init(saveData);
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
