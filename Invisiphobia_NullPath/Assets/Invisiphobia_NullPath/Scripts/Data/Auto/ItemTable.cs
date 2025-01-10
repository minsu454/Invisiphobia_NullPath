/// =========================================================================
///     주의: 자동 생성된 코드입니다. 파일을 수정할 경우 재생성된 코드에 의해
///     작업 내용이 사라질 수 있습니다.
/// =========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ItemTable
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
    /// 언어별이름
    /// </summary>
    public int itemName;

    /// <summary>
    /// 에러메시지
    /// </summary>
    public List<int> errorMessage;

    /// <summary>
    /// 경로
    /// </summary>
    public string path;

    /// <summary>
    /// 사용횟수
    /// </summary>
    public int useCount;

    /// <summary>
    /// 아이템타입
    /// </summary>
    public DesignEnums.ItemCarryType itemCarryType;

    /// <summary>
    /// 상호작용텍스트
    /// </summary>
    public List<int> interactText;

    /// <summary>
    /// 동작텍스트(-1은 사용안함)
    /// </summary>
    public int actionText;

}
public class ItemTableLoader
{
    public List<ItemTable> ItemsList { get; private set; }
    public Dictionary<int, ItemTable> ItemsDict { get; private set; }

    public ItemTableLoader(string path = "JSON/ItemTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ItemTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ItemTable> Items;
    }

    public ItemTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ItemTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
