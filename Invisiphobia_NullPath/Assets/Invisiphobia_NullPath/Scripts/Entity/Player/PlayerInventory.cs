using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int curCount = 0;
    private int maxCount = 0;
    private const int handCount = 2;

    [SerializeField] private Tablet Tablet;
    private List<ThrowItem> handList = new List<ThrowItem>(2);
    private List<ThrowItem> bagList = new List<ThrowItem>();

    //private HashSet<> bagSet;     생성해줄 아이템

    public void Init(Player player)
    {
        maxCount += handCount;
        Tablet.Init(player);
    }

    /// <summary>
    /// 테블릿 설정 함수
    /// </summary>
    public void SetTablet(Tablet tablet)
    {
        if (tablet != null)
            return;

        Tablet = tablet;
    }

    /// <summary>
    /// 아이템 설정 함수
    /// </summary>
    public void SetHand(ThrowItem item, ItemTable table)
    {
        if (table.itemCarryType == DesignEnums.ItemCarryType.None)
            return;

        int temp = (int)table.itemCarryType + curCount;

        item.gameObject.SetActive(false);

        if (temp > maxCount)
            OverHandItem(temp);

        handList.Add(item);

        curCount = temp;

        //Todo
        Debug.Log(table.name);
    }

    public void SetHand(ThrowItem item, ItemTable table, GameObject handPrefab)
    {
        if (table.itemCarryType == DesignEnums.ItemCarryType.None)
            return;

        int temp = (int)table.itemCarryType + curCount;

        item.gameObject.SetActive(false);

        if (temp > maxCount)
            OverHandItem(temp);

        handList.Add(item);

        curCount = temp;

        //Todo
        GameObject go = Instantiate(handPrefab);
        ThrowObject brick = go.GetComponent<ThrowObject>();
        brick.Interact(Player.Instance); // interact -> init
        //interact - action
        Debug.Log(table.name);
    }

    private int OverHandItem(int temp)
    {
        if (handList.Count == 0)
        {
            return temp;
        }

        if (handList.Count == handCount)
        {
            RemoveItem();
        }

        RemoveItem();

        temp = maxCount;

        return temp;
    }

    /// <summary>
    /// 아이템 삭제 함수
    /// </summary>
    private void RemoveItem()
    {
        DropItem();
        handList.RemoveAt(0);
    }

    private void DropItem() 
    { 
        ThrowItem item;

        item = handList[0];
        item.gameObject.SetActive(true);
    }

    /// <summary>
    /// 아이템 설정 함수
    /// </summary>
    public void SetBag(ThrowItem item, DesignEnums.ItemCarryType type)
    {

    }
}
