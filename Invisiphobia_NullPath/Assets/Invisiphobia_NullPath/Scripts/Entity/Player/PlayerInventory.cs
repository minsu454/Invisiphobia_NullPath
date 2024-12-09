using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int curCount = 0;
    private int maxCount = 0;
    private const int handCount = 2;

    [SerializeField] private Tablet Tablet;
    private List<InHandItem> handList = new List<InHandItem>(2);

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
    public void SetHand(InHandItem item, ItemTable table, GameObject handPrefab)
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
        GameObject go = Instantiate(handPrefab, transform);
    }

    private int OverHandItem(int temp)
    {
        if (handList.Count == handCount)
        {
            DropItem();
            RemoveItem();
        }

        DropItem();
        RemoveItem();

        temp = maxCount;

        return temp;
    }

    /// <summary>
    /// 아이템 삭제 함수
    /// </summary>
    public void RemoveItem()
    {
        if (handList.Count == 0)
            return;

        DropItem();
        handList.RemoveAt(0);

        //if(handList.Count != 0)

    }

    /// <summary>
    /// 아이템 바닥에 버리는 함수
    /// </summary>
    private void DropItem() 
    {
        InHandItem item;

        item = handList[0];
        item.gameObject.SetActive(true);
    }
}
