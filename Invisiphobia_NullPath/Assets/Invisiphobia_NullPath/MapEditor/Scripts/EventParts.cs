using Common.EnumExtensions;
using Common.Path;
using Common.StringEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventParts : MonoBehaviour, IParts
{
    public List<Transform> OnCompleteTrList = new List<Transform>();
    [SerializeField] private TabletType tabletType;
    public TabletType TabletType { get { return tabletType; } }

    public string PuzzlePath { get; private set; } = string.Empty;

    private Regex pattern = new Regex(@"^UI/Puzzle/(.+)\.prefab$");

    public void Init(string useGoPath, List<PointData> eventList)
    {
        try
        {
            tabletType = PathToEnum(useGoPath);
            PuzzlePath = tabletType != TabletType.None ? useGoPath : string.Empty;

            for (int i = 0; i < eventList.Count; i++)
            {
                OnCompleteTrList[i].position = eventList[i].Pos;
                OnCompleteTrList[i].rotation = eventList[i].Rot;
            }
        }
        catch
        {
            throw new ArgumentNullException(gameObject.name);
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
