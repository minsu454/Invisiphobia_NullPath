using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartScene
{
    static StartScene()
    {
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorPath.StartScenePath);
    }
}
