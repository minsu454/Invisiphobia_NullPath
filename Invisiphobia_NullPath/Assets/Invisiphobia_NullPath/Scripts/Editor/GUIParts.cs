using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GUIParts
{
    /// <summary>
    /// UI들 가로로 만들어주는 함수
    /// </summary>
    public static void CreateHorizontal(params Action[] content)
    {
        GUILayout.BeginHorizontal();

        for (int i = 0; i < content.Length; i++)
            content[i].Invoke();

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 에리어 만들어주는 함수
    /// </summary>
    public static void CreateArea(Rect rect, Color color,  params Action<Rect>[] content)
    {
        GUI.color = color; 
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white; // 기본 색상 복원

        GUILayout.BeginArea(rect, GUIStyle.none);

        for (int i = 0; i < content.Length; i++)
            content[i].Invoke(rect);

        GUILayout.EndArea();
    }

    /// <summary>
    /// 경로에 있는 모든 파일들 반환하는 함수
    /// </summary>
    public static void LoadAllInFolder<T>(string folderPath, out T[] arr) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });
        T[] returnArr = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            returnArr[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        arr = returnArr;
    }

    /// <summary>
    /// 경로에 있는 모든 파일들 반환하는 함수
    /// </summary>
    public static void LoadAllInFolder<T>(string folderPath, out Dictionary<string, T> dict) where T : Component
    {
        string[] guids = AssetDatabase.FindAssets($"t:GameObject", new[] { folderPath });
        Dictionary<string, T> returnDict = new Dictionary<string, T>();

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            T component = go.GetComponent<T>();
            returnDict.Add(go.name, component);
        }

        dict = returnDict;
    }

    /// <summary>
    /// Texture 만들어주는 함수
    /// </summary>
    public static Texture2D CreateTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}