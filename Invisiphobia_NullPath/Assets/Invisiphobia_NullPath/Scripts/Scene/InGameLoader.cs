using UnityEngine;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    protected override void InitScene()
    {
        CreateMapManager();
    }

    private void CreateMapManager()
    {
        GameObject go = new GameObject("MapManager");
        MapManager mapManager = go.AddComponent<MapManager>();

        mapManager.Init();
    }
}
