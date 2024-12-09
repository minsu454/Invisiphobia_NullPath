using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int curCount = 0;
    private int maxCount = 0;
    private const int handCount = 2;

    [SerializeField] private Tablet Tablet;

    private readonly Stack<InHandItem> groundItemStack = new Stack<InHandItem>(2);
    private readonly Stack<GameObject> handItemStack = new Stack<GameObject>(2);
    private readonly Stack<Action> interactStack = new Stack<Action>(2);

    public void Init(Player player)
    {
        maxCount += handCount;

        Tablet.Init(player);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    /// <summary>
    /// 테블릿 설정 함수
    /// </summary>
    public void SetTablet()
    {
        
    }

    /// <summary>
    /// 아이템 설정 함수
    /// </summary>
    public void SetHand(InHandItem item, GameObject handPrefab, Action interact = null)
    {
        if (item.Table.itemCarryType == DesignEnums.ItemCarryType.None)
            return;

        item.gameObject.SetActive(false);

        CleanInventory(item);
        groundItemStack.Push(item);
        SetTabletHidden();

        //Todo
        GameObject go = Instantiate(handPrefab, transform);
        handItemStack.Push(go);
        interactStack.Push(interact);
    }

    private void CleanInventory(InHandItem item)
    {
        int temp = (int)item.Table.itemCarryType + curCount;
        
        if (temp <= maxCount)
        {
            curCount = temp;
            return;
        }

        while (temp > maxCount)
        {
            DropItem();
            temp = (int)item.Table.itemCarryType + curCount;
        }

        curCount = temp;
    }

    private void SetTabletHidden()
    {
        if (curCount == maxCount)
            Tablet.Hidden();
        else
            Tablet.UnHidden();
    }

    /// <summary>
    /// 아이템 바닥에 버리는 함수
    /// </summary>
    private void DropItem() 
    {
        if (!groundItemStack.TryPeek(out InHandItem item))
            return;

        item = groundItemStack.Peek();
        item.gameObject.SetActive(true);
        item.IconActive(true);
        item.transform.position = transform.position + transform.forward;

        GameObject handGo = handItemStack.Pop();
        Destroy(handGo);

        curCount -= (int)item.Table.itemCarryType;

        RemoveItem();
    }

    /// <summary>
    /// 아이템 삭제 함수
    /// </summary>
    private void RemoveItem()
    {
        groundItemStack.Pop();
    }
}
