using System;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

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
        Color backgroundColor = color; 
        GUI.color = backgroundColor; 
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white; // 기본 색상 복원

        GUILayout.BeginArea(rect, GUIStyle.none);

        for (int i = 0; i < content.Length; i++)
            content[i].Invoke(rect);

        GUILayout.EndArea();
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