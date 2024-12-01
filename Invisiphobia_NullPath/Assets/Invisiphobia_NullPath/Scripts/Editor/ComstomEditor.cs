using UnityEditor;
using UnityEngine;

public abstract class ComstomEditor<T> : EditorWindow where T : EditorWindow
{
    protected static T window;
    protected MapEditorManager manager;
    protected ComstomMapEditorController controller;

    protected virtual void OnEnable()
    {
        manager = new MapEditorManager();
        controller = new ComstomMapEditorController();
    }

    /// <summary>
    /// 프레임마다 주기적으로 업데이트 해야하는 것들 함수
    /// </summary>
    protected abstract void OnSceneGUI(SceneView sceneView);

    /// <summary>
    /// ComstomWindow 생성 함수
    /// </summary>
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
        manager = null;
        controller = null;
    }
}
