/// =========================================================================
///     주의: 자동 생성된 코드입니다. 파일을 수정할 경우 재생성된 코드에 의해
///     작업 내용이 사라질 수 있습니다.
/// =========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class InteractTextTable
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 상호작용
    /// </summary>
    public string interactText;

    /// <summary>
    /// 영어
    /// </summary>
    public string english;

    /// <summary>
    /// 한국어
    /// </summary>
    public string korean;

}
public class InteractTextTableLoader
{
    public List<InteractTextTable> ItemsList { get; private set; }
    public Dictionary<int, InteractTextTable> ItemsDict { get; private set; }

    public InteractTextTableLoader(string path = "JSON/InteractTextTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, InteractTextTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<InteractTextTable> Items;
    }

    public InteractTextTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public InteractTextTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
