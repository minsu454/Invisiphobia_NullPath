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

        if (temp > maxCount)
            RemoveItem();

        item.gameObject.SetActive(false);
        handList.Add(item);
        curCount = temp;

        //Todo
    }

    /// <summary>
    /// 아이템 삭제 함수
    /// </summary>
    public void RemoveItem()
    {
        if (handList.Count == 0)
            return;

        //Todo

        HandheldItem item = handList[0];
        item.gameObject.SetActive(true);

        handList.RemoveAt(0);
    }

    /// <summary>
    /// 아이템 설정 함수
    /// </summary>
    public void SetBag(HandheldItem item, DesignEnums.ItemCarryType type)
    {
        
    }
}
