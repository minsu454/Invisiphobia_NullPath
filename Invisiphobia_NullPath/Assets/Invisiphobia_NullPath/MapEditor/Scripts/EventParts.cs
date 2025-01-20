using Common.EnumExtensions;
using Common.Path;
using Common.StringEx;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventParts : MonoBehaviour, IParts
{
    [Header("Completed")]
    [SerializeField] private bool isCompleted = false;
    public bool IsCompleted {
        get { return isCompleted; }
        set { isCompleted = value; } }
    public List<Transform> OnCompleteTrList = new List<Transform>();

    [Header("TabletType")]
    [SerializeField] private TabletType tabletType;
    public TabletType TabletType { get { return tabletType; } }

    public string PuzzlePath { get { return puzzlePath; } }
    
    private Regex pattern = new Regex(@"^UI/Puzzle/(.+)\.prefab$");

    [SerializeField] private string puzzlePath;

    public void Init(bool isCompleted, string useGoPath, List<PointData> eventList)
    {
        IsCompleted = isCompleted;
        tabletType = PathToEnum(useGoPath);
        puzzlePath = tabletType != TabletType.None ? useGoPath : string.Empty;
        for (int i = 0; i < eventList.Count; i++)
        {
            OnCompleteTrList[i].position = eventList[i].Pos;
            OnCompleteTrList[i].rotation = eventList[i].Rot;
        }
    }

    /// <summary>
    /// 경로를 enum값으로 바꿔주는 함수
    /// </summary>
    private TabletType PathToEnum(string path)
    {
        Match match = pattern.Match(path);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid PuzzlePath format.");
        }

        return StringExtensions.StringToEnum<TabletType>(match.Groups[1].Value);
    }

    /// <summary>
    /// 경로 반환 함수
    /// </summary>
    public string GetPath()
    {
        return AddressablePath.PuzzlePath(tabletType.EnumToString());
    }
}
