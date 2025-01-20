using Common.Objects;
using Common.Path;
using Common.Save;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    private const string navMeshBakerPath = "NavMesh/NavMeshBaker";
    private SaveMapManager saveMapManager;

    public GameManager Game;

    protected override void InitScene()
    {
        CreateGameManager();

        SaveData saveData = SaveService.Load();

        Player player = EntityManager.Instance.Player;
        player.transform.position = saveData.PlayerData.Pos;
        player.transform.rotation = saveData.PlayerData.Rot;
        player.PlayerInventory.Tablet.Setting(saveData.PlayerData.battery);
        player.Init();

        SaveManager(saveData);

        EntityManager.Instance.Init();
    }

    /// <summary>
    /// 게임 메니저 제작 함수
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
        GameObject go = new GameObject("SaveMapManager");
        saveMapManager = go.AddComponent<SaveMapManager>();
        saveMapManager.Init(saveData);
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

    public void Save()
    {
        SaveData saveData = new SaveData();

        Player player = EntityManager.Instance.Player;
        saveData.PlayerData = new PlayerData(player.transform.position, player.transform.rotation, player.PlayerInventory.InHandItemId, player.PlayerInventory.Tablet.CurrentCharge);

        foreach (Prop item in saveMapManager.ItemPartsList)
        {
            ItemData data = new ItemData(item.Id, item.name, item.transform.position, item.transform.rotation, item.StateType, item.Charge);
            saveData.ItemDataList.Add(data);
        }

        List<EventParts> eventDataList = new List<EventParts>();
        foreach (EventParts parts in saveMapManager.EventPartsList)
        {
            List<PointData> eventList = new List<PointData>();

            foreach (Transform point in parts.OnCompleteTrList)
            {
                PointData pointData = new PointData(
                point.name,
                point.transform.position,
                point.transform.rotation);

                eventList.Add(pointData);
            }

            EventData eventData = new EventData(
                parts.name,
                parts.transform.position,
                parts.transform.rotation,
                parts.GetPath(),
                eventList,
                parts.IsCompleted);
              
            saveData.EventDataList.Add(eventData);
        }

        string json = JsonUtility.ToJson(saveData);

        SaveService.Save(json);
    }
}
