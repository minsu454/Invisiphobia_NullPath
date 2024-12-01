using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.MapEditor;

public sealed class MapEditorManager
{
    public bool IsCreateData { get; private set; } = false;

    private const string useScenePath = "Assets/Invisiphobia_NullPath/Scenes/MapEditor.unity";      //전용씬 경로

    /// <summary>
    /// 2D맵 생성 함수
    /// </summary>
    public void CreateMap<T>(ref T builder) where T : Component
    {
        if (builder != null)
        {
            Debug.LogWarning("map has already been created.");
            return;
        }

        SceneEditorManager.OpenTempScene(useScenePath);

        GameObject go = new GameObject("Map");
        builder = go.AddComponent<T>();

        IsCreateData = true;

        Debug.Log("Create Completed");
    }

    /// <summary>
    /// 2D맵 삭제 함수
    /// </summary>
    public void DeleteMap<T>(ref T builder) where T : Component
    {
        if (builder == null)
        {
            Debug.LogWarning("map has already been deleted.");
            return;
        }

        Object.DestroyImmediate(builder);
        IsCreateData = false;

        SceneEditorManager.CloseTempScene();
        Debug.Log("Delete Completed");
    }
}


