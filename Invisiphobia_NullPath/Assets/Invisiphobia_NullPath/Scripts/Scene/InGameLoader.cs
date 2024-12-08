using Common.Objects;
using Common.Path;
using UnityEngine;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    public Player Player;

    private const string volumePath = "Volume/GeneralVolume";

    protected override void InitScene()
    {
        CreateVolume();
        CreatePlayer();
        CreateMapManager();
    }

    private void CreateVolume()
    {
        ObjectManager.Instantiate(volumePath);
    }

    private void CreatePlayer()
    {
        GameObject go = ObjectManager.Instantiate(AddressablePath.EntityPath("Player"));
        Player = go.GetComponent<Player>();
    }

    private void CreateMapManager()
    {
        GameObject go = new GameObject("MapManager");
        MapManager mapManager = go.AddComponent<MapManager>();

        mapManager.Init();
    }


}
