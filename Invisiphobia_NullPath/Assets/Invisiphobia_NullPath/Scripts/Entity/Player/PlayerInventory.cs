using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int curCount = 0;
    private int maxCount = 0;
    private const int handCount = 2;

    [SerializeField] private Tablet Tablet;
    private List<HandheldItem> handList = new List<HandheldItem>(2);
    private List<HandheldItem> bagList = new List<HandheldItem>();

    //private HashSet<> bagSet;     생성해줄 아이템

    public void Init()
    {
        maxCount += handCount;
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
    public void SetHand(HandheldItem item, DesignEnums.ItemCarryType type)
    {
        if (type == DesignEnums.ItemCarryType.None)
            return;

        int temp = (int)type + curCount;

        item.gameObject.SetActive(false);

        if (temp > maxCount)
            OverHandItem(temp);

        handList.Add(item);

        curCount = temp;

        //Todo
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
        HandheldItem item;

        item = handList[0];
        item.gameObject.SetActive(true);
    }

    /// <summary>
    /// 아이템 설정 함수
    /// </summary>
    public void SetBag(HandheldItem item, DesignEnums.ItemCarryType type)
    {

    }
}
