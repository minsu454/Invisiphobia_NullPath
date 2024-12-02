using UnityEditor;
using UnityEngine;

public abstract class ComstomEditor<T> : EditorWindow where T : EditorWindow
{
    protected static T window;
    protected MapEditorManager editorManager;
    protected MapSaveManager saveManager;

    protected ComstomMapEditorController controller;

    protected virtual void OnEnable()
    {
        editorManager = new MapEditorManager();
        controller = new ComstomMapEditorController();
        saveManager = new MapSaveManager();
    }

    /// <summary>
    /// 프레임마다 주기적으로 업데이트 해야하는 것들 함수
    /// </summary>
    protected abstract void OnSceneGUI(SceneView sceneView);

    /// <summary>
    /// OnSceneGUI 실행시켜주는 함수
    /// </summary>
    protected void Run()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    /// <summary>
    /// OnSceneGUI 멈추는 함수
    /// </summary>
    protected void Stop()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

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
        Stop();
        editorManager = null;
        saveManager = null;
        controller = null;
    }
}
