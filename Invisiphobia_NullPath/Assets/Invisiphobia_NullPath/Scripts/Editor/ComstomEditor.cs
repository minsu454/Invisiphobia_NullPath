using UnityEditor;
using UnityEngine;

public abstract class ComstomEditor<T> : EditorWindow where T : EditorWindow
{
    protected static T window;
    protected MapEditorManager mapManager;

    protected virtual void OnEnable()
    {
        mapManager = new MapEditorManager();
    }

    protected abstract void OnSceneGUI(SceneView sceneView);

    protected static void CreateComstomWindow(string name, Vector2 minSize, Vector2 maxSize)
    {
        if (window != null)
        {
            return;
        }

        window = GetWindow<T>(name);

        //// 최소, 최대 크기 지정
        window.minSize = minSize;
        window.maxSize = maxSize;

        window.Show();
    }

    protected virtual void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        mapManager = null;
    }
}
