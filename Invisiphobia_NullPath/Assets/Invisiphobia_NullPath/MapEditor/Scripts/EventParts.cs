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
    [SerializeField] private PuzzleType PuzzleType;

    public string PuzzlePath { get; private set; } = string.Empty;

    private Regex pattern = new Regex(@"^UI/Puzzle/(.+)\.prefab$");

    public void Init(string useGoPath, List<PointData> eventList)
    {
        try
        {
            PuzzleType = PathToEnum(useGoPath);
            PuzzlePath = PuzzleType != PuzzleType.None ? useGoPath : string.Empty;

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
    private PuzzleType PathToEnum(string path)
    {
        Match match = pattern.Match(path);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid PuzzlePath format.");
        }

        return StringExtensions.StringToEnum<PuzzleType>(match.Groups[1].Value);
    }

    /// <summary>
    /// 경로 반환 함수
    /// </summary>
    public string GetPath()
    {
        return AddressablePath.PuzzlePath(PuzzleType.EnumToString());
    }
}
