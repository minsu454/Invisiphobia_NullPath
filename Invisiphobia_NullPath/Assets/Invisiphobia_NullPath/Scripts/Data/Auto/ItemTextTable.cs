/// =========================================================================
///     주의: 자동 생성된 코드입니다. 파일을 수정할 경우 재생성된 코드에 의해
///     작업 내용이 사라질 수 있습니다.
/// =========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ItemTextTable
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 아이템이름
    /// </summary>
    public string name;

    /// <summary>
    /// english
    /// </summary>
    public string english;

    /// <summary>
    /// 한국어
    /// </summary>
    public string korean;

}
public class ItemTextTableLoader
{
    public List<ItemTextTable> ItemsList { get; private set; }
    public Dictionary<int, ItemTextTable> ItemsDict { get; private set; }

    public ItemTextTableLoader(string path = "JSON/ItemTextTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ItemTextTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ItemTextTable> Items;
    }

    public ItemTextTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ItemTextTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
