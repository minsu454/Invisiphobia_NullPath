using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.MapEditor;

public sealed class MapEditorManager
{

    public bool IsCreateData { get; private set; } = false;

    private const string useScenePath = "Assets/Invisiphobia_NullPath/Scenes/MapEditor.unity";

    /// <summary>
    /// 2D맵 생성 함수
    /// </summary>
    public void CreateMap(ref MapBuilder mapBuilder)
    {
        if (mapBuilder != null)
        {
            Debug.LogWarning("map has already been created.");
            return;
        }

        SceneEditorManager.OpenTempScene(useScenePath);

        GameObject go = new GameObject("Map");
        mapBuilder = go.AddComponent<MapBuilder>();

        IsCreateData = true;

        Debug.Log("Create Completed");
    }

    /// <summary>
    /// 2D맵 삭제 함수
    /// </summary>
    public void DeleteMap(ref MapBuilder mapBuilder)
    {
        if (mapBuilder == null)
        {
            Debug.LogWarning("map has already been deleted.");
            return;
        }

        Object.DestroyImmediate(mapBuilder);
        IsCreateData = false;

        SceneEditorManager.CloseTempScene();
        Debug.Log("Delete Completed");
    }
}
