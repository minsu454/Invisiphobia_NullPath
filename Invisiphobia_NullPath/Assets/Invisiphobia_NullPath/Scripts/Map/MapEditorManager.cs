#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Editor
{
    public sealed class MapEditorManager
    {
        private string brforeScenePath; // 현재 씬 저장용
        private Scene brforeScene;     // 임시 씬

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

            if (brforeScene.IsValid())
                return;

            brforeScenePath = EditorSceneManager.GetActiveScene().path;
            brforeScene = EditorSceneManager.OpenScene(useScenePath);

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

            if (!brforeScene.IsValid())
                return;

            Object.DestroyImmediate(mapBuilder);
            IsCreateData = false;

            EditorSceneManager.OpenScene(brforeScenePath);
            Debug.Log("Delete Completed");
        }
    }
}


#endif