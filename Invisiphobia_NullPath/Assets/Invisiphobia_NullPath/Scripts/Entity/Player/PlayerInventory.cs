using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int curCount = 0;
    private int maxCount = 0;
    private const int handCount = 2;

    public Tablet Tablet;
    [SerializeField] private Transform itemHandParentTr;

    private bool isNotUse = false;
    public bool IsNotUse
    {
        get { return isNotUse; }
        set
        {
            isNotUse = value;
            UseEvent?.Invoke(isNotUse);
        }
    }
    public event Action<bool> UseEvent;
    public event Action<InHandItem> OnHandItemChanged;

    private readonly Stack<InHandItem> groundItemStack = new Stack<InHandItem>(2);
    private readonly Stack<GameObject> handItemStack = new Stack<GameObject>(2);
    private readonly Stack<Action<Transform>> interactStack = new Stack<Action<Transform>>(2);

    public void Init(Player player)
    {
        maxCount += handCount;

        Tablet.Init(player);

        player.PlayerController.playerPutDownActionEvent += DropItem;
        player.PlayerController.playerZoomClickActionEvent += OnZoomClick;
    }

    /// <summary>
    /// 테블릿 설정 함수
    /// </summary>
    public void SetTablet(int unLockTabletSkill)
    {
        Tablet.UnLockTabletSkill(unLockTabletSkill);
    }

    /// <summary>
    /// 아이템 설정 함수
    /// </summary>
    public void SetHand(InHandItem item, GameObject handPrefab, Action<Transform> interact = null)
    {
        if (item.ItemTable.itemCarryType == DesignEnums.ItemCarryType.None)
            return;

        if (IsNotUse)
            return;

        item.gameObject.SetActive(false);

        CleanInventory(item);
        groundItemStack.Push(item);
        SetTabletHidden();

        //Todo
        GameObject go = Instantiate(handPrefab, itemHandParentTr);
        handItemStack.Push(go);
        interactStack.Push(interact);

        OnHandItemChanged?.Invoke(item);
    }

    public bool IsLockOffItemInHand(int itemId)
    {
        if(!groundItemStack.TryPeek(out InHandItem item))
        {
            return false;
        }
        
        if(item.ItemTable.key == itemId)
        {
            return true;
        }

        return false;
    }

    private void CleanInventory(InHandItem item)
    {
        int temp = (int)item.ItemTable.itemCarryType + curCount;
        
        if (temp <= maxCount)
        {
            curCount = temp;
            return;
        }

        while (temp > maxCount)
        {
            DropItem();
            temp = (int)item.ItemTable.itemCarryType + curCount;
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
        if (IsNotUse)
            return;

        if (!groundItemStack.TryPeek(out InHandItem item))
            return;

        item.gameObject.SetActive(true);
        item.IconActive(true);
        item.transform.position = Camera.main.transform.forward * 0.1f + transform.position;

        curCount -= (int)item.ItemTable.itemCarryType;

        RemoveItem();
        SetTabletHidden();

        OnHandItemChanged?.Invoke(groundItemStack.TryPeek(out InHandItem nextItem) ? nextItem : null);
    }

    /// <summary>
    /// 아이템 삭제 함수
    /// </summary>
    private void RemoveItem()
    {
        groundItemStack.Pop();
        interactStack.Pop();

        GameObject handGo = handItemStack.Pop();
        Destroy(handGo);
    }

    /// <summary>
    /// 줌클릭 실행 이벤트 함수
    /// </summary>
    private void OnZoomClick()
    {
        if (!interactStack.TryPeek(out Action<Transform> action))
            return;

        if (action == null)
            return;


        InHandItem item = groundItemStack.Peek();

        item = groundItemStack.Peek();
        item.gameObject.SetActive(true);
        item.IconActive(true);
        item.transform.position = transform.position + transform.forward;

        curCount -= (int)item.ItemTable.itemCarryType;

        RemoveItem();

        action?.Invoke(transform);

        SetTabletHidden();

        OnHandItemChanged?.Invoke(groundItemStack.TryPeek(out InHandItem nextItem) ? nextItem : null);
    }

    public int Count()
    {
        return curCount;
    }

    /// <summary>
    /// 인벤토리 클리어
    /// </summary>
    private void Clear()
    {
        groundItemStack.Clear();
        handItemStack.Clear();
        interactStack.Clear();
    }
}
